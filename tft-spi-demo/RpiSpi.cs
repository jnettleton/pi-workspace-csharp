using System.Device.Gpio;
using System.Device.Spi;

namespace TftSpiDemo
{
    public class RpiSpi
    {
        // const int TFT_CD_DELAY = 0;
        const int TFT_MAX_BYTES = 8; //4 * 1024;
        const int TFT_RST = 25;
        const int TFT_DC = 24;
        const int TFT_CS_DISPLAY = 8;
        const int TFT_CS_TOUCH = 7;

        private SpiDevice spi_device;
        private bool command;
        private bool display;
        private GpioController tft_rst; // command = Low, data = High
        private GpioController tft_dc; // command = Low, data = High
        private GpioController tft_cs_display; // command = Low, data = High
        private GpioController tft_cs_touch; // command = Low, data = High

        public RpiSpi()
        {
            // busId 0, chipSelectLine 0 (CE0, GPIO8)
            var spi_settings = new SpiConnectionSettings(0, 0) {
                ClockFrequency = 31200000, //500000, // 26000000, // max 26 MHz
                DataBitLength = 8,
                Mode = SpiMode.Mode0,
            };
            spi_device = SpiDevice.Create(spi_settings);

            tft_rst = new GpioController();
            tft_rst.OpenPin(TFT_RST, PinMode.Output);

            tft_cs_display = new GpioController();
            tft_cs_display.OpenPin(TFT_CS_DISPLAY, PinMode.Output);

            tft_cs_touch = new GpioController();
            tft_cs_touch.OpenPin(TFT_CS_TOUCH, PinMode.Output);

            tft_dc = new GpioController();
            tft_dc.OpenPin(TFT_DC, PinMode.Output);

            tft_dc.Write(TFT_DC, PinValue.Low);
            command = true;

            display = false;
            select_display();
        }

        public void write_reg(ST7735Command cmd, byte data)
        {
            lock (this)
            {
                dc_set_low();
                spi_device.WriteByte((byte) cmd);

                dc_set_high();
                spi_device.WriteByte(data);
            }
        }

        public void write_command(ST7735Command cmd, int delay = 0)
        {
            lock (this)
            {
                dc_set_low();
                spi_device.WriteByte((byte) cmd);
            }

            if (delay != 0)
            {
                Thread.Sleep(delay);
            }
        }

        public void write_data(byte data, int delay = 0)
        {
            lock (this)
            {
                dc_set_high();
                spi_device.WriteByte(data);
            }

            if (delay != 0)
            {
                Thread.Sleep(delay);
            }
        }

        public void write_data_length(byte[] data, int length, int delay = 0)
        {
            lock (this)
            {
                dc_set_high();
                if (length > data.Length) return;

                if (length <= TFT_MAX_BYTES)
                {
                    for (var i = 0; i < length; i++)
                    {
                        spi_device.WriteByte(data[i]);
                    }
                } else {
                    var span = new ReadOnlySpan<byte>(data, 0, length);
                    spi_device.Write(span);
                    Thread.Sleep(0);
                }
            }

            if (delay != 0)
            {
                Thread.Sleep(delay);
            }
        }

        public void write_data(byte[] data, int delay = 0)
        {
            lock (this)
            {
                dc_set_high();
                if (data.Length <= TFT_MAX_BYTES)
                {
                    for (var i = 0; i < data.Length; i++)
                    {
                        spi_device.WriteByte(data[i]);
                    }
                } else {
                    var span = new ReadOnlySpan<byte>(data);
                    spi_device.Write(span);
                    Thread.Sleep(0);
                }
            }

            if (delay != 0)
            {
                Thread.Sleep(delay);
            }
        }

        public byte read_byte()
        {
            lock (this)
            {
                return spi_device.ReadByte();
            }
        }

        public byte[] read_data()
        {
            lock (this)
            {
                var buffer = new byte[1024];
                Span<byte> data = new Span<byte>(buffer);
                spi_device.Read(data);
                
                return buffer;
            }
        }

        private void dc_set_low(int delay = 0)
        {
            if (!command)
            {
                if (delay != 0)
                {
                    Thread.Sleep(delay);
                }

                tft_dc.Write(TFT_DC, PinValue.Low);
                command = true;
            }
        }

        private void dc_set_high(int delay = 0)
        {
            if (command)
            {
                if (delay != 0)
                {
                    Thread.Sleep(delay);
                }
                
                tft_dc.Write(TFT_DC, PinValue.High);
                command = false;
            }
        }

        public void reset()
        {
            tft_rst.Write(TFT_RST, PinValue.High);
            Thread.Sleep(1);
            tft_rst.Write(TFT_RST, PinValue.Low);
            Thread.Sleep(1);
            tft_rst.Write(TFT_RST, PinValue.High);
            Thread.Sleep(120);
        }

        public void select_touch()
        {
            if (display)
            {
                display = false;
                tft_cs_display.Write(TFT_CS_DISPLAY, PinValue.High);
                tft_cs_touch.Write(TFT_CS_TOUCH, PinValue.Low);
            }
        }

        public void select_display()
        {
            if (!display)
            {
                display = true;
                tft_cs_touch.Write(TFT_CS_TOUCH, PinValue.High);
                tft_cs_display.Write(TFT_CS_DISPLAY, PinValue.Low);
            }
        }
    }
}
