namespace SunSpecGraphing
{
    public partial class mainWindow : Form
    {
        private GraphingAPI graph;
        private List<SpectrometerDataHandler> loadedData = null;
        private string activeDataFolder = ""; // TODO: wtf was this for??

        public mainWindow()
        {
            Program.ConsoleAllocator.AllocConsole();
            InitializeComponent();

            // This will ensure that all the math is correct for now and can/will be changed later
            //this.FormBorderStyle = FormBorderStyle.FixedSingle;
            //this.MaximizeBox = false;
            this.Resize += new EventHandler(mainWindow_Resize);
        }

        private void mainWindow_Resize(object sender, EventArgs e)
        {
            if (graph != null)
            {
                // Make the width have padding with the based on the width of the itemList
                int newWidth = this.ClientSize.Width - 300;

                int newHeight = this.ClientSize.Height - 150;

                graph.UpdateDimensions(newWidth, newHeight);
                UpdateGraphs();
            }
        }
        private void UpdateGraphs()
        {
            graph.UpdateGraph();
        }

        private void toggleGraphColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (loadedData != null)
            {
                graph.useGraphingColor = !graph.useGraphingColor;
                UpdateGraphs();
            }
        }

        private async void loadFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                DialogResult result = folderBrowserDialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    string selectedFolder = folderBrowserDialog.SelectedPath;
                    // Console.WriteLine(selectedFolder);
                    string[] files = Directory.GetFiles(selectedFolder);

                    // Initialize progress bar
                    dataLoadingProgressBar.Minimum = 0;
                    dataLoadingProgressBar.Maximum = files.Length;
                    dataLoadingProgressBar.Value = 0;
                    dataLoadingProgressBar.Visible = true;

                    loadedData = new List<SpectrometerDataHandler>();
                    dataList.Items.Clear();

                    // Load files asynchronously
                    foreach (string file in files)
                    {
                        if (file.EndsWith(".spec"))
                        {
                            // Load each file asynchronously
                            SpectrometerDataHandler data = await Task.Run(() => new SpectrometerDataHandler(file));

                            loadedData.Add(data);
                            dataList.Items.Add(file);

                            // Update progress bar
                            dataLoadingProgressBar.Value++;
                        }
                    }

                    if (loadedData.Count > 0)
                    {
                        graph = new GraphingAPI(800, 475, 50, 50, this, loadedData[0]);
                    }
                    dataLoadingProgressBar.Visible = false;
                }
            }
        }


        private void dataList_SelectedIndexChanged(object sender, EventArgs e)
        {
            graph.UpdateSpectrometerData(loadedData[dataList.SelectedIndex]);
            graph.UpdateGraph();
        }

        private void saveGraphImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (loadedData != null)
            {
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "PNG Image|*.png";
                    saveFileDialog.Title = "Save Graph As Image";
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        SaveGraphAsImage(saveFileDialog.FileName);
                    }
                }
            }
        }

        private void SaveGraphAsImage(string filePath)
        {
            // Calculate the total height including the spectra bar

            // TODO: Calculate actual positions
            int spectraBarYPosition = graph.Height + 100; 
            int totalHeight = spectraBarYPosition + graph.SpectraBarHeight;

            int imageWidth = graph.Width + graph.GraphPaddingLeft + graph.GraphPaddingRight + 80;
            int imageHeight = totalHeight;

            using (Bitmap bitmap = new Bitmap(imageWidth, imageHeight))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    // Render the graph and the spectra bar to the bitmap
                    graph.RenderGraph(g);
                    graph.RenderSpectraBar(g);
                }

                bitmap.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
            }
        }
    }
}
