//#define ENABLE_COLOR18

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

            display.init_screen_size(0, 0, 320, 480);
            display.set_rotation(TFTRotate.Degrees90);
#if ENABLE_COLOR18
            display.fill_screen(ST7735Color18.BLUE);

            display.draw_text(10, 10, "Hello, World!", ST7735Color18.WHITE, ST7735Color18.BLUE, 4);

            display.fill_rectangle(0, 280, 120, 40, ST7735Color18.RED);
            display.draw_text(12, 292, "Executed", ST7735Color18.WHITE, ST7735Color18.RED, 2);

            display.fill_rectangle(120, 280, 120, 40, ST7735Color18.YELLOW);
            display.draw_text(126, 292, "Scheduled", ST7735Color18.BLACK, ST7735Color18.YELLOW, 2);

            display.fill_rectangle(240, 280, 120, 40, ST7735Color18.MAGENTA);
            display.draw_text(252, 292, "Routines", ST7735Color18.BLACK, ST7735Color18.MAGENTA, 2);

            display.fill_rectangle(360, 280, 120, 40, ST7735Color18.BLACK);
            display.draw_text(384, 292, "Scenes", ST7735Color18.WHITE, ST7735Color18.BLACK, 2);
#else
            display.fill_screen(ST7735Color.BLUE);

            display.draw_text(42, 4, "Hello, World!", ST7735Color.WHITE, 2);

            display.fill_rectangle(0, 280, 120, 40, ST7735Color.RED);
            display.fill_rectangle(0, 280, 120, 2, ST7735Color.WHITE);
            display.fill_rectangle(0, 318, 120, 2, ST7735Color.WHITE);
            display.fill_rectangle(0, 280, 2, 40, ST7735Color.WHITE);
            display.fill_rectangle(118, 280, 2, 40, ST7735Color.WHITE);
            display.draw_text(12, 292, "Executed", ST7735Color.WHITE, 2);

            display.fill_rectangle(120, 280, 120, 40, ST7735Color.YELLOW);
            display.draw_text(126, 292, "Scheduled", ST7735Color.BLACK, 2);

            display.fill_rectangle(240, 280, 120, 40, ST7735Color.MAGENTA);
            display.draw_text(252, 292, "Routines", ST7735Color.BLACK, 2);

            display.fill_rectangle(360, 280, 120, 40, ST7735Color.BLACK);
            display.draw_text(384, 292, "Scenes", ST7735Color.WHITE, 2);
#endif

            display.draw_bitmap(0, 4, 32, 32, Images.PI);
        }
    }
}
