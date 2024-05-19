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

            graph = new GraphingAPI(800, 500, 0, 75, this);

            SpectrometerHandler.PrintData();

            Global.MAX_GRAPH_Y_VALUE = SpectrometerHandler.currentData.Max();

            Conversions.SetUpConstants();

            graph.BakeWavelengthPositions();
        }

        private void UpdateGraphs()
        {
            graph.DrawDataGraph();

            // TODO OPT: Maybe have a seperate call for this because it doesn't change
            // This may help performance if needed
            graph.DrawSpectraBar();
        }

        private void toggleGraphColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Global.UseGraphingColor = !Global.UseGraphingColor;
            UpdateGraphs();
        }
    }
}
