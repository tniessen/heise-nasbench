namespace nasbench
{
    partial class BenchmarkProgressDialog
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
            this.mainProgressLabel = new System.Windows.Forms.Label();
            this.mainProgressBar = new System.Windows.Forms.ProgressBar();
            this.subProgressLabel = new System.Windows.Forms.Label();
            this.subProgressBar = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            //
            // mainProgressLabel
            //
            this.mainProgressLabel.AutoSize = true;
            this.mainProgressLabel.Location = new System.Drawing.Point(13, 13);
            this.mainProgressLabel.Name = "mainProgressLabel";
            this.mainProgressLabel.Size = new System.Drawing.Size(35, 13);
            this.mainProgressLabel.TabIndex = 0;
            this.mainProgressLabel.Text = "label1";
            //
            // mainProgressBar
            //
            this.mainProgressBar.Location = new System.Drawing.Point(13, 30);
            this.mainProgressBar.Name = "mainProgressBar";
            this.mainProgressBar.Size = new System.Drawing.Size(515, 23);
            this.mainProgressBar.TabIndex = 1;
            //
            // subProgressLabel
            //
            this.subProgressLabel.AutoSize = true;
            this.subProgressLabel.Location = new System.Drawing.Point(13, 71);
            this.subProgressLabel.Name = "subProgressLabel";
            this.subProgressLabel.Size = new System.Drawing.Size(35, 13);
            this.subProgressLabel.TabIndex = 2;
            this.subProgressLabel.Text = "label2";
            //
            // subProgressBar
            //
            this.subProgressBar.Location = new System.Drawing.Point(12, 87);
            this.subProgressBar.Name = "subProgressBar";
            this.subProgressBar.Size = new System.Drawing.Size(515, 23);
            this.subProgressBar.TabIndex = 3;
            //
            // BenchmarkProgressDialog
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(540, 133);
            this.ControlBox = false;
            this.Controls.Add(this.subProgressBar);
            this.Controls.Add(this.subProgressLabel);
            this.Controls.Add(this.mainProgressBar);
            this.Controls.Add(this.mainProgressLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "BenchmarkProgressDialog";
            this.ShowInTaskbar = false;
            this.Text = "Fortschritt";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label mainProgressLabel;
        private System.Windows.Forms.ProgressBar mainProgressBar;
        private System.Windows.Forms.Label subProgressLabel;
        private System.Windows.Forms.ProgressBar subProgressBar;

    }
}