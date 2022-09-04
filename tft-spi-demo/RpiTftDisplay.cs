using System.Device.Gpio;

namespace TftSpiDemo
{
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
                       // INVON = 0x21, // display invert on
                       // DISPOFF = 0x28, // display off
        DISPON = 0x29, // display on
                       // TFT_IDLE_MDODE_ON = 0x39, // idle mode on
                       // TFT_IDLE_MODE_OFF = 0x38, // idle mode off
        CASET = 0x2A, // column address set
        RASET = 0x2B, //row/page address set
        RAMWR = 0x2C, // memory write
                      // RAMRD = 0x2E, // memory read
                      // PTLAR = 0x30, // partial area
                      // VSCRDEF = 0x33, // vertical scroll def
        COLMOD = 0x3A, // interface pixel format
                       // MADCTL = 0x36, // memory access control
                       // VSCRSADD = 0x37, //vertical access control

        // frame rate control
        FRMCTR1 = 0xB1, // normal
        FRMCTR2 = 0xB2, // idle
        FRMCTR3 = 0xB3, // partial

        INVCTR = 0xB4, // display inversion control
                       // DISSET5 = 0xB6, // display function set

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

    // enum ST7335MadControl {
    //     MADCTL_MY = 0x80,
    //     MADCTL_MX = 0x40,
    //     MADCTL_MV = 0x20,
    //     MADCTL_ML = 0x10,
    // }

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

        GpioController tft_rst;

        //// tft_buffer: vec!<u8>(),
        ST7735Color txt_color;
        ST7735Color txt_bg_color;

        public RpiTftDisplay()
        {
            //    pub fn new () -> Self {
            rpi_spi = new RpiSpi();
            //let gpio25 = Gpio::new ().unwrap().get(25).unwrap().into_output();

            mode = TFTMode.DISPLAYOFF;
            pcb_type = TFTPcbType.None;

            x_start = 0;
            y_start = 0;
            // cursor_x: 0,
            // cursor_y: 0,

            tft_height = 320;
            tft_width = 480;
            tft_start_height = tft_height;
            tft_start_width = tft_width;

            tft_rst = new GpioController();
            tft_rst.OpenPin(25, PinMode.Output);

            // tft_buffer: [],
            txt_color = ST7735Color.WHITE;
            txt_bg_color = ST7735Color.BLACK;
        }

