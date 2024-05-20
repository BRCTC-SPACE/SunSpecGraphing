namespace SunSpecGraphing
{
    public partial class mainWindow : Form
    {
        private GraphingAPI graph;
        private SpectrometerDataHandler currentData;
        private System.Windows.Forms.Timer updateTimer;

        public mainWindow()
        {
            Program.ConsoleAllocator.AllocConsole();
            InitializeComponent();

            // This will ensure that all the Math is correct for now and can/will be changed later
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            currentData = new SpectrometerDataHandler("C:\\Users\\pokem\\Desktop\\Share\\AllData\\data.spec");
            graph = new GraphingAPI(600, 400, 100, 75, this, currentData);

            updateTimer = new System.Windows.Forms.Timer();
            updateTimer.Interval = 10000; // 10 seconds
            updateTimer.Tick += new EventHandler(UpdateTimer_Tick);
            updateTimer.Start();

        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            //graph.UpdateSpectrometerData();
            UpdateGraphs();
        }


        private void UpdateGraphs()
        {
            graph.UpdateGraph();
        }

        private void toggleGraphColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            graph.useGraphingColor = !graph.useGraphingColor;
            UpdateGraphs();
        }
    }
}
