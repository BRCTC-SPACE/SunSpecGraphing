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
        private SpectrometerDataHandler data;

        // Vars that can be changed if needed
        // (Technically I want to make them all changable but these
        //  ones are more like "settings")
        public bool useGraphingColor = false;

        private int GRAPH_PADDING_TOP    = 3;
        private int GRAPH_PADDING_BOTTOM = 3;
        private int SPECTRA_BAR_HEIGHT   = 50;

        // This will be changable based on the users option
        private int MAX_INTENSITY_VALUE = 65535;


        public int[] wavelengthPositions = new int[Global.SAMPLE_SIZE];

        public Color[] WavelengthColors = new Color[Global.SAMPLE_SIZE];
        public Color[] ActiveWavelengthColorIntensities = new Color[Global.SAMPLE_SIZE];
        public float[] WavelenghtGradientPositions = new float[Global.SAMPLE_SIZE];


        public GraphingAPI(int width, int height, int pos_x, int pos_y, Form parent, SpectrometerDataHandler data)
        {
            WIDTH = width;
            HEIGHT = height;
            POS_X = pos_x;
            POS_Y = pos_y;
            this.parent = parent;
            this.data = data;

            MAX_INTENSITY_VALUE = data.GetMaxIntensity();
            Init();
        }

        private void DrawDataGraph()
        {
            Point[] points = new Point[Global.SAMPLE_SIZE];
            for (int i = 0; i < Global.SAMPLE_SIZE; i++)
            {
                points[i] = new Point(wavelengthPositions[i], CalculateYPosition(data.Intensities[i]));
            }

            using (Graphics g = parent.CreateGraphics())
            {
                if (useGraphingColor)
                {
                    ColorBlend colorBlend = new ColorBlend();
                    colorBlend.Colors = WavelengthColors;
                    colorBlend.Positions = WavelenghtGradientPositions;

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

        private void DrawSpectraBar()
        {
            UpdateAllColors();

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
                colorBlend.Colors = ActiveWavelengthColorIntensities;
                colorBlend.Positions = WavelenghtGradientPositions;

                using (LinearGradientBrush brush = new LinearGradientBrush(rect, Color.Red, Color.Blue, LinearGradientMode.Horizontal))
                {
                    brush.InterpolationColors = colorBlend;
                    g.FillRectangle(brush, rect);
                }
            }
        }

        private int CalculateYPosition(int Intensity)
        {
            // Calculate Y

            // TODO: Add logic for if local or global max

            int y = (HEIGHT - (int)(((double)Intensity / (double)MAX_INTENSITY_VALUE) * (double)HEIGHT)) + POS_Y;
            // Console.WriteLine(y);
            return y;
        }

        private void BakeWavelengthPositions()
        {
            // Calculate all the X values once as they don't change
            for (int i = 0; i < Global.SAMPLE_SIZE; i++)
            {
                wavelengthPositions[i] = (int)((double)(Global.WAVELENGTHS[i] - Global.WAVELENGTHS[0]) / (double)Global.SPECTROMETER_RANGE * (double)WIDTH) + POS_X;
                // Console.WriteLine(wavelengthPositions[i]);
            }
        }

        private void UpdateAllColors()
        {
            for (int i = 0; i < Global.SAMPLE_SIZE; i++)
            {
                Color updatedColor = Conversions.IntensityToColor(data.Intensities[i], WavelengthColors[i], MAX_INTENSITY_VALUE);
                ActiveWavelengthColorIntensities[i] = updatedColor;
            }
        }

        private void CalculateAllPositionalWavelengthValues()
        {
            for (int i = 0; i < Global.SAMPLE_SIZE; i++)
            {
                // NOTE: Multiplied by one here is redundant but the expresion is
                // ( x / a ) * b WHERE x = input value, a = current range, b = wanted range
                WavelenghtGradientPositions[i] = (((float)Global.WAVELENGTHS[i] - (float)Global.WAVELENGTHS[0]) / (float)Global.SPECTROMETER_RANGE);
            }
        }

        private void ConvertAllWavelenghtsToARGB()
        {
            for (int i = 0; i < Global.SAMPLE_SIZE; i++)
            // 2048 samples for each wavelength
            {
                WavelengthColors[i] = Conversions.wavelengthToARGB(Global.WAVELENGTHS[i]);
                ActiveWavelengthColorIntensities = (Color[])WavelengthColors.Clone();
            }
        }

        private void Init()
        {
            BakeWavelengthPositions();
            CalculateAllPositionalWavelengthValues();
            ConvertAllWavelenghtsToARGB();
        }

        public void UpdateGraph()
        {
            DrawDataGraph();
            DrawSpectraBar();
        }
    }
}
