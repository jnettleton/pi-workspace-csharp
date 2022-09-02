namespace OledI2cDemo
{
    internal class Program
    {
        // static GpioController controller = new GpioController(PinNumberingScheme.Logical);
        // {
        // };
        static void Main(string[] args)
        {
            var oled = new ssd1306();
            oled.init();

            oled.clearBuffer();
            oled.drawString(0, 0, "123456789012345678901234567890", 1, 1);

            oled.drawFillRect(0, 16, 32, 15, 1);
            oled.drawString(10, 20, "EX", 1, 0);

            oled.drawRect(32, 16, 32, 15, 1);
            oled.drawString(42, 20, "SC", 1, 1);

            oled.drawFillRect(64, 16, 32, 15, 1);
            oled.drawString(74, 20, "R", 1, 0);

            oled.drawRect(96, 16, 31, 15, 1);
            oled.drawString(106, 20, "S", 1, 1);

            oled.refresh();
        }
    }
}
