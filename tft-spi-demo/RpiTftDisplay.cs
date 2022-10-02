//#define ENABLE_COLOR18

using System.Device.Gpio;

namespace TftSpiDemo
{
    public enum Ili9486Command : byte
    {
        // NOP = 0x00, // non operation
        SWRESET = 0x01, // soft reset
        // RDDID = 0x04, // read device id
        // RDDST = 0x09,
        // SLPIN = 0x10, //sleep on
        SLPOUT = 0x11, // sleep off
        // PTLON = 0x12, // partial mode
        NORON = 0x13, // normal display
        INVOFF = 0x20, // display invert off
        // INVON = 0x21, // display invert on
        // DISPOFF = 0x28, // display off
        DISPON = 0x29, // display on
        // TFT_IDLE_MDODE_ON = 0x39, // idle mode on
        // TFT_IDLE_MODE_OFF = 0x38, // idle mode off
        CASET = 0x2A, // column address set
        RASET = 0x2B, // row/page address set
        RAMWR = 0x2C, // memory write
        // RAMRD = 0x2E, // memory read
        // PTLAR = 0x30, // partial area
        // VSCRDEF = 0x33, // vertical scroll def

        MADCTL = 0x36, // memory access control
        // VSCRSADD = 0x37, //vertical access control
        COLMOD = 0x3A, // interface pixel format

        // frame rate control
        FRMCTR1 = 0xB1, // normal
        FRMCTR2 = 0xB2, // idle
        FRMCTR3 = 0xB3, // partial

        INVCTR = 0xB4, // display inversion control
        DISSET5 = 0xB6, // display function set

        // power control
        PWCTR1 = 0xC0,
        PWCTR2 = 0xC1,
        PWCTR3 = 0xC2,
        PWCTR4 = 0xC3,
        PWCTR5 = 0xC4,
        // PWCTR6 = 0xFC,

        VMCTR1 = 0xC5, // VCOM control 1

        // RDDID1 = 0xDA,
        // RDDID2 = 0xDB,
        // RDDID3 = 0xDC,
        // RDDID4 = 0xDD,

        GMCTRP1 = 0xE0, // positive gamma correction setting
        GMCTRN1 = 0xE1, // negative gamma correction setting
    }

    public enum ST7735Command : byte
    {
        // NOP = 0x00, // non operation
        SWRESET = 0x01, // soft reset
        // RDDID = 0x04, // read device id
        // RDDST = 0x09,
        // SLPIN = 0x10, //sleep on
        SLPOUT = 0x11, // sleep off
        // PTLON = 0x12, // partial mode
        NORON = 0x13, // normal display
        INVOFF = 0x20, // display invert off
        INVON = 0x21, // display invert on
        // DISPOFF = 0x28, // display off
        DISPON = 0x29, // display on
        // TFT_IDLE_MDODE_ON = 0x39, // idle mode on
        // TFT_IDLE_MODE_OFF = 0x38, // idle mode off
        CASET = 0x2A, // column address set
        RASET = 0x2B, // row/page address set
        RAMWR = 0x2C, // memory write
        // RAMRD = 0x2E, // memory read
        // PTLAR = 0x30, // partial area
        // VSCRDEF = 0x33, // vertical scroll def

        MADCTL = 0x36, // memory access control
        // VSCRSADD = 0x37, //vertical access control
        COLMOD = 0x3A, // interface pixel format

        // interface mode control
        IFMCTL = 0xB0,

        // frame rate control
        FRMCTR1 = 0xB1, // normal
        FRMCTR2 = 0xB2, // idle
        FRMCTR3 = 0xB3, // partial

        INVCTR = 0xB4, // display inversion control
        DISSET5 = 0xB6, // display function set

        // power control
        PWCTR1 = 0xC0,
        PWCTR2 = 0xC1,
        PWCTR3 = 0xC2,
        PWCTR4 = 0xC3,
        PWCTR5 = 0xC4,
        // PWCTR6 = 0xFC,

        VMCTR1 = 0xC5, // VCOM control 1

