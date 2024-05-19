using System.Drawing.Drawing2D;

namespace SunSpecGraphing
{
    internal class GraphingAPI
    {
        // Graph position is bottom left
        public int WIDTH;
        public int HEIGHT;
        public int POS_X;
        public int POS_Y;

        public int[] wavelengthPositions = new int[Global.SAMPLE_SIZE];

        public GraphingAPI(int width, int height, int pos_x, int pos_y)
        {
            WIDTH = width;
            HEIGHT = height;
            POS_X = pos_x;
            POS_Y = pos_y;
        }

        public void DrawData(Graphics g)
        {
            Point[] points = new Point[Global.SAMPLE_SIZE];
            for (int i = 0; i < Global.SAMPLE_SIZE; i++)
            {
                points[i] = new Point(wavelengthPositions[i], CalculateYPosition(SpectrometerHandler.currentData[i]));
            }

            ColorBlend colorBlend = new ColorBlend();
            colorBlend.Colors = Global.WAVELENGTH_ARGB;
            colorBlend.Positions = Global.WAVELENGTH_GRADIENT_POS;

            using (LinearGradientBrush brush = new LinearGradientBrush(points[0], points[points.Length - 1], Color.Red, Color.Blue))
            {
                brush.InterpolationColors = colorBlend;
                g.DrawLines(new Pen(brush), points);
            }
        }

        public int CalculateYPosition(int Intensity)
        {
            // Calculate Y
            int y = (HEIGHT- (int)(((double)Intensity / (double)Global.MAX_Y_VALUE) * (double)HEIGHT)) + POS_Y;
            // Console.WriteLine(y);
            return y;
        }

        public void BakeWavelengthPositions()
        {
            // Calculate all the X values once as they don't change
            for (int i = 0; i < Global.SAMPLE_SIZE; i++)
            {
                wavelengthPositions[i] = (int)((double)(Global.WAVELENGTHS[i] - Global.WAVELENGTHS[0]) / (double)Global.SPECTROMETER_RANGE * (double)WIDTH) + POS_X;
                // Console.WriteLine(wavelengthPositions[i]);
            }
        }
    }
}
