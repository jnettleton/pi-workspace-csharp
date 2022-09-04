using TftSpiDemo;

namespace OledI2cDemo
{
    internal class Program
    {
        // static GpioController controller = new GpioController(PinNumberingScheme.Logical);
        // {
        // };
        static void Main(string[] args)
        {
            var display = new RpiTftDisplay();
            display.initialize();
            display.fill_screen(ST7735Color.RED);
        }
    }
}