        // RDDID1 = 0xDA,
        // RDDID2 = 0xDB,
        // RDDID3 = 0xDC,
        // RDDID4 = 0xDD,

        GMCTRP1 = 0xE0, // positive gamma correction setting
        GMCTRN1 = 0xE1, // negative gamma correction setting
        DGCTL1 = 0xE2, // digital game control 1
    }

    [Flags]
    enum ST7735MadControl {
        MADCTL_MY = 0x80,
        MADCTL_MX = 0x40,
        MADCTL_MV = 0x20,
        MADCTL_ML = 0x10,
        MADCTL_RGB = 0x00,
        MADCTL_BGR = 0x08,
        MADCTL_MH = 0x04
    }

#if ENABLE_COLOR18
    public enum ST7735Color18 : UInt32
    {
        BLACK = 0x000000,
        BLUE = 0x0000FC,
        RED = 0xFC0000,
        GREEN = 0x00FC00,
        CYAN = 0x00FCFC, // 00, 3F, 1F
        MAGENTA = 0xFC00FC, // 1F, 00, 1F
        YELLOW = 0xFCF800, // 1F, 3E, 00
        WHITE = 0xFFFFFF,
        TAN = 0x344044, // 1D, 10, 11
        GREY = 0x4C9844, // 13, 26, 11
        BROWN = 0x604004 // 18, 10, 01
    }
#else    
    public enum ST7735Color : ushort
    {
        BLACK = 0x0000,
        BLUE = 0x001F,
        RED = 0xF800,
        GREEN = 0x07E0,
        CYAN = 0x07FF,
        MAGENTA = 0xF81F,
        YELLOW = 0xFFE0,
        WHITE = 0xFFFF,
        TAN = 0xED01,
        GREY = 0x9CD1,
        BROWN = 0x6201
    }
#endif

    public enum TFTMode
    {
        // NORMAL,
        // PARTIAL,
        // IDLE,
        // SLEEP,
        // INVERT,
        // DISPLAYON,
        DISPLAYOFF,
    }

    public enum TFTRotate {
        Degrees0,
        Degrees90,
        Degrees180,
        Degrees270,
    }

    public enum TFTPcbType
    {
        Red,
        Green,
        Black,
        None
    }

    public class RpiTftDisplay
    {
        RpiSpi rpi_spi;
        TFTMode mode;
        TFTPcbType pcb_type;

        ushort x_start;
        ushort y_start;
        //// cursor_x: u16,
        //// cursor_y: u16,

        ushort tft_width;
        ushort tft_height;
        ushort tft_start_width;
        ushort tft_start_height;
        ushort start_column;
        ushort start_row;

