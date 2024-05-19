using System.Drawing.Drawing2D;

namespace SunSpecGraphing
{
    internal class GraphingAPI
    {
        // Graph position is bottom left
        private int WIDTH;
        private int HEIGHT;
        private int POS_X;
        private int POS_Y;
        private Form parent;

        public int[] wavelengthPositions = new int[Global.SAMPLE_SIZE];

        public GraphingAPI(int width, int height, int pos_x, int pos_y, Form parent)
        {
            WIDTH = width;
            HEIGHT = height;
            POS_X = pos_x;
            POS_Y = pos_y;
            this.parent = parent;
        }

        public void DrawDataGraph()
        {
            Point[] points = new Point[Global.SAMPLE_SIZE];
            for (int i = 0; i < Global.SAMPLE_SIZE; i++)
            {
                points[i] = new Point(wavelengthPositions[i], CalculateYPosition(SpectrometerHandler.currentData[i]));
            }

            using (Graphics g = parent.CreateGraphics())
            {
                if (Global.UseGraphingColor)
                {
                    ColorBlend colorBlend = new ColorBlend();
                    colorBlend.Colors = Global.WAVELENGTH_ARGB;
                    colorBlend.Positions = Global.WAVELENGTH_GRADIENT_POS;

                    using (LinearGradientBrush brush = new LinearGradientBrush(points[0], points[points.Length - 1], Color.Red, Color.Blue))
                    {
                        brush.InterpolationColors = colorBlend;
                        g.DrawLines(new Pen(brush), points);
                    }
                }
                else
                {
                    using (Pen pen = new Pen(Color.Black))
                    {
                        g.DrawLines(pen, points);
                    }
                }
            }
        }

        public void DrawSpectraBar()
        {
            Conversions.UpdateAllColors();

            using (Graphics g = parent.CreateGraphics())
            {
                Rectangle rect = new Rectangle(50, 400, 600, 20);

                using (SolidBrush backgroundBrush = new SolidBrush(Color.Black))
                {
                    g.FillRectangle(backgroundBrush, rect);
                }

                // Create a ColorBlend object
                ColorBlend colorBlend = new ColorBlend();
                colorBlend.Colors = Global.WAVELENGTH_ARGB_ACTIVE;
                colorBlend.Positions = Global.WAVELENGTH_GRADIENT_POS;

                // Create a LinearGradientBrush with the color blend
                using (LinearGradientBrush brush = new LinearGradientBrush(rect, Color.Red, Color.Blue, LinearGradientMode.Horizontal))
                {
                    brush.InterpolationColors = colorBlend;

                    // Fill a rectangle with the gradient brush
                    g.FillRectangle(brush, rect);
                }
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
