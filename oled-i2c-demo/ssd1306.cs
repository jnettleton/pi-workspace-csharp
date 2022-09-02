using System.Device.I2c;

namespace OledI2cDemo
{
    public class ssd1306
    {

		private byte[] BitReverseTable256 = new byte[]
		{
			0x00, 0x80, 0x40, 0xC0, 0x20, 0xA0, 0x60, 0xE0, 0x10, 0x90, 0x50, 0xD0, 0x30, 0xB0, 0x70, 0xF0,
			0x08, 0x88, 0x48, 0xC8, 0x28, 0xA8, 0x68, 0xE8, 0x18, 0x98, 0x58, 0xD8, 0x38, 0xB8, 0x78, 0xF8,
			0x04, 0x84, 0x44, 0xC4, 0x24, 0xA4, 0x64, 0xE4, 0x14, 0x94, 0x54, 0xD4, 0x34, 0xB4, 0x74, 0xF4,
			0x0C, 0x8C, 0x4C, 0xCC, 0x2C, 0xAC, 0x6C, 0xEC, 0x1C, 0x9C, 0x5C, 0xDC, 0x3C, 0xBC, 0x7C, 0xFC,
			0x02, 0x82, 0x42, 0xC2, 0x22, 0xA2, 0x62, 0xE2, 0x12, 0x92, 0x52, 0xD2, 0x32, 0xB2, 0x72, 0xF2,
			0x0A, 0x8A, 0x4A, 0xCA, 0x2A, 0xAA, 0x6A, 0xEA, 0x1A, 0x9A, 0x5A, 0xDA, 0x3A, 0xBA, 0x7A, 0xFA,
			0x06, 0x86, 0x46, 0xC6, 0x26, 0xA6, 0x66, 0xE6, 0x16, 0x96, 0x56, 0xD6, 0x36, 0xB6, 0x76, 0xF6,
			0x0E, 0x8E, 0x4E, 0xCE, 0x2E, 0xAE, 0x6E, 0xEE, 0x1E, 0x9E, 0x5E, 0xDE, 0x3E, 0xBE, 0x7E, 0xFE,
			0x01, 0x81, 0x41, 0xC1, 0x21, 0xA1, 0x61, 0xE1, 0x11, 0x91, 0x51, 0xD1, 0x31, 0xB1, 0x71, 0xF1,
			0x09, 0x89, 0x49, 0xC9, 0x29, 0xA9, 0x69, 0xE9, 0x19, 0x99, 0x59, 0xD9, 0x39, 0xB9, 0x79, 0xF9,
			0x05, 0x85, 0x45, 0xC5, 0x25, 0xA5, 0x65, 0xE5, 0x15, 0x95, 0x55, 0xD5, 0x35, 0xB5, 0x75, 0xF5,
			0x0D, 0x8D, 0x4D, 0xCD, 0x2D, 0xAD, 0x6D, 0xED, 0x1D, 0x9D, 0x5D, 0xDD, 0x3D, 0xBD, 0x7D, 0xFD,
			0x03, 0x83, 0x43, 0xC3, 0x23, 0xA3, 0x63, 0xE3, 0x13, 0x93, 0x53, 0xD3, 0x33, 0xB3, 0x73, 0xF3,
			0x0B, 0x8B, 0x4B, 0xCB, 0x2B, 0xAB, 0x6B, 0xEB, 0x1B, 0x9B, 0x5B, 0xDB, 0x3B, 0xBB, 0x7B, 0xFB,
			0x07, 0x87, 0x47, 0xC7, 0x27, 0xA7, 0x67, 0xE7, 0x17, 0x97, 0x57, 0xD7, 0x37, 0xB7, 0x77, 0xF7,
			0x0F, 0x8F, 0x4F, 0xCF, 0x2F, 0xAF, 0x6F, 0xEF, 0x1F, 0x9F, 0x5F, 0xDF, 0x3F, 0xBF, 0x7F, 0xFF
		};
        private I2cDevice _i2cDevice;
        const int SSD1306_BUS = 1;
        const int SSD1306_ADDR = 0x3c;