        TFTRotate tft_rotation;
#if ENABLE_COLOR18
        ST7735Color18 txt_color;
        ST7735Color18 txt_bg_color;
#else
        ST7735Color txt_color;
        ST7735Color txt_bg_color;
#endif
        byte[] font = new byte[] {
            0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x5F, 0x00, 0x00,
            0x00, 0x07, 0x00, 0x07, 0x00,
            0x14, 0x7F, 0x14, 0x7F, 0x14,
            0x24, 0x2A, 0x7F, 0x2A, 0x12,
            0x23, 0x13, 0x08, 0x64, 0x62,
            0x36, 0x49, 0x56, 0x20, 0x50,
            0x00, 0x08, 0x07, 0x03, 0x00,
            0x00, 0x1C, 0x22, 0x41, 0x00,
            0x00, 0x41, 0x22, 0x1C, 0x00,
            0x2A, 0x1C, 0x7F, 0x1C, 0x2A,
            0x08, 0x08, 0x3E, 0x08, 0x08,
            0x00, 0x80, 0x70, 0x30, 0x00,
            0x08, 0x08, 0x08, 0x08, 0x08,
            0x00, 0x00, 0x60, 0x60, 0x00,
            0x20, 0x10, 0x08, 0x04, 0x02,
            0x3E, 0x51, 0x49, 0x45, 0x3E,
            0x00, 0x42, 0x7F, 0x40, 0x00,
            0x72, 0x49, 0x49, 0x49, 0x46,
            0x21, 0x41, 0x49, 0x4D, 0x33,
            0x18, 0x14, 0x12, 0x7F, 0x10,
            0x27, 0x45, 0x45, 0x45, 0x39,
            0x3C, 0x4A, 0x49, 0x49, 0x31,
            0x41, 0x21, 0x11, 0x09, 0x07,
            0x36, 0x49, 0x49, 0x49, 0x36,
            0x46, 0x49, 0x49, 0x29, 0x1E,
            0x00, 0x00, 0x14, 0x00, 0x00,
            0x00, 0x40, 0x34, 0x00, 0x00,
            0x00, 0x08, 0x14, 0x22, 0x41,
            0x14, 0x14, 0x14, 0x14, 0x14,
            0x00, 0x41, 0x22, 0x14, 0x08,
            0x02, 0x01, 0x59, 0x09, 0x06,
            0x3E, 0x41, 0x5D, 0x59, 0x4E,
            0x7C, 0x12, 0x11, 0x12, 0x7C,
            0x7F, 0x49, 0x49, 0x49, 0x36,
            0x3E, 0x41, 0x41, 0x41, 0x22,
            0x7F, 0x41, 0x41, 0x41, 0x3E,
            0x7F, 0x49, 0x49, 0x49, 0x41,
            0x7F, 0x09, 0x09, 0x09, 0x01,
            0x3E, 0x41, 0x41, 0x51, 0x73,
            0x7F, 0x08, 0x08, 0x08, 0x7F,
            0x00, 0x41, 0x7F, 0x41, 0x00,
            0x20, 0x40, 0x41, 0x3F, 0x01,
            0x7F, 0x08, 0x14, 0x22, 0x41,
            0x7F, 0x40, 0x40, 0x40, 0x40,
            0x7F, 0x02, 0x1C, 0x02, 0x7F,
            0x7F, 0x04, 0x08, 0x10, 0x7F,
            0x3E, 0x41, 0x41, 0x41, 0x3E,
            0x7F, 0x09, 0x09, 0x09, 0x06,
            0x3E, 0x41, 0x51, 0x21, 0x5E,
            0x7F, 0x09, 0x19, 0x29, 0x46
        };
        byte[] font2 = new byte[] {
            0x26, 0x49, 0x49, 0x49, 0x32,
            0x03, 0x01, 0x7F, 0x01, 0x03,
            0x3F, 0x40, 0x40, 0x40, 0x3F,
            0x1F, 0x20, 0x40, 0x20, 0x1F,
            0x3F, 0x40, 0x38, 0x40, 0x3F,
            0x63, 0x14, 0x08, 0x14, 0x63,
            0x03, 0x04, 0x78, 0x04, 0x03,
            0x61, 0x59, 0x49, 0x4D, 0x43,
            0x00, 0x7F, 0x41, 0x41, 0x41,
            0x02, 0x04, 0x08, 0x10, 0x20,
            0x00, 0x41, 0x41, 0x41, 0x7F,
            0x04, 0x02, 0x01, 0x02, 0x04,
            0x40, 0x40, 0x40, 0x40, 0x40,
            0x00, 0x03, 0x07, 0x08, 0x00,
            0x20, 0x54, 0x54, 0x78, 0x40,
            0x7F, 0x28, 0x44, 0x44, 0x38,
            0x38, 0x44, 0x44, 0x44, 0x28,
            0x38, 0x44, 0x44, 0x28, 0x7F,
            0x38, 0x54, 0x54, 0x54, 0x18,
            0x00, 0x08, 0x7E, 0x09, 0x02,
            0x18, 0xA4, 0xA4, 0x9C, 0x78,
            0x7F, 0x08, 0x04, 0x04, 0x78,
            0x00, 0x44, 0x7D, 0x40, 0x00,
            0x20, 0x40, 0x40, 0x3D, 0x00,
            0x7F, 0x10, 0x28, 0x44, 0x00,
            0x00, 0x41, 0x7F, 0x40, 0x00,
            0x7C, 0x04, 0x78, 0x04, 0x78,
            0x7C, 0x08, 0x04, 0x04, 0x78,
            0x38, 0x44, 0x44, 0x44, 0x38,
            0xFC, 0x18, 0x24, 0x24, 0x18,
            0x18, 0x24, 0x24, 0x18, 0xFC,
            0x7C, 0x08, 0x04, 0x04, 0x08,
            0x48, 0x54, 0x54, 0x54, 0x24,
            0x04, 0x04, 0x3F, 0x44, 0x24,
            0x3C, 0x40, 0x40, 0x20, 0x7C,
            0x1C, 0x20, 0x40, 0x20, 0x1C,
            0x3C, 0x40, 0x30, 0x40, 0x3C,
            0x44, 0x28, 0x10, 0x28, 0x44,
            0x4C, 0x90, 0x90, 0x90, 0x7C,
            0x44, 0x64, 0x54, 0x4C, 0x44,
            0x00, 0x08, 0x36, 0x41, 0x00,
            0x00, 0x00, 0x77, 0x00, 0x00,
            0x00, 0x41, 0x36, 0x08, 0x00,
            0x02, 0x01, 0x02, 0x04, 0x02
        };

