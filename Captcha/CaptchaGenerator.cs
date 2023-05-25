using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Captcha
{
    internal class CaptchaGenerator
    {
        private static readonly Random random = new Random();
        private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private const int length = 8;
        public string CaptchaText;
        public Avalonia.Media.Imaging.Bitmap CaptchaImage;

        public CaptchaGenerator()
        {
            GenerateCaptcha();
            
        }

        public void GenerateCaptcha()
        {
            GenerateCaptchaText();
            GenerateImage();
        }
        private void GenerateCaptchaText()
        {
            CaptchaText = new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        private void GenerateImage()
        {
            Bitmap captcha = new Bitmap(400, 150);
            DrawTextOnImage(captcha, Brushes.Black, CaptchaText);
            DrawRandomLines(captcha, Brushes.Black, 50);
            captcha = YSinusDistortion(captcha, 10, 10);
            captcha = XSinusDistortion(captcha, 10, 4);
            DrawRandomLines(captcha, Brushes.Black, 50);

            using (MemoryStream memory = new MemoryStream())
            {
                captcha.Save(memory, ImageFormat.Png);
                memory.Position = 0;
                Avalonia.Media.Imaging.Bitmap avaBitmap = new Avalonia.Media.Imaging.Bitmap(memory);
                CaptchaImage = avaBitmap;
            }
        }
        private void DrawTextOnImage(Bitmap image, Brush color, string text)
        {
            PointF location = new PointF(image.Width / (length * 10), image.Height / 5);
            using (Graphics graphics = Graphics.FromImage(image))
            {
                using (Font arialFont = new Font("Arial", (float)image.Width / ((float)length * 1.1f)))
                {
                    graphics.DrawString(text, arialFont, color, location);
                }
            }
        }

        private void DrawRandomLines(Bitmap image, Brush color, int quantity)
        {
            Pen pen = new Pen(color);
            using (Graphics graphics = Graphics.FromImage(image))
            {
                for (int i = 0; i < quantity; i++)
                {
                    graphics.DrawLine(pen,
                    new Point(random.Next(0, image.Width), random.Next(0, image.Height)),
                    new Point(random.Next(0, image.Width), random.Next(0, image.Height)));
                }
            }
        }
        private Bitmap YSinusDistortion(Bitmap image, double lenght, double height)
        {
            Bitmap distorted = new Bitmap(image.Width, image.Height);
            for (int x = 0; x < image.Width; x++)
            {
                double sinx = Math.Sin((double)x / lenght) * height;
                for (int y = 0; y < image.Height; y++)
                {
                    Color pixelColor = image.GetPixel(x, y);
                    int newY = y + (int)sinx;
                    if (newY >= image.Height | newY < 0)
                        continue;
                    distorted.SetPixel(x, newY, pixelColor);
                }
            }
            return distorted;
        }
        private Bitmap XSinusDistortion(Bitmap image, double lenght, double height)
        {
            Bitmap distorted = new Bitmap(image.Width, image.Height);
            for (int y = 0; y < image.Height; y++)
            {
                double sinY = Math.Sin((double)y / lenght) * height;
                for (int x = 0; x < image.Width; x++)
                {
                    Color pixelColor = image.GetPixel(x, y);
                    int newX = x + (int)sinY;
                    if (newX >= image.Width | newX < 0)
                        continue;
                    distorted.SetPixel(newX, y, pixelColor);
                }
            }
            return distorted;
        }

    }
}
