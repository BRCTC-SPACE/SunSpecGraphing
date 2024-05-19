namespace SunSpecGraphing
{
    public partial class mainWindow : Form
    {
        private GraphingAPI graph;

        public mainWindow()
        {
            Program.ConsoleAllocator.AllocConsole();
            InitializeComponent();

            // This will ensure that all the Math is correct for now and can/will be changed later
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            graph = new GraphingAPI(500, 300, 10, 100, this);

            SpectrometerHandler.PrintData();

            Global.MAX_Y_VALUE = SpectrometerHandler.currentData.Max();

            Conversions.SetUpConstants();

            graph.BakeWavelengthPositions();
        }

        private void toggleGraphColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Global.UseGraphingColor = !Global.UseGraphingColor;
            graph.DrawDataGraph();
        }
    }
}