        public RpiTftDisplay(RpiSpi spi)
        {
            rpi_spi = spi;

            mode = TFTMode.DISPLAYOFF;
            pcb_type = TFTPcbType.None;

            x_start = 0;
            y_start = 0;
            // cursor_x: 0,
            // cursor_y: 0,
            start_column = 0;
            start_row = 0;

            tft_height = 480;
            tft_width = 320;
            tft_start_height = tft_height;
            tft_start_width = tft_width;

            // tft_buffer: [],
#if ENABLE_COLOR18
            txt_color = ST7735Color18.WHITE;
            txt_bg_color = ST7735Color18.BLACK;
#else
            txt_color = ST7735Color.WHITE;
            txt_bg_color = ST7735Color.BLACK;
#endif
        }

        public void select()
        {
            rpi_spi.select_display();
        }

        public void init_screen_size(ushort x_offset, ushort y_offset, ushort width, ushort height)
        {
            start_column = x_offset;
            start_row = y_offset;
            x_start = x_offset;
            y_start = y_offset;

            tft_width = width;
            tft_start_width = width;
            tft_height = height;
            tft_start_height = height;

            // let bufsize = width * height * 2;
            // let buffer: vec![u8; &bufsize] = [];
            // self.tft_buffer = buffer;
        }

        public void set_rotation(TFTRotate rotate)
        {
            byte madctrl = 0;

            tft_rotation = rotate;
            switch (rotate)
            {
                case TFTRotate.Degrees0:
                    if (pcb_type == TFTPcbType.Black) {
                        madctrl = (byte) ( ST7735MadControl.MADCTL_MX | ST7735MadControl.MADCTL_MY | ST7735MadControl.MADCTL_RGB);
                    } else {
                        // madctrl = (byte) (ST7735MadControl.MADCTL_MX | ST7735MadControl.MADCTL_MY | ST7735MadControl.MADCTL_BGR);
                        madctrl = (byte) (ST7735MadControl.MADCTL_BGR | ST7735MadControl.MADCTL_MY);
                    }
                    tft_width = tft_start_width;
                    tft_height = tft_start_height;
                    x_start = start_column;
                    y_start = start_row;
                    break;
                case TFTRotate.Degrees90:
                    if (pcb_type == TFTPcbType.Black) {
                        madctrl = (byte) (ST7735MadControl.MADCTL_MY | ST7735MadControl.MADCTL_MV | ST7735MadControl.MADCTL_RGB);
                    } else {
                        // madctrl = (byte) (ST7735MadControl.MADCTL_MY | ST7735MadControl.MADCTL_MV | ST7735MadControl.MADCTL_BGR);
                        madctrl = (byte) (ST7735MadControl.MADCTL_BGR | ST7735MadControl.MADCTL_MV);
                    }
                    tft_width = tft_start_height;
                    tft_height = tft_start_width;
                    x_start = start_row;
                    y_start = start_column;
                    break;
                case TFTRotate.Degrees180:
                    if (pcb_type == TFTPcbType.Black) {
                        madctrl = (byte) (ST7735MadControl.MADCTL_RGB);
                    } else {
                        // madctrl = (byte) (ST7735MadControl.MADCTL_BGR);
                        madctrl = (byte) (ST7735MadControl.MADCTL_BGR | ST7735MadControl.MADCTL_MX);
                    }
                    tft_width = tft_start_width;
                    tft_height = tft_start_height;
                    x_start = start_column;
                    y_start = start_row;
                    break;
                case TFTRotate.Degrees270:
                    if (pcb_type == TFTPcbType.Black) {
                        madctrl = (byte) (ST7735MadControl.MADCTL_MX | ST7735MadControl.MADCTL_MV | ST7735MadControl.MADCTL_RGB);
                    } else {
                        // madctrl = (byte) (ST7735MadControl.MADCTL_MX | ST7735MadControl.MADCTL_MV | ST7735MadControl.MADCTL_BGR);
                        madctrl = (byte) (ST7735MadControl.MADCTL_BGR | ST7735MadControl.MADCTL_MV | ST7735MadControl.MADCTL_MX | ST7735MadControl.MADCTL_MY);
                    }
                    tft_width = tft_start_height;
                    tft_height = tft_start_width;
                    x_start = start_row;
                    y_start = start_column;
                    break;
            }

            rpi_spi.write_command(ST7735Command.MADCTL);
            rpi_spi.write_data(madctrl);
        }

#if ENABLE_COLOR18
        public void fill_screen(ST7735Color18 color)
#else
        public void fill_screen(ST7735Color color)
#endif
        {
            fill_rectangle(0, 0, tft_width, tft_height, color);
        }

#if ENABLE_COLOR18
        public void fill_rectangle(ushort x, ushort y, ushort w, ushort h, ST7735Color18 color)
#else
        public void fill_rectangle(ushort x, ushort y, ushort w, ushort h, ST7735Color color)
#endif
        {
            if (x >= tft_width || y >= tft_height) { return; };
            if ((x + w) >= tft_width) { w = (ushort) (tft_width - x); }
            if ((y + h) >= tft_height) { h = (ushort) (tft_height - y); }

            var data = new List<byte>();
#if ENABLE_COLOR18
            var hi = (byte) (((UInt32) color >> 16) & 0xFC);
            var md = (byte) (((UInt32) color >> 8) & 0xFC);
            var lo = (byte) ((byte) color & 0xFC);

            for (var j = 0; j < w; j++) {
                data.Add(hi);
                data.Add(md);
                data.Add(lo);
            }
#else
            var hi = (byte) (((ushort) color >> 8) & 0xFF);
            var lo = (byte) ((ushort) color & 0xFF);

            for (var j = 0; j < w; j++) {
                data.Add(hi);
                data.Add(lo);
            }
#endif
            var buffer = data.ToArray();

            set_addr_window(x, y, w, h);
            for (var i = 0; i < h; i++) {
                rpi_spi.write_data(buffer);
            }
        }

