using IronOcr;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace SecretProject
{
    public class ImageToText
    {
        private AutoOcr _ocr;
        private Bitmap _bitmap;
        private double _accuracy = 0.005;
        private double _min;
        private double _max;
        public ImageToText(string path)
        {
            if (File.Exists(path) == false)
                throw new FileNotFoundException();
            _min = 1 - _accuracy;
            _max = 1 + _accuracy;
            _bitmap = new Bitmap(path);
            _ocr = new AutoOcr();
        }

        /*public void ClearImageFromAllColorBut(Color color)
        {
            if (color == Color.Black)
                throw new InvalidOperationException("Black is the background color!");

            _min = (1 - _accuracy);
            _max = (1 + _accuracy);

            for (var x = 1; x < _bitmap.Width - 1; x++)
            {
                for (var y = 1; y < _bitmap.Height - 1; y++)
                {                    
                    color = ClearPixelIfNeeded(color, x, y);
                }
            }
        }*/

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Color GetPixelMaskedColor(Color color, int x, int y)
        {
            Color pixel = _bitmap.GetPixel(x, y);

            if (pixel.R < color.R * _min || pixel.R > color.R * _max ||
                pixel.G < color.G * _min || pixel.G > color.G * _max ||
                pixel.B < color.B * _min || pixel.B > color.B * _max)
            {
                return Color.Black;
            }

            return color;
        }

        public string ReadText(int x, int y, int width, int height)
        {
            if (x + width > _bitmap.Size.Width || y + height > _bitmap.Size.Height)
                throw new InvalidOperationException("Height and width are too big");
            var newImage = new Bitmap(width, height);
            var maxLength = x + width - 1;
            var maxHight = y + height - 1;
            for (var cropX = x ; cropX < maxLength; cropX++)
            {
                for (var cropY = y; cropY < maxHight; cropY++)
                {
                    var newPixelColor = GetPixelMaskedColor(Color.White, cropX, cropY);
                    newImage.SetPixel(cropX-x,cropY-y, newPixelColor);
                }
            }
            var clearPath = @"c:\work\test_clear.png";
            newImage.Save(clearPath);
            return _ocr.Read(clearPath).Text;
        }
    }
}
