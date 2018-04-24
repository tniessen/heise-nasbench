namespace nasbench
{
    partial class LogFileViewerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogFileViewerForm));
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newViewerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainTabControl = new System.Windows.Forms.TabControl();
            this.summaryTabPage = new System.Windows.Forms.TabPage();
            this.SummaryPanel = new nasbench.LogFileSummaryControl();
            this.eventsTabPage = new System.Windows.Forms.TabPage();
            this.LogFileEvents = new nasbench.LogFileEventsControl();
            this.fileTreeTabPage = new System.Windows.Forms.TabPage();
            this.FileTree = new nasbench.LogFileFileTreeControl();
            this.infoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip.SuspendLayout();
            this.mainTabControl.SuspendLayout();
            this.summaryTabPage.SuspendLayout();
            this.eventsTabPage.SuspendLayout();
            this.fileTreeTabPage.SuspendLayout();
            this.SuspendLayout();
            //
            // menuStrip
            //
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.infoToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(785, 24);
            this.menuStrip.TabIndex = 2;
            this.menuStrip.Text = "menuStrip1";
            //
            // fileToolStripMenuItem
            //
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newViewerToolStripMenuItem,
            this.openToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.fileToolStripMenuItem.Text = "Datei";
            //
            // newViewerToolStripMenuItem
            //
            this.newViewerToolStripMenuItem.Name = "newViewerToolStripMenuItem";
            this.newViewerToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newViewerToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.newViewerToolStripMenuItem.Text = "Neues Fenster";
            //
            // openToolStripMenuItem
            //
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.openToolStripMenuItem.Text = "Datei öffnen";
            //
            // exitToolStripMenuItem
            //
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.exitToolStripMenuItem.Text = "Beenden";
            //
            // mainTabControl
            //
            this.mainTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mainTabControl.Controls.Add(this.summaryTabPage);
            this.mainTabControl.Controls.Add(this.eventsTabPage);
            this.mainTabControl.Controls.Add(this.fileTreeTabPage);
            this.mainTabControl.Location = new System.Drawing.Point(12, 27);
            this.mainTabControl.Name = "mainTabControl";
            this.mainTabControl.SelectedIndex = 0;
            this.mainTabControl.Size = new System.Drawing.Size(761, 442);
            this.mainTabControl.TabIndex = 3;
            //
            // summaryTabPage
            //
            this.summaryTabPage.Controls.Add(this.SummaryPanel);
            this.summaryTabPage.Location = new System.Drawing.Point(4, 22);
            this.summaryTabPage.Name = "summaryTabPage";
            this.summaryTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.summaryTabPage.Size = new System.Drawing.Size(753, 416);
            this.summaryTabPage.TabIndex = 0;
            this.summaryTabPage.Text = "Übersicht";
            this.summaryTabPage.UseVisualStyleBackColor = true;
            //
            // SummaryPanel
            //
            this.SummaryPanel.Location = new System.Drawing.Point(6, 6);
            this.SummaryPanel.Name = "SummaryPanel";
            this.SummaryPanel.Size = new System.Drawing.Size(741, 404);
            this.SummaryPanel.TabIndex = 0;
            //
            // eventsTabPage
            //
            this.eventsTabPage.Controls.Add(this.LogFileEvents);
            this.eventsTabPage.Location = new System.Drawing.Point(4, 22);
            this.eventsTabPage.Name = "eventsTabPage";
            this.eventsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.eventsTabPage.Size = new System.Drawing.Size(753, 416);
            this.eventsTabPage.TabIndex = 1;
            this.eventsTabPage.Text = "Ereignisse";
            this.eventsTabPage.UseVisualStyleBackColor = true;
            //
            // LogFileEvents
            //
            this.LogFileEvents.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LogFileEvents.Location = new System.Drawing.Point(7, 7);
            this.LogFileEvents.Name = "LogFileEvents";
            this.LogFileEvents.Size = new System.Drawing.Size(740, 406);
            this.LogFileEvents.TabIndex = 0;
            //
            // fileTreeTabPage
            //
            this.fileTreeTabPage.Controls.Add(this.FileTree);
            this.fileTreeTabPage.Location = new System.Drawing.Point(4, 22);
            this.fileTreeTabPage.Name = "fileTreeTabPage";
            this.fileTreeTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.fileTreeTabPage.Size = new System.Drawing.Size(753, 416);
            this.fileTreeTabPage.TabIndex = 2;
            this.fileTreeTabPage.Text = "Dateien";
            this.fileTreeTabPage.UseVisualStyleBackColor = true;
            //
            // FileTree
            //
            this.FileTree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FileTree.Location = new System.Drawing.Point(6, 6);
            this.FileTree.Name = "FileTree";
            this.FileTree.Size = new System.Drawing.Size(741, 404);
            this.FileTree.TabIndex = 0;
            //
            // infoToolStripMenuItem
            //
            this.infoToolStripMenuItem.Name = "infoToolStripMenuItem";
            this.infoToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.infoToolStripMenuItem.Text = "Info";
            //
            // LogFileViewerForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(785, 481);
            this.Controls.Add(this.mainTabControl);
            this.Controls.Add(this.menuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.Name = "LogFileViewerForm";
            this.Text = "Log-Dateien";
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.mainTabControl.ResumeLayout(false);
            this.summaryTabPage.ResumeLayout(false);
            this.eventsTabPage.ResumeLayout(false);
            this.fileTreeTabPage.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newViewerToolStripMenuItem;
        private System.Windows.Forms.TabControl mainTabControl;
        private System.Windows.Forms.TabPage summaryTabPage;
        private System.Windows.Forms.TabPage eventsTabPage;
        private System.Windows.Forms.TabPage fileTreeTabPage;
        public LogFileSummaryControl SummaryPanel;
        public LogFileFileTreeControl FileTree;
        public LogFileEventsControl LogFileEvents;
        private System.Windows.Forms.ToolStripMenuItem infoToolStripMenuItem;
    }
}