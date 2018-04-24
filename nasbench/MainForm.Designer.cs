namespace nasbench
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.label1 = new System.Windows.Forms.Label();
            this.dirsGroupBox = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.startButton = new System.Windows.Forms.Button();
            this.filesGroupBox = new System.Windows.Forms.GroupBox();
            this.fileGroupGrid = new System.Windows.Forms.DataGridView();
            this.count = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.size = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mainToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.optionsGroupBox = new System.Windows.Forms.GroupBox();
            this.verifyModeDescription = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.verifyModeComboBox = new System.Windows.Forms.ComboBox();
            this.singleFilePerGroupCheckbox = new System.Windows.Forms.CheckBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nasConnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.infoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logBox = new nasbench.LogBox();
            this.logDirInput = new nasbench.PathTextBox();
            this.testFileDirInput = new nasbench.PathTextBox();
            this.targetDirInput = new nasbench.PathTextBox();
            this.dirsGroupBox.SuspendLayout();
            this.filesGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fileGroupGrid)).BeginInit();
            this.optionsGroupBox.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            //
            // label1
            //
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Zielverzeichnis";
            //
            // dirsGroupBox
            //
            this.dirsGroupBox.Controls.Add(this.logDirInput);
            this.dirsGroupBox.Controls.Add(this.label4);
            this.dirsGroupBox.Controls.Add(this.testFileDirInput);
            this.dirsGroupBox.Controls.Add(this.label2);
            this.dirsGroupBox.Controls.Add(this.targetDirInput);
            this.dirsGroupBox.Controls.Add(this.startButton);
            this.dirsGroupBox.Controls.Add(this.label1);
            this.dirsGroupBox.Location = new System.Drawing.Point(12, 34);
            this.dirsGroupBox.Name = "dirsGroupBox";
            this.dirsGroupBox.Padding = new System.Windows.Forms.Padding(9);
            this.dirsGroupBox.Size = new System.Drawing.Size(264, 219);
            this.dirsGroupBox.TabIndex = 9;
            this.dirsGroupBox.TabStop = false;
            this.dirsGroupBox.Text = "Verzeichnisse";
            //
            // label4
            //
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 114);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(132, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Verzeichnis für Logdateien";
            //
            // label2
            //
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(175, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Lokales Verzeichnis für Testdateien";
            //
            // startButton
            //
            this.startButton.Location = new System.Drawing.Point(12, 190);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(240, 23);
            this.startButton.TabIndex = 6;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            //
            // filesGroupBox
            //
            this.filesGroupBox.Controls.Add(this.fileGroupGrid);
            this.filesGroupBox.Location = new System.Drawing.Point(282, 34);
            this.filesGroupBox.Name = "filesGroupBox";
            this.filesGroupBox.Size = new System.Drawing.Size(264, 219);
            this.filesGroupBox.TabIndex = 11;
            this.filesGroupBox.TabStop = false;
            this.filesGroupBox.Text = "Testdateien";
            //
            // fileGroupGrid
            //
            this.fileGroupGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.fileGroupGrid.BackgroundColor = System.Drawing.SystemColors.Window;
            this.fileGroupGrid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.fileGroupGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.fileGroupGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.count,
            this.size});
            this.fileGroupGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.fileGroupGrid.Location = new System.Drawing.Point(6, 22);
            this.fileGroupGrid.Name = "fileGroupGrid";
            this.fileGroupGrid.RowHeadersVisible = false;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            this.fileGroupGrid.RowsDefaultCellStyle = dataGridViewCellStyle1;
            this.fileGroupGrid.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.SystemColors.Window;
            this.fileGroupGrid.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            this.fileGroupGrid.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.fileGroupGrid.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.fileGroupGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.fileGroupGrid.Size = new System.Drawing.Size(252, 191);
            this.fileGroupGrid.TabIndex = 0;
            //
            // count
            //
            this.count.HeaderText = "Anzahl";
            this.count.Name = "count";
            //
            // size
            //
            this.size.HeaderText = "Größe";
            this.size.Name = "size";
            //
            // optionsGroupBox
            //
            this.optionsGroupBox.Controls.Add(this.verifyModeDescription);
            this.optionsGroupBox.Controls.Add(this.label3);
            this.optionsGroupBox.Controls.Add(this.verifyModeComboBox);
            this.optionsGroupBox.Controls.Add(this.singleFilePerGroupCheckbox);
            this.optionsGroupBox.Location = new System.Drawing.Point(553, 34);
            this.optionsGroupBox.Name = "optionsGroupBox";
            this.optionsGroupBox.Size = new System.Drawing.Size(264, 219);
            this.optionsGroupBox.TabIndex = 12;
            this.optionsGroupBox.TabStop = false;
            this.optionsGroupBox.Text = "Optionen";
            //
            // verifyModeDescription
            //
            this.verifyModeDescription.Location = new System.Drawing.Point(6, 92);
            this.verifyModeDescription.Name = "verifyModeDescription";
            this.verifyModeDescription.Size = new System.Drawing.Size(252, 43);
            this.verifyModeDescription.TabIndex = 13;
            this.verifyModeDescription.Text = "Beschreibung";
            //
            // label3
            //
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Inhalte verifizieren";
            //
            // verifyModeComboBox
            //
            this.verifyModeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.verifyModeComboBox.FormattingEnabled = true;
            this.verifyModeComboBox.Location = new System.Drawing.Point(6, 68);
            this.verifyModeComboBox.Name = "verifyModeComboBox";
            this.verifyModeComboBox.Size = new System.Drawing.Size(119, 21);
            this.verifyModeComboBox.TabIndex = 11;
            //
            // singleFilePerGroupCheckbox
            //
            this.singleFilePerGroupCheckbox.AutoSize = true;
            this.singleFilePerGroupCheckbox.Location = new System.Drawing.Point(6, 22);
            this.singleFilePerGroupCheckbox.Name = "singleFilePerGroupCheckbox";
            this.singleFilePerGroupCheckbox.Size = new System.Drawing.Size(164, 17);
            this.singleFilePerGroupCheckbox.TabIndex = 10;
            this.singleFilePerGroupCheckbox.Text = "Reduziere lokale Testdateien";
            this.mainToolTip.SetToolTip(this.singleFilePerGroupCheckbox, "Bildet eine lokale Testdatei auf mehrere Remote-Testdateien ab, sodass ein Vielfa" +
        "ches des lokalen Speicherplatzes auf dem NAS-System getestet werden kann.");
            this.singleFilePerGroupCheckbox.UseVisualStyleBackColor = true;
            //
            // menuStrip1
            //
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolsToolStripMenuItem,
            this.infoToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(826, 24);
            this.menuStrip1.TabIndex = 13;
            this.menuStrip1.Text = "menuStrip1";
            //
            // toolsToolStripMenuItem
            //
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nasConnToolStripMenuItem,
            this.logFileToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            //
            // logFileToolStripMenuItem
            //
            this.logFileToolStripMenuItem.Name = "logFileToolStripMenuItem";
            this.logFileToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.logFileToolStripMenuItem.Text = "Log-Dateien betrachten";
            //
            // nasConnToolStripMenuItem
            //
            this.nasConnToolStripMenuItem.Name = "nasConnToolStripMenuItem";
            this.nasConnToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.nasConnToolStripMenuItem.Text = "NAS-Verbindung";
            //
            // infoToolStripMenuItem
            //
            this.infoToolStripMenuItem.Name = "infoToolStripMenuItem";
            this.infoToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.infoToolStripMenuItem.Text = "Info";
            //
            // logBox
            //
            this.logBox.Location = new System.Drawing.Point(12, 259);
            this.logBox.Name = "logBox";
            this.logBox.Size = new System.Drawing.Size(805, 205);
            this.logBox.TabIndex = 10;
            //
            // logDirInput
            //
            this.logDirInput.Location = new System.Drawing.Point(12, 130);
            this.logDirInput.Name = "logDirInput";
            this.logDirInput.Path = null;
            this.logDirInput.Size = new System.Drawing.Size(240, 23);
            this.logDirInput.TabIndex = 15;
            //
            // testFileDirInput
            //
            this.testFileDirInput.Location = new System.Drawing.Point(12, 85);
            this.testFileDirInput.Name = "testFileDirInput";
            this.testFileDirInput.Path = null;
            this.testFileDirInput.Size = new System.Drawing.Size(240, 23);
            this.testFileDirInput.TabIndex = 13;
            //
            // targetDirInput
            //
            this.targetDirInput.Location = new System.Drawing.Point(12, 39);
            this.targetDirInput.Name = "targetDirInput";
            this.targetDirInput.Path = null;
            this.targetDirInput.Size = new System.Drawing.Size(240, 23);
            this.targetDirInput.TabIndex = 11;
            //
            // MainForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(826, 472);
            this.Controls.Add(this.optionsGroupBox);
            this.Controls.Add(this.filesGroupBox);
            this.Controls.Add(this.logBox);
            this.Controls.Add(this.dirsGroupBox);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "NAS-Benchmark";
            this.dirsGroupBox.ResumeLayout(false);
            this.dirsGroupBox.PerformLayout();
            this.filesGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fileGroupGrid)).EndInit();
            this.optionsGroupBox.ResumeLayout(false);
            this.optionsGroupBox.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox dirsGroupBox;
        private System.Windows.Forms.Button startButton;
        private LogBox logBox;
        private System.Windows.Forms.GroupBox filesGroupBox;
        private System.Windows.Forms.DataGridView fileGroupGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn count;
        private System.Windows.Forms.DataGridViewTextBoxColumn size;
        private System.Windows.Forms.ToolTip mainToolTip;
        private PathTextBox targetDirInput;
        private PathTextBox testFileDirInput;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox optionsGroupBox;
        private System.Windows.Forms.CheckBox singleFilePerGroupCheckbox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox verifyModeComboBox;
        private PathTextBox logDirInput;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem logFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nasConnToolStripMenuItem;
        private System.Windows.Forms.Label verifyModeDescription;
        private System.Windows.Forms.ToolStripMenuItem infoToolStripMenuItem;
    }
}
