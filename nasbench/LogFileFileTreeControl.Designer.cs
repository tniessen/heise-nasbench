namespace nasbench
{
    partial class LogFileFileTreeControl
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
            this.performanceTreeView = new System.Windows.Forms.TreeView();
            this.mainSplitContainer = new System.Windows.Forms.SplitContainer();
            this.detailsTextBox = new System.Windows.Forms.TextBox();
            this.performanceGraphView = new nasbench.PerformanceGraphView();
            this.newViewerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainSplitContainer.Panel1.SuspendLayout();
            this.mainSplitContainer.Panel2.SuspendLayout();
            this.mainSplitContainer.SuspendLayout();
            this.SuspendLayout();
            //
            // performanceTreeView
            //
            this.performanceTreeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.performanceTreeView.Location = new System.Drawing.Point(3, 3);
            this.performanceTreeView.Name = "performanceTreeView";
            this.performanceTreeView.Size = new System.Drawing.Size(196, 471);
            this.performanceTreeView.TabIndex = 0;
            //
            // mainSplitContainer
            //
            this.mainSplitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mainSplitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.mainSplitContainer.Location = new System.Drawing.Point(3, 3);
            this.mainSplitContainer.Name = "mainSplitContainer";
            //
            // mainSplitContainer.Panel1
            //
            this.mainSplitContainer.Panel1.Controls.Add(this.performanceTreeView);
            //
            // mainSplitContainer.Panel2
            //
            this.mainSplitContainer.Panel2.Controls.Add(this.detailsTextBox);
            this.mainSplitContainer.Panel2.Controls.Add(this.performanceGraphView);
            this.mainSplitContainer.Size = new System.Drawing.Size(779, 475);
            this.mainSplitContainer.SplitterDistance = 200;
            this.mainSplitContainer.TabIndex = 4;
            //
            // detailsTextBox
            //
            this.detailsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.detailsTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.detailsTextBox.Location = new System.Drawing.Point(4, 4);
            this.detailsTextBox.Multiline = true;
            this.detailsTextBox.Name = "detailsTextBox";
            this.detailsTextBox.ReadOnly = true;
            this.detailsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.detailsTextBox.Size = new System.Drawing.Size(568, 179);
            this.detailsTextBox.TabIndex = 4;
            //
            // performanceGraphView
            //
            this.performanceGraphView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.performanceGraphView.Location = new System.Drawing.Point(3, 189);
            this.performanceGraphView.Name = "performanceGraphView";
            this.performanceGraphView.Size = new System.Drawing.Size(569, 283);
            this.performanceGraphView.TabIndex = 3;
            //
            // newViewerToolStripMenuItem
            //
            this.newViewerToolStripMenuItem.Name = "newViewerToolStripMenuItem";
            this.newViewerToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newViewerToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.newViewerToolStripMenuItem.Text = "Neues Fenster";
            //
            // LogFileFileTreeControl
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mainSplitContainer);
            this.Name = "LogFileFileTreeControl";
            this.Size = new System.Drawing.Size(785, 481);
            this.mainSplitContainer.Panel1.ResumeLayout(false);
            this.mainSplitContainer.Panel2.ResumeLayout(false);
            this.mainSplitContainer.Panel2.PerformLayout();
            this.mainSplitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView performanceTreeView;
        private PerformanceGraphView performanceGraphView;
        private System.Windows.Forms.SplitContainer mainSplitContainer;
        private System.Windows.Forms.TextBox detailsTextBox;
        private System.Windows.Forms.ToolStripMenuItem newViewerToolStripMenuItem;
    }
}