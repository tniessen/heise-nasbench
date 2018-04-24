namespace nasbench
{
    partial class LogFileEventsControl
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.eventGridView = new System.Windows.Forms.DataGridView();
            this.timeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.typeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.detailsColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.eventGridView)).BeginInit();
            this.SuspendLayout();
            //
            // eventGridView
            //
            this.eventGridView.AllowUserToAddRows = false;
            this.eventGridView.AllowUserToDeleteRows = false;
            this.eventGridView.AllowUserToResizeRows = false;
            this.eventGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.eventGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.eventGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.timeColumn,
            this.typeColumn,
            this.detailsColumn});
            this.eventGridView.Location = new System.Drawing.Point(4, 3);
            this.eventGridView.Name = "eventGridView";
            this.eventGridView.ReadOnly = true;
            this.eventGridView.RowHeadersVisible = false;
            this.eventGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.eventGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.eventGridView.Size = new System.Drawing.Size(593, 394);
            this.eventGridView.TabIndex = 0;
            //
            // timeColumn
            //
            this.timeColumn.HeaderText = "Zeit";
            this.timeColumn.Name = "timeColumn";
            this.timeColumn.ReadOnly = true;
            //
            // typeColumn
            //
            this.typeColumn.HeaderText = "Typ";
            this.typeColumn.Name = "typeColumn";
            this.typeColumn.ReadOnly = true;
            this.typeColumn.Width = 180;
            //
            // detailsColumn
            //
            this.detailsColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.detailsColumn.HeaderText = "Details";
            this.detailsColumn.Name = "detailsColumn";
            this.detailsColumn.ReadOnly = true;
            //
            // LogFileEventsControl
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.eventGridView);
            this.Name = "LogFileEventsControl";
            this.Size = new System.Drawing.Size(600, 400);
            ((System.ComponentModel.ISupportInitialize)(this.eventGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView eventGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn timeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn typeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn detailsColumn;

    }
}
