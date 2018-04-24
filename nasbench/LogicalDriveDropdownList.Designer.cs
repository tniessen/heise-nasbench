namespace nasbench
{
    partial class LogicalDriveDropdownList
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
            this.driveComboBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            //
            // driveComboBox
            //
            this.driveComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.driveComboBox.FormattingEnabled = true;
            this.driveComboBox.Location = new System.Drawing.Point(0, 0);
            this.driveComboBox.Name = "driveComboBox";
            this.driveComboBox.Size = new System.Drawing.Size(150, 21);
            this.driveComboBox.TabIndex = 0;
            //
            // LogicalDriveDropdownList
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.driveComboBox);
            this.Name = "LogicalDriveDropdownList";
            this.Size = new System.Drawing.Size(150, 21);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox driveComboBox;
    }
}
