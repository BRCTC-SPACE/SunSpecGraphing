using System.Drawing.Drawing2D;

namespace SunSpecGraphing
{
    internal class GraphingAPI
    {
        // Graph position is bottom left 
        // TODO: Figure out if this is correct

        // Variables that are initalized on object creation
        private int WIDTH;
        private int HEIGHT;
        private int POS_X;
        private int POS_Y;
        private Form parent;

        // Vars that can be changed if needed
        // (Technically I want to make them all changable but these
        //  ones are more like "settings")
        private int GRAPH_PADDING_TOP    = 3;
        private int GRAPH_PADDING_BOTTOM = 3;
        private int SPECTRA_BAR_HEIGHT   = 50;


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
                Rectangle rect = new Rectangle(
                    POS_X,                                 // X
                    (HEIGHT - SPECTRA_BAR_HEIGHT) + POS_Y, // Y
                    WIDTH,                                 // Width
                    SPECTRA_BAR_HEIGHT                     // Height
                );


                // Creates a solid black bacground which help defines the spectra
                // TODO: Maybe add an option for the background colour??
                using (SolidBrush backgroundBrush = new SolidBrush(Color.Black))
                {
                    g.FillRectangle(backgroundBrush, rect);
                }

                // Create the blend of spectra based on the active alpha/intensity of data
                ColorBlend colorBlend = new ColorBlend();
                colorBlend.Colors = Global.WAVELENGTH_ARGB_ACTIVE;
                colorBlend.Positions = Global.WAVELENGTH_GRADIENT_POS;

                using (LinearGradientBrush brush = new LinearGradientBrush(rect, Color.Red, Color.Blue, LinearGradientMode.Horizontal))
                {
                    brush.InterpolationColors = colorBlend;
                    g.FillRectangle(brush, rect);
                }
            }
        }

        public int CalculateYPosition(int Intensity)
        {
            // Calculate Y
            int y = (HEIGHT - (int)(((double)Intensity / (double)Global.MAX_GRAPH_Y_VALUE) * (double)HEIGHT)) + POS_Y;
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