		const byte OLED_HEIGHT = 32;
		const byte OLED_WIDTH = 128;
		const int BUFFER_SIZE = OLED_HEIGHT*OLED_WIDTH/8;
		byte[] oled_buffer = new byte[BUFFER_SIZE];

        //_swap(a, b) (((a) ^= (b)), ((b) ^= (a)), ((a) ^= (b))) 

        enum pixelcolor
		{
			black_pixel,
			white_pixel,
			inverse_pixel,
		};

		public ssd1306()
        {
            var i2cSettings = new I2cConnectionSettings(SSD1306_BUS, SSD1306_ADDR);
            _i2cDevice = I2cDevice.Create(i2cSettings);
        }

		public void init()
        {
            try
            {
	            // OLED turn off and check if OLED is connected
                sendCommand(SSD1306_ADDR, 0xae);

				// Set display oscillator frequency and divide ratio
				sendCommand(SSD1306_ADDR, 0xd5);
				sendCommand(SSD1306_ADDR, 0x80); // 0x50);

				// Set multiplex ratio
				sendCommand(SSD1306_ADDR, 0xa8);
				sendCommand(SSD1306_ADDR, OLED_HEIGHT - 1); //0x3f);
				// Set display start line
				sendCommand(SSD1306_ADDR, 0xd3);
				sendCommand(SSD1306_ADDR, 0x00);
				// Set the lower column address
				sendCommand(SSD1306_ADDR, 0x00);
				// Set the higher column address
				sendCommand(SSD1306_ADDR, 0x10);

				// Set page address
				sendCommand(SSD1306_ADDR, 0xb0);

				// Charge pump
				sendCommand(SSD1306_ADDR, 0x8d);
				sendCommand(SSD1306_ADDR, 0x14);

				// Memory mode
				sendCommand(SSD1306_ADDR, 0x20);
				sendCommand(SSD1306_ADDR, 0x00);

				// Set segment from left to right
				sendCommand(SSD1306_ADDR, 0xa0 | 0x01);
				// Set OLED upside up
				sendCommand(SSD1306_ADDR, 0xc8);
				// Set common signal pad configuration
				sendCommand(SSD1306_ADDR, 0xda);
				sendCommand(SSD1306_ADDR, 0x02); // 0x12);

				// Set Contrast
				sendCommand(SSD1306_ADDR, 0x81);
				// Contrast data
				sendCommand(SSD1306_ADDR, 0x8F); // 0x00);

				// Set discharge recharge periods
				sendCommand(SSD1306_ADDR, 0xd9);
				sendCommand(SSD1306_ADDR, 0xf1);

				// Set common mode pad output voltage 
				sendCommand(SSD1306_ADDR, 0xdb);
				sendCommand(SSD1306_ADDR, 0x40);

				// Set Entire display
				sendCommand(SSD1306_ADDR, 0xa4);

				// Set Normal display
				sendCommand(SSD1306_ADDR, 0xa6);
				// Stop scroll
				sendCommand(SSD1306_ADDR, 0x2e);

				// OLED turn on
				sendCommand(SSD1306_ADDR, 0xaf);

				// Set column address
				sendCommand(SSD1306_ADDR, 0x21);
				// Start Column
				sendCommand(SSD1306_ADDR, 0x00);
				// Last column
				sendCommand(SSD1306_ADDR, 127);

				// Set page address
				sendCommand(SSD1306_ADDR, 0x22);
				// Start Page
				sendCommand(SSD1306_ADDR, 0x00);
				// Last Page
				sendCommand(SSD1306_ADDR, 0x07);
            }
            catch (Exception)
            {
                // IOException -- unable to open port
            }
        }

        public void sendCommand(byte addr, byte cmd)
        {
            var dataToSend = new byte[2];
			dataToSend[0] = 0x00;
            dataToSend[1] = cmd;

			_i2cDevice.Write(dataToSend);
        }

		private void writeData(byte addr, byte[] data)
        {
            var dataToSend = new byte[data.Length + 1];
            dataToSend[0] = 0x40;
            for (int i = 0; i < data.Length; i++)
            {
                dataToSend[i + 1] = data[i];
            }

            _i2cDevice.Write(dataToSend);
		}

