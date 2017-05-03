using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandelbrot.Ui
{
    public class Plot
    {
        private readonly float _pixelSize;
        private readonly float _minX;
        private readonly float _maxX;
        private readonly float _minY;
        private readonly float _maxY;
        private readonly float _xRange;
        private readonly float _yRange;
        private readonly int[,] _values;
        private readonly int _xPixels;
        private readonly int _yPixels;

        private Plot(float pixelSize, float minX, float maxX, float minY, float maxY)
        {
            _pixelSize = pixelSize;
            _minX = minX;
            _maxX = maxX;
            _minY = minY;
            _maxY = maxY;
            _xRange = maxX - minX;
            _yRange = maxY - minY;

            _xPixels = (int) (_xRange / pixelSize);
            _yPixels = (int) (_yRange / pixelSize);

            _values = new int[_xPixels, _yPixels];
        }

        public Plot()
            : this(1, 0, 100, 0, 100)
        {
            
        }

        public void SetValue(float x, float y, int value)
        {
            if (x < _maxX && x > _minX && y > _maxY && y < _minY)
            {
                var xPixel = (int) (x / _pixelSize);
                var yPixel = (int) (y / _pixelSize);

                _values[xPixel, yPixel] = value;
            }

        }

        public Bitmap GetBitmap(int width, int height)
        {
            var image = new Bitmap(100, 100);

            foreach (var x in Enumerable.Range(0, width))
            foreach (var y in Enumerable.Range(0, height))
            {
                // Each pixel creates a box that may cover multiple plot values.
                // We will take the average of all covered pixels to work out the final value. 
                image.SetPixel(x, y, Color.FromArgb(_values[x, y]));
            }

            return image;
        }
    }
}