        public void set_addr_window(ushort x, ushort y, ushort w, ushort h)
        {
            var xs = x + x_start;
            var xe = x + x_start + w;
            rpi_spi.write_command(ST7735Command.CASET);
            rpi_spi.write_data(new byte[] { (byte)(xs >> 8), (byte)(xs & 0xFF), (byte)(xe >> 8), (byte)(xe & 0xFF) });

            var ys = y + y_start;
            var ye = y + y_start + h;
            rpi_spi.write_command(ST7735Command.RASET);
            rpi_spi.write_data(new byte[] { (byte)(ys >> 8), (byte)(ys & 0xFF), (byte)(ye >> 8), (byte)(ye & 0xFF) });

            rpi_spi.write_command(ST7735Command.RAMWR);
        }

#if ENABLE_COLOR18
        public void draw_pixel(ushort x, ushort y, ST7735Color18 color)
#else
        public void draw_pixel(ushort x, ushort y, ST7735Color color)
#endif
        {
            if (x >= tft_width || y >= tft_height) return;
#if ENABLE_COLOR18
            var hi = (byte) (((UInt32) color >> 16) & 0xFC);
            var md = (byte) (((UInt32) color >> 8) & 0xFC);
            var lo = (byte) ((byte) color & 0xFC);

            set_addr_window(x, y, 1, 1);
            rpi_spi.write_data(new byte[] { hi, md, lo });
#else
            var hi = (byte) (((ushort) color >> 8) & 0xFF);
            var lo = (byte) ((ushort) color & 0xFF);

            set_addr_window(x, y, 1, 1);
            rpi_spi.write_data(new byte[] { hi, lo });
#endif

        }

// #if ENABLE_COLOR18
//         public void draw_char(ushort x, ushort y, byte c, ST7735Color18 color, byte size)
// #else
//         public void draw_char(ushort x, ushort y, byte c, ST7735Color color, byte size)
// #endif
//         {
//             draw_char(x, y, c, color, color, size);
//         }

#if ENABLE_COLOR18
        public void draw_char(ushort x, ushort y, byte c, ST7735Color18 color, ST7735Color18 bg, byte size)
#else
        public void draw_char(ushort x, ushort y, byte c, ST7735Color color, ST7735Color bg, byte size)
#endif
        {
            if (x >= tft_width || y >= tft_height || (x + 6 * size - 1) < 0 || (y + 8 * size - 1) < 0) return;
            if (size < 1) size = 1;
            if (c < ' ' || c > '~') c = (byte)'?';

            // fill_rectangle(x, y, (ushort)(5 * size), (ushort)(7 * size), bg);

            for (int i = 0; i < 5; i++) // char - 5 columns
            {
                byte line;
                if (c < 'S') line = font[(c - 32) * 5 + i];
                else line = font2[(c - 'S') * 5 + i];

                for (int j = 0; j < 7; j++, line >>= 1) // char - 7 rows
                {
                    if ((line & 0x01) != 0)
                    {
                        if (size == 1) draw_pixel((ushort)(x + i), (ushort)(y + j), color);
                        else fill_rectangle((ushort)(x + (i * size)), (ushort)(y + (j * size)), size, size, color);
                    }
                    else if (bg != color)
                    {
                        if (size == 1) draw_pixel((ushort)(x + i), (ushort)(y + j), bg);
                        else fill_rectangle((ushort)(x + i * size), (ushort)(y + j * size), size, size, bg);
                    }
                }
            }
            if (bg != color)
            {
                // 6th column (space between chars)
                fill_rectangle((ushort)(x + 5 * size), y, size, (ushort)(7 * size), bg);
            }
        }

