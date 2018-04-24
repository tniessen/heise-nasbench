namespace nasbench
{
    partial class PathTextBox
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
            this.rawPathTextBox = new System.Windows.Forms.TextBox();
            this.browseButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // rawPathTextBox
            //
            this.rawPathTextBox.Location = new System.Drawing.Point(0, 1);
            this.rawPathTextBox.Name = "rawPathTextBox";
            this.rawPathTextBox.Size = new System.Drawing.Size(213, 20);
            this.rawPathTextBox.TabIndex = 0;
            //
            // browseButton
            //
            this.browseButton.Location = new System.Drawing.Point(219, 0);
            this.browseButton.Name = "browseButton";
            this.browseButton.Size = new System.Drawing.Size(35, 23);
            this.browseButton.TabIndex = 1;
            this.browseButton.Text = "...";
            this.browseButton.UseVisualStyleBackColor = true;
            //
            // PathTextBox
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.browseButton);
            this.Controls.Add(this.rawPathTextBox);
            this.Name = "PathTextBox";
            this.Size = new System.Drawing.Size(254, 23);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox rawPathTextBox;
        private System.Windows.Forms.Button browseButton;
    }
}