		public void drawPixel(int x, int y, byte color)
		{
			// Verify that pixel is inside of OLED matrix
			if (x >= 0 && x < OLED_WIDTH && y >= 0 && y < OLED_HEIGHT)
			{
				switch (color)
				{
					case 0:
						{
							oled_buffer[x + (y / 8) * 128] &= (byte)(~(1 << (y & 7)));
						}
						break;
					case 1:
						{
							oled_buffer[x + (y / 8) * 128] |= (byte)(1 << (y & 7));
						}
						break;
					case 2:
						{
							oled_buffer[x + (y / 8) * 128] ^= (byte)(1 << (y & 7));
						}
						break;
					default:
						break;
				}
			}
		}

		public void drawRect(int x, int y, int width, int height, byte color)
		{
			for (int i = x; i < x + width; i++)
			{
				// Draw the top line of rectangle
				drawPixel(i, y, color);
				// Draw the inferior line of rectangle
				drawPixel(i, (y + height), color);
			}
			for (int i = y; i < y + height; i++)
			{
				// Draw the right line of rectangle
				drawPixel(x, i, color);
				// Draw lthe ledf line of rectangle
				drawPixel((x + width), i, color);
			}
		}

		/**
		* @brief  Draw a fill rectangle given start point, width and height
		* @param  x: x coordinate
		* @param  y: y coordinate
		* @param  width: rectangle width
		* @param  height: rectangle height
		* @retval None.
		*/
		public void drawFillRect(int x, int y, int width, int height, byte color)
		{
			for (int i = x; i < x + width; i++)
			{
				for (int j = y; j < y + height; j++)
				{
					drawPixel(i, j, color);
				}
			}
		}

		private bool bitTest(byte data, byte n)
		{
			if (n < 0 || n > 7)
			{
				return false;
			}

			if (((data >> (7 - n)) & 1) == 1)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		/**
        * @brief  Draw a string
        * @param  x: x coordinate of start point
        * @param  y: y coordinate of start point
        * @param  textptr: pointer 
        * @param  size: scale
        * @retval None.
        */
		public void drawString(int x, int y, string text, int size, byte color)
		{
			// Loop counters
			byte i;
			byte j;
			byte k;
			byte l;
			byte m;
			// Stores character data
			byte[] pixelData = new byte[5];

			// Loop through the passed string
			for (i = 0; i < text.Length; ++i, ++x)
			{
				// Get data font
				for (byte p = 0; p < 5; p++)
					pixelData[p] = ssd1306font.data[text[i] - ' ', p];

				// Performs character wrapping
				if (x + 5 * size >= 128)
				{
					// Set x at far left position
					x = 0;
					// Set y at next position down
					y += 7 * size + 1;
				}

				// Loop through character byte data
				for (j = 0; j < 5; ++j, x += size)
				{
					// Loop through the vertical pixels
					for (k = 0; k < 7 * size; ++k)
					{
						// Check if the pixel should be set
						if (bitTest(BitReverseTable256[pixelData[j]], k))
						{
							// The next two loops change the character's size
							for (l = 0; l < size; ++l)
							{
								for (m = 0; m < size; ++m)
								{
									// Draws the pixel
									drawPixel(x + m, y + k * size + l, color);
								}
							}
						}
					}
				}
			}
		}

		/**
		  * @brief  Send OLED buffer to OLED RAM 
		  * @retval None.
		  */
		public void refresh()
		{
			// Set the lower column address to zero
			sendCommand(SSD1306_ADDR, 0x00);
			// Set the higher column address to zero
			sendCommand(SSD1306_ADDR, 0x10);
			// Set page address to zero
			sendCommand(SSD1306_ADDR, 0xb0);
			// Send OLED buffer to sd1306 RAM
			writeData(SSD1306_ADDR, oled_buffer);
		}

		/**
		  * @brief  Set all buffer's bytes to zero
		  * @retval None.
		  */
		public void clearBuffer()
		{
			for (int i = 0; i < oled_buffer.Length; i++)
			{
				oled_buffer[i] = 0;
			}
		}
	}
}
