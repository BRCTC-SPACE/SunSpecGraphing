namespace SunSpecGraphing
{
    partial class mainWindow
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            loadFolderToolStripMenuItem = new ToolStripMenuItem();
            saveGraphImageToolStripMenuItem = new ToolStripMenuItem();
            optionsToolStripMenuItem = new ToolStripMenuItem();
            toggleGraphColorToolStripMenuItem = new ToolStripMenuItem();
            dataList = new ListBox();
            dataLoadingProgressBar = new ProgressBar();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, optionsToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1169, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { loadFolderToolStripMenuItem, saveGraphImageToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // loadFolderToolStripMenuItem
            // 
            loadFolderToolStripMenuItem.Name = "loadFolderToolStripMenuItem";
            loadFolderToolStripMenuItem.Size = new Size(169, 22);
            loadFolderToolStripMenuItem.Text = "Load Folder";
            loadFolderToolStripMenuItem.Click += loadFolderToolStripMenuItem_Click;
            // 
            // saveGraphImageToolStripMenuItem
            // 
            saveGraphImageToolStripMenuItem.Name = "saveGraphImageToolStripMenuItem";
            saveGraphImageToolStripMenuItem.Size = new Size(169, 22);
            saveGraphImageToolStripMenuItem.Text = "Save Graph Image";
            saveGraphImageToolStripMenuItem.Click += saveGraphImageToolStripMenuItem_Click;
            // 
            // optionsToolStripMenuItem
            // 
            optionsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { toggleGraphColorToolStripMenuItem });
            optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            optionsToolStripMenuItem.Size = new Size(61, 20);
            optionsToolStripMenuItem.Text = "Options";
            // 
            // toggleGraphColorToolStripMenuItem
            // 
            toggleGraphColorToolStripMenuItem.Name = "toggleGraphColorToolStripMenuItem";
            toggleGraphColorToolStripMenuItem.Size = new Size(176, 22);
            toggleGraphColorToolStripMenuItem.Text = "Toggle Graph Color";
            toggleGraphColorToolStripMenuItem.Click += toggleGraphColorToolStripMenuItem_Click;
            // 
            // dataList
            // 
            dataList.FormattingEnabled = true;
            dataList.ItemHeight = 15;
            dataList.Location = new Point(859, 27);
            dataList.Name = "dataList";
            dataList.Size = new Size(300, 589);
            dataList.TabIndex = 1;
            dataList.SelectedIndexChanged += dataList_SelectedIndexChanged;
            // 
            // dataLoadingProgressBar
            // 
            dataLoadingProgressBar.Location = new Point(12, 593);
            dataLoadingProgressBar.Name = "dataLoadingProgressBar";
            dataLoadingProgressBar.Size = new Size(841, 23);
            dataLoadingProgressBar.TabIndex = 3;
            dataLoadingProgressBar.Visible = false;
            // 
            // mainWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Silver;
            ClientSize = new Size(1169, 629);
            Controls.Add(dataLoadingProgressBar);
            Controls.Add(dataList);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "mainWindow";
            Text = "Spectrometer Data Viewer";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem optionsToolStripMenuItem;
        private ToolStripMenuItem toggleGraphColorToolStripMenuItem;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem loadFolderToolStripMenuItem;
        private ListBox dataList;
        private ProgressBar progressBar1;
        private ProgressBar dataLoadingProgressBar;
        private ToolStripMenuItem saveGraphImageToolStripMenuItem;
    }
}
