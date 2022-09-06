using System.Device.Gpio;
using System.Device.Spi;

namespace TftSpiDemo
{
    public class RpiSpi
    {
        private SpiDevice spi_device;
        private bool command;
        private GpioController tft_dc_gpio24; // command = Low, data = High

        public RpiSpi()
        {
            // busId 0, chipSelectLine 0 (CE0, GPIO8)
            var spi_settings = new SpiConnectionSettings(0, 0) {
                ClockFrequency = 26000000, // max 26 MHz
                Mode = SpiMode.Mode0,
            };
            spi_device = SpiDevice.Create(spi_settings);

            tft_dc_gpio24 = new GpioController();
            tft_dc_gpio24.OpenPin(24, PinMode.Output);
        }

        public void write_reg(ST7735Command cmd, byte data)
        {
            dc_set_low();
            spi_device.WriteByte((byte) cmd);

            dc_set_high();
            spi_device.WriteByte(data);
        }

        public void write_command_delay(ST7735Command cmd, int delay)
        {
            dc_set_low();
            spi_device.WriteByte((byte) cmd);

            if (delay != 0) {
                Thread.Sleep(delay);
            }
        }

        public void write_data_delay(byte data, int delay)
        {
            dc_set_high();
            spi_device.WriteByte(data);

            if (delay != 0) {
                Thread.Sleep(delay);
            }
        }

        public void write_data_delay(byte[] data, int delay)
        {
            var span = new ReadOnlySpan<byte>(data);
            dc_set_high();
            spi_device.Write(span);

            if (delay != 0) {
                Thread.Sleep(delay);
            }
        }

        private void dc_set_low()
        {
            if (!command) {
                tft_dc_gpio24.Write(24, PinValue.Low);
                command = true;
            }
        }

        private void dc_set_high()
        {
            if (command) {
                tft_dc_gpio24.Write(24, PinValue.High);
                command = false;
            }
        }
    }
}