        int wrap = 1;
        void set_text_wrap(int w)
        {
            wrap = w;
        }

#if ENABLE_COLOR18
        public void draw_text(ushort x, ushort y, string text, ST7735Color18 color, byte size)
#else
        public void draw_text(ushort x, ushort y, string text, ST7735Color color, byte size)
#endif
        {
            draw_text(x, y, text, color, color, size);
        }

#if ENABLE_COLOR18
        public void draw_text(ushort x, ushort y, string text, ST7735Color18 color, ST7735Color18 bg, byte size)
#else
        public void draw_text(ushort x, ushort y, string text, ST7735Color color, ST7735Color bg, byte size)
#endif
        {
            ushort cursor_x = x, cursor_y = y;
            int textsize = text.Length;
            for (int i = 0; i < textsize; i++)
            {
                if (wrap != 0 && (cursor_x + size * 5) > tft_width)
                {
                    cursor_x = 0;
                    cursor_y = (ushort)(cursor_y + size * 7 + 3);
                    if (cursor_y > tft_height) cursor_y = tft_height;
                    if (text[i] == ' ') continue;
                }
                
                draw_char(cursor_x, cursor_y, (byte)text[i], color, bg, size);

                cursor_x = (ushort)(cursor_x + size * 6);
                if (cursor_x > tft_width) cursor_x = tft_width;
            }
            
        }

