using System.Drawing.Drawing2D;

namespace SunSpecGraphing
{
    public partial class mainWindow : Form
    {
        GraphingAPI graph = new GraphingAPI(500, 300, 10, 0);
        public mainWindow()
        {
            Program.ConsoleAllocator.AllocConsole();
            InitializeComponent();
            // This will ensure that all the Math is correct for now and can/will be changed later
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            SpectrometerHandler.PrintData();
            Global.MAX_Y_VALUE = SpectrometerHandler.currentData.Max();
            Conversions.SetUpConstants();
            graph.BakeWavelengthPositions();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Conversions.UpdateAllColors();

            Graphics g = e.Graphics;
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
            graph.DrawData(g);
        }
    }
}
