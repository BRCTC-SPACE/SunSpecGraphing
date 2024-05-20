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

        private int GRAPH_PADDING_TOP    = 0;
        private int GRAPH_PADDING_BOTTOM = 0;
        private int GRAPH_PADDING_LEFT   = 0;
        private int GRAPH_PADDING_RIGHT  = 0;
        private int SPECTRA_BAR_HEIGHT   = 50;

        // This will be changable based on the users option
        private int MAX_INTENSITY_VALUE = 65535;
        private string GraphFontType = "Arial";
        private int GraphFontSize = 10;


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

        private void DrawAxes(Graphics g)
        {
            using (Pen axisPen = new Pen(Color.Black, 2))
            {
                // X-Axis
                g.DrawLine(axisPen,
                    POS_X + GRAPH_PADDING_LEFT,
                    HEIGHT - GRAPH_PADDING_BOTTOM + POS_Y,
                    WIDTH - GRAPH_PADDING_RIGHT + POS_X,
                    HEIGHT - GRAPH_PADDING_BOTTOM + POS_Y
                );

                // Y-Axis
                g.DrawLine(axisPen,
                    POS_X + GRAPH_PADDING_LEFT,
                    POS_Y + GRAPH_PADDING_TOP,
                    POS_X + GRAPH_PADDING_LEFT,
                    HEIGHT - GRAPH_PADDING_BOTTOM + POS_Y
                );
            }
        }

        private void DrawLabels(Graphics g)
        {
            int labelStep = 100;
            using (Font font = new Font("Arial", 10))
            using (Brush brush = new SolidBrush(Color.Black))
            {
                // Draw X axis labels
                for (int i = 0; i < Global.SAMPLE_SIZE; i += labelStep)
                {
                    string label = Math.Round(Global.WAVELENGTHS[i]).ToString();
                    SizeF labelSize = g.MeasureString(label, font);
                    float x = wavelengthPositions[i] - labelSize.Width / 2;
                    float y = HEIGHT - GRAPH_PADDING_BOTTOM + POS_Y + 5;
                    g.DrawString(label, font, brush, x, y);
                }

                // Draw Y-Axis labels (e.g., intensity values)
                int numberOfLabels = 10;
                for (int i = 0; i <= numberOfLabels; i++)
                {
                    int intensity = MAX_INTENSITY_VALUE / numberOfLabels * i;
                    string label = intensity.ToString();
                    SizeF labelSize = g.MeasureString(label, font);
                    float x = POS_X + GRAPH_PADDING_LEFT - labelSize.Width - 5; // Adjusted for padding
                    float y = CalculateYPosition(intensity) - labelSize.Height / 2;
                    g.DrawString(label, font, brush, x, y);
                }
            }
        }

        private void DrawLegend(Graphics g)
        {
            using (Font font = new Font(GraphFontType, GraphFontSize))
            using (Brush brush = new SolidBrush(Color.Black))
            {
                string legend = "Wavelength Intensities";
                SizeF legendSize = g.MeasureString(legend, font);
                float x = WIDTH + POS_X - GRAPH_PADDING_RIGHT - legendSize.Width - 10;
                float y = POS_Y + GRAPH_PADDING_TOP;
                g.DrawString(legend, font, brush, x, y);
            }
        }

        private int CalculateYPosition(int Intensity)
        {
            int adjustedHeight = HEIGHT - GRAPH_PADDING_TOP - GRAPH_PADDING_BOTTOM;
            int y = (int)(((double)Intensity / (double)MAX_INTENSITY_VALUE) * (double)adjustedHeight);
            y = HEIGHT - y - GRAPH_PADDING_BOTTOM + POS_Y;
            return y;
        }

        private void BakeWavelengthPositions()
        {
            int adjustedWidth = WIDTH - GRAPH_PADDING_LEFT - GRAPH_PADDING_RIGHT;
            double wavelengthStart = Global.WAVELENGTHS[0];

            for (int i = 0; i < Global.SAMPLE_SIZE; i++)
            {
                wavelengthPositions[i] = (int)((double)(Global.WAVELENGTHS[i] - wavelengthStart) / (double)Global.SPECTROMETER_RANGE * (double)adjustedWidth) + GRAPH_PADDING_LEFT + POS_X;
            }
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
                // Clear the drawing area
                g.Clear(parent.BackColor);

                // Fill the area behind the graph with white
                using (Brush whiteBrush = new SolidBrush(Color.Gray))
                {
                    g.FillRectangle(whiteBrush,
                        POS_X + GRAPH_PADDING_LEFT,
                        POS_Y + GRAPH_PADDING_TOP,
                        WIDTH - GRAPH_PADDING_LEFT - GRAPH_PADDING_RIGHT,
                        HEIGHT - GRAPH_PADDING_TOP - GRAPH_PADDING_BOTTOM);
                }

                // Draw axes
                DrawAxes(g);

                // Draw data points
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

                // Draw legend and labels
                DrawLegend(g);
                DrawLabels(g);
            }
        }


        private void DrawSpectraBar()
        {
            UpdateAllColors();

            using (Graphics g = parent.CreateGraphics())
            {
                using (Font font = new Font(GraphFontType, GraphFontSize))
                {
                    SizeF labelSize = g.MeasureString("Example", font);
                    int labelHeight = (int)labelSize.Height;
                    int spectraBarYPosition = POS_Y + HEIGHT - GRAPH_PADDING_BOTTOM + labelHeight + 10;

                    Rectangle rect = new Rectangle(
                        POS_X,                      // X
                        spectraBarYPosition,        // Y
                        WIDTH,                      // Width
                        SPECTRA_BAR_HEIGHT          // Height
                    );

                    // Creates a solid black background which helps define the spectra
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

        public void UpdateSpectrometerData(SpectrometerDataHandler newData)
        {
            data = newData;
            MAX_INTENSITY_VALUE = data.GetMaxIntensity();
        }
    }
}