        const int TX_RX_BUF_LENGTH = 2 * 1024;
        public void draw_bitmap(ushort x_pos, ushort y_pos, ushort bitmap_width, ushort bitmap_height, byte[] image)
        {
            var i = 0; //bitmap_width * (bitmap_height - 1);

#if true
            set_addr_window(x_pos, y_pos, bitmap_width, bitmap_height);
            var buffer = new byte[TX_RX_BUF_LENGTH];
            var count = 0;
            for (var y = 0; y < bitmap_height; y++)
            {
                for (var x = 0; x < bitmap_width; x++)
                {
                    if (count == TX_RX_BUF_LENGTH)
                    {
                        rpi_spi.write_data_length(buffer, count);
                        count = 0;
                    }
#if ENABLE_COLOR18
                    buffer[count++] = image[i + 2];     // blue
                    buffer[count++] = image[i + 1]; // green
                    buffer[count++] = image[i]; // red
                    i += 3;
#else
                    buffer[count++] = image[i];
                    buffer[count++] = image[i + 1];
                    i += 2;
#endif
                }
            }
            if (count != 0)
            {
                rpi_spi.write_data(buffer, count);
            }
#else
            for (ushort y = 0; y < bitmap_height; y++)
            {
                for (ushort x = 0; x < bitmap_width; x++)
                {
                    if (i >= image.Length) break;

                    ushort color = (ushort) ((image[i + 1] << 8) + image[i]);
                    i += 2;
                    draw_pixel(x, y, (ST7735Color) color);
                }
            }
#endif
        }

        public void set_cursor()
        {
        }

        public void init_pcb_type(TFTPcbType tft_pcb_type)
        {
            pcb_type = tft_pcb_type;
        }

        public void initialize()
        {
            // https://github.com/gavinlyonsrepo/ST7735_TFT_RPI/blob/main/src/ST7735_TFT.cpp
            // https://github.com/maudeve-it/ST7735S-STM32/blob/main/SOURCE/z_displ_ST7735.c

            select();
            init_ili9486();
            // set_addr_window(0, 0, 320, 480);

            pcb_type = TFTPcbType.None;
        }

        private void init_ili9486()
        {
            rpi_spi.write_command(ST7735Command.SWRESET, 150);

            rpi_spi.write_reg(ST7735Command.IFMCTL, 0x00);
            
            rpi_spi.write_command(ST7735Command.SLPOUT, 250);

#if ENABLE_COLOR18
            rpi_spi.write_reg(ST7735Command.COLMOD, 0x66);
#else
            rpi_spi.write_reg(ST7735Command.COLMOD, 0x55);
#endif

            rpi_spi.write_reg(ST7735Command.PWCTR3, 0x44);

            rpi_spi.write_command(ST7735Command.VMCTR1);
            rpi_spi.write_data(new byte[] { 0x00, 0x00, 0x00, 0x00 });

            rpi_spi.write_command(ST7735Command.GMCTRP1);
            rpi_spi.write_data(new byte[] {
                0x0F, 0x1F, 0x1C, 0x0C, 0x0F, 0x08, 0x48, 0x98,
                0x37, 0x0A, 0x13, 0x04, 0x11, 0x0D, 0x00 });

            rpi_spi.write_command(ST7735Command.GMCTRN1);
            rpi_spi.write_data(new byte[] {
                0x0F, 0x32, 0x2E, 0x0B, 0x0D, 0x05, 0x47, 0x75,
                0x37, 0x06, 0x10, 0x03, 0x24, 0x20, 0x00 });

            rpi_spi.write_command(ST7735Command.DGCTL1);
            rpi_spi.write_data(new byte[] {
                0x0F, 0x32, 0x2E, 0x0B, 0x0D, 0x05, 0x47, 0x75,
                0x37, 0x06, 0x10, 0x03, 0x24, 0x20, 0x00 });

            rpi_spi.write_command(ST7735Command.SLPOUT, 120);

            // rpi_spi.write_command_delay(ST7735Command.INVON);

            // rpi_spi.write_command_delay(ST7735Command.MADCTL);
            // rpi_spi.write_data_delay(new byte[] { (byte)(ST7735MadControl.MADCTL_MX | ST7735MadControl.MADCTL_BGR) });

            rpi_spi.write_command(ST7735Command.DISPON, 150);
        }