        public void init_screen_size(ushort x_offset, ushort y_offset, ushort width, ushort height)
        {
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

        public void fill_screen(ST7735Color color)
        {
            fill_rectangle(0, 0, tft_width, tft_height, color);
        }

        public void fill_rectangle(ushort x, ushort y, ushort w, ushort h, ST7735Color color)
        {
            if (x >= tft_width || y >= tft_height) { return; };
            if ((x + w - 1) >= tft_height) { w = (ushort) (tft_width - x); }
            if ((y + h - 1) >= tft_height) { h = (ushort) (tft_height - y); }
            var hi = (byte) ((ushort) color >> 8);
            var lo = (byte) color;

#if true
            set_addr_window(x, y, (ushort)(x + w - 1), (ushort)(y + h - 1));
            rpi_spi.write_command_delay(ST7735Command.RAMWR, 0);
            // var buffer = new byte[] { hi, lo };
            var data = new List<byte>();
            for (var i = 0; i < h; i++) {
                for (var j = 0; j < w; j++) {
                    // rpi_spi.write_data_delay(hi, 0);
                    // rpi_spi.write_data_delay(lo, 0);

                    // rpi_spi.write_data_delay(buffer, 0);

                    data.Add(hi);
                    data.Add(lo);
                }

                rpi_spi.write_data_delay(data.ToArray(), 0);
                data.Clear();
            }
#else
            var data = new List<byte>();
            for (var i = 0; i < h; i++) {
                for (var j = 0; j < w; j++) {
                    data.Add(hi);
                    data.Add(lo);
                }
            }

            set_addr_window(x, y, (ushort)(x + w - 1), (ushort)(y + h - 1));
            rpi_spi.write_command_delay(ST7735Command.RAMWR, 0);
            rpi_spi.write_data_delay(data.ToArray(), 0);
#endif
        }

        public void set_addr_window(ushort x0, ushort y0, ushort x1, ushort y1)
        {
            var value0 = x0 + x_start;
            var value1 = x1 + x_start;
            rpi_spi.write_command_delay(ST7735Command.CASET, 0);
            
            byte[] data = new byte[] { (byte)(value0 >> 8), (byte) value0, (byte) (value1 >> 8), (byte) value1 };
            rpi_spi.write_data_delay(data, 0);

            value0 = y0 + y_start;
            value1 = y1 + y_start;
            rpi_spi.write_command_delay(ST7735Command.RASET, 0);
            rpi_spi.write_data_delay(new byte[] { (byte)(value0 >> 8), (byte)value0, (byte)(value1 >> 8), (byte)value1 }, 0);
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

            reset_pin();

            cmd2_none();
            pcb_type = TFTPcbType.None;
        }

        private void cmd1()
        {
        }

        private void cmd2_none()
        {
            rpi_spi.write_command_delay(ST7735Command.SWRESET, 150);
            rpi_spi.write_command_delay(ST7735Command.SLPOUT, 500);

            rpi_spi.write_command_delay(ST7735Command.FRMCTR1, 0);
            rpi_spi.write_data_delay(new byte[] { 0x01, 0x2C, 0x2D }, 10);

            rpi_spi.write_command_delay(ST7735Command.FRMCTR2, 0);
            rpi_spi.write_data_delay(new byte[] { 0x01, 0x2C, 0x2D }, 0);

            rpi_spi.write_command_delay(ST7735Command.FRMCTR3, 0);
            rpi_spi.write_data_delay(new byte[] { 0x01, 0x2C, 0x2D, 0x01, 0x2C, 0x2D }, 0);

            rpi_spi.write_command_delay(ST7735Command.INVCTR, 0);
            rpi_spi.write_data_delay(new byte[] { 0x07 }, 0);

            rpi_spi.write_command_delay(ST7735Command.PWCTR1, 0);
            rpi_spi.write_data_delay(new byte[] { 0xA2, 0x02, 0x84 }, 10);

            rpi_spi.write_command_delay(ST7735Command.PWCTR2, 0);
            rpi_spi.write_data_delay(new byte[] { 0xC5 }, 0);

            rpi_spi.write_command_delay(ST7735Command.PWCTR3, 0);
            rpi_spi.write_data_delay(new byte[] { 0x0A, 0x00 }, 0);

            rpi_spi.write_command_delay(ST7735Command.PWCTR4, 0);
            rpi_spi.write_data_delay(new byte[] { 0x8A, 0x2A }, 0);

            rpi_spi.write_command_delay(ST7735Command.PWCTR5, 0);
            rpi_spi.write_data_delay(new byte[] { 0x8A, 0xEE }, 0);

            rpi_spi.write_command_delay(ST7735Command.VMCTR1, 0);
            rpi_spi.write_data_delay(new byte[] { 0x0E }, 10);

            rpi_spi.write_command_delay(ST7735Command.INVOFF, 0);

            rpi_spi.write_command_delay(ST7735Command.COLMOD, 0);
            rpi_spi.write_data_delay(new byte[] { 0x05 }, 10);

            //// 480 x 320
            rpi_spi.write_command_delay(ST7735Command.CASET, 0); //0-479
            rpi_spi.write_data_delay(new byte[] { 0x00, 0x00, 0x01, 0xDF }, 0); //0-479

            rpi_spi.write_command_delay(ST7735Command.RASET, 0); //0-319
            rpi_spi.write_data_delay(new byte[] { 0x00, 0x00, 0x01, 0x3F }, 0); //0-319

            rpi_spi.write_command_delay(ST7735Command.GMCTRP1, 0);
            rpi_spi.write_data_delay(new byte[] {
                0x02, 0x1C, 0x07, 0x12, 0x37, 0x32, 0x29, 0x2D,
                0x29, 0x25, 0x2B, 0x39, 0x00, 0x01, 0x03, 0x10 }, 0);

            rpi_spi.write_command_delay(ST7735Command.GMCTRN1, 0);
            rpi_spi.write_data_delay(new byte[] {
                0x3B, 0x1D, 0x07, 0x06, 0x2E, 0x2C, 0x29, 0x2D,
                0x2E, 0x2E, 0x37, 0x3F, 0x00, 0x00, 0x02, 0x10 }, 10);

            rpi_spi.write_command_delay(ST7735Command.NORON, 10);
            rpi_spi.write_command_delay(ST7735Command.DISPON, 100);
        }

        private void cmd3()
        {
        }

        private void reset_pin()
        {
            tft_rst.Write(25, PinValue.High);
            Thread.Sleep(10);
            tft_rst.Write(25, PinValue.Low);
            Thread.Sleep(10);
            tft_rst.Write(25, PinValue.High);
            Thread.Sleep(10);
        }
    }
}
