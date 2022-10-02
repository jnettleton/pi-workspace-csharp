using System.Device.Gpio;

namespace TftSpiDemo
{
    public class RpiTftTouch
    {
        RpiSpi rpi_spi;

        public RpiTftTouch(RpiSpi spi)
        {
            rpi_spi = spi;
        }

        public void initialize()
        {
            select();
        }

        public void select()
        {
            rpi_spi.select_touch();
        }
    }
}