        private void init_st7735()
        {
            rpi_spi.write_command(ST7735Command.SWRESET, 150);
            rpi_spi.write_command(ST7735Command.SLPOUT, 255);

            rpi_spi.write_command(ST7735Command.FRMCTR1);
            rpi_spi.write_data(new byte[] { 0x01, 0x2C, 0x2D }, 10);

            rpi_spi.write_command(ST7735Command.FRMCTR2);
            rpi_spi.write_data(new byte[] { 0x01, 0x2C, 0x2D });

            rpi_spi.write_command(ST7735Command.FRMCTR3);
            rpi_spi.write_data(new byte[] { 0x01, 0x2C, 0x2D, 0x01, 0x2C, 0x2D });

            rpi_spi.write_command(ST7735Command.MADCTL);
            rpi_spi.write_data((byte) ST7735MadControl.MADCTL_BGR);

            rpi_spi.write_command(ST7735Command.DISSET5);
            rpi_spi.write_data(new byte[] { 0x15, 0x02 });

            rpi_spi.write_reg(ST7735Command.INVCTR, 0x07);

            rpi_spi.write_command(ST7735Command.PWCTR1);
            rpi_spi.write_data(new byte[] { 0xA2, 0x02, 0x84 });

            rpi_spi.write_reg(ST7735Command.PWCTR2, 0xC5);

            rpi_spi.write_command(ST7735Command.PWCTR3);  
            rpi_spi.write_data(new byte[] { 0x0A, 0x00 });

            rpi_spi.write_command(ST7735Command.PWCTR4);
            rpi_spi.write_data(new byte[] { 0x8A, 0x2A });

            rpi_spi.write_command(ST7735Command.PWCTR5);
            rpi_spi.write_data(new byte[] { 0x8A, 0xEE });

            rpi_spi.write_reg(ST7735Command.VMCTR1, 0x0E);

            rpi_spi.write_command(ST7735Command.INVOFF);

            rpi_spi.write_reg(ST7735Command.MADCTL, 0xC8);

            rpi_spi.write_reg(ST7735Command.COLMOD, 0x05);

            //// 480 x 320
            rpi_spi.write_command(ST7735Command.CASET);
            rpi_spi.write_data(new byte[] { 0x00, 0x00, 0x01, 0x3F }); //0-319

            rpi_spi.write_command(ST7735Command.RASET);
            rpi_spi.write_data(new byte[] { 0x00, 0x00, 0x01, 0xDF }); //0-479

            rpi_spi.write_command(ST7735Command.GMCTRP1);
            rpi_spi.write_data(new byte[] {
                0x02, 0x1C, 0x07, 0x12, 0x37, 0x32, 0x29, 0x2D,
                0x29, 0x25, 0x2B, 0x39, 0x00, 0x01, 0x03, 0x10 });

            rpi_spi.write_command(ST7735Command.GMCTRN1);
            rpi_spi.write_data(new byte[] {
                0x3B, 0x1D, 0x07, 0x06, 0x2E, 0x2C, 0x29, 0x2D,
                0x2E, 0x2E, 0x37, 0x3F, 0x00, 0x00, 0x02, 0x10 }, 10);

            rpi_spi.write_command(ST7735Command.NORON, 10);
            rpi_spi.write_command(ST7735Command.DISPON, 100);
        }
    }
}
