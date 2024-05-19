namespace SunSpecGraphing
{
    public partial class mainWindow : Form
    {
        private GraphingAPI graph;
        private SpectrometerDataHandler currentData;

        public mainWindow()
        {
            Program.ConsoleAllocator.AllocConsole();
            InitializeComponent();

            // This will ensure that all the Math is correct for now and can/will be changed later
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            currentData = new SpectrometerDataHandler("C:\\Users\\pokem\\Desktop\\Share\\AllData\\data.spec");
            graph = new GraphingAPI(800, 500, 0, 75, this, currentData);
        }

        private void UpdateGraphs()
        {
            try
            {
                graph.UpdateGraph();
            } 
            catch
            {

            }
        }

        private void toggleGraphColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            graph.useGraphingColor = !graph.useGraphingColor;
            UpdateGraphs();
        }
    }
}
