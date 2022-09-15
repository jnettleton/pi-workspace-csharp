using System.Device.Gpio;
using System.Device.Spi;

namespace TftSpiDemo
{
    public class RpiSpi
    {
        private SpiDevice spi_device;
        private byte dc_pin;
        private bool command;
        private GpioController tft_dc; // command = Low, data = High

        public RpiSpi(byte dc)
        {
            dc_pin = dc;

            // busId 0, chipSelectLine 0 (CE0, GPIO8)
            var spi_settings = new SpiConnectionSettings(0, 0) {
                ClockFrequency = 31200000, //500000, // 26000000, // max 26 MHz
                DataBitLength = 8,
                Mode = SpiMode.Mode0,
            };
            spi_device = SpiDevice.Create(spi_settings);

            tft_dc = new GpioController();
            tft_dc.OpenPin(24, PinMode.Output);
            tft_dc.Write(dc_pin, PinValue.Low);
            command = true;
        }

        public void write_reg(ST7735Command cmd, byte data)
        {
            dc_set_low();
            spi_device.WriteByte((byte) cmd);

            dc_set_high();
            spi_device.WriteByte(data);
        }

        public void write_command(ST7735Command cmd, int delay = 0)
        {
            dc_set_low();
            spi_device.WriteByte((byte) cmd);

            if (delay != 0) {
                Thread.Sleep(delay);
            }
        }

        public void write_data(byte data, int delay = 0)
        {
            dc_set_high();
            spi_device.WriteByte(data);

            if (delay != 0) {
                Thread.Sleep(delay);
            }
        }

        public void write_data_length(byte[] data, int length, int delay = 0)
        {
            dc_set_high();
            if (length > data.Length) return;

            if (length <= 32)
            {
                for (var i = 0; i < length; i++)
                {
                    spi_device.WriteByte(data[i]);
                }
            } else {
                var span = new ReadOnlySpan<byte>(data, 0, length);
                spi_device.Write(span);
            }

            if (delay != 0) {
                Thread.Sleep(delay);
            }
        }

        public void write_data(byte[] data, int delay = 0)
        {
            dc_set_high();
            if (data.Length <= 32)
            {
                for (var i = 0; i < data.Length; i++)
                {
                    spi_device.WriteByte(data[i]);
                }
            } else {
                var span = new ReadOnlySpan<byte>(data);
                spi_device.Write(span);
            }

            if (delay != 0) {
                Thread.Sleep(delay);
            }
        }

        private void dc_set_low(int delay = 0)
        {
            if (!command)
            {
                tft_dc.Write(dc_pin, PinValue.Low);
                command = true;
                if (delay != 0) {
                    Thread.Sleep(delay);
                }
            }
        }

        private void dc_set_high(int delay = 0)
        {
            if (command)
            {
                tft_dc.Write(dc_pin, PinValue.High);
                command = false;
                if (delay != 0) {
                    Thread.Sleep(delay);
                }
            }
        }
    }
}
