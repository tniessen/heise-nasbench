namespace nasbench
{
    partial class ConnectionDialog
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.connectButton = new System.Windows.Forms.Button();
            this.driveList = new nasbench.LogicalDriveDropdownList();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.passwordTextBox = new System.Windows.Forms.MaskedTextBox();
            this.usernameTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.shareTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.connectionList = new nasbench.LogicalDriveDropdownList();
            this.disconnectButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            //
            // groupBox1
            //
            this.groupBox1.Controls.Add(this.connectButton);
            this.groupBox1.Controls.Add(this.driveList);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.passwordTextBox);
            this.groupBox1.Controls.Add(this.usernameTextBox);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.shareTextBox);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(207, 236);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            //
            // connectButton
            //
            this.connectButton.Location = new System.Drawing.Point(9, 193);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(187, 23);
            this.connectButton.TabIndex = 27;
            this.connectButton.Text = "Verbinden";
            this.connectButton.UseVisualStyleBackColor = true;
            //
            // driveList
            //
            this.driveList.Location = new System.Drawing.Point(9, 149);
            this.driveList.Name = "driveList";
            this.driveList.SelectedDrive = null;
            this.driveList.Size = new System.Drawing.Size(187, 21);
            this.driveList.TabIndex = 26;
            //
            // label5
            //
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 133);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(51, 13);
            this.label5.TabIndex = 25;
            this.label5.Text = "Laufwerk";
            //
            // label2
            //
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 19;
            this.label2.Text = "Share";
            //
            // passwordTextBox
            //
            this.passwordTextBox.Location = new System.Drawing.Point(9, 110);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.PasswordChar = '●';
            this.passwordTextBox.Size = new System.Drawing.Size(188, 20);
            this.passwordTextBox.TabIndex = 24;
            //
            // usernameTextBox
            //
            this.usernameTextBox.Location = new System.Drawing.Point(9, 71);
            this.usernameTextBox.Name = "usernameTextBox";
            this.usernameTextBox.Size = new System.Drawing.Size(188, 20);
            this.usernameTextBox.TabIndex = 22;
            //
            // label4
            //
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 94);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 13);
            this.label4.TabIndex = 23;
            this.label4.Text = "Passwort";
            //
            // shareTextBox
            //
            this.shareTextBox.Location = new System.Drawing.Point(9, 32);
            this.shareTextBox.Name = "shareTextBox";
            this.shareTextBox.Size = new System.Drawing.Size(188, 20);
            this.shareTextBox.TabIndex = 20;
            //
            // label3
            //
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 13);
            this.label3.TabIndex = 21;
            this.label3.Text = "Benutzername";
            //
            // groupBox2
            //
            this.groupBox2.Controls.Add(this.connectionList);
            this.groupBox2.Controls.Add(this.disconnectButton);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(13, 255);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(207, 111);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            //
            // connectionList
            //
            this.connectionList.Location = new System.Drawing.Point(7, 32);
            this.connectionList.Name = "connectionList";
            this.connectionList.SelectedDrive = null;
            this.connectionList.Size = new System.Drawing.Size(190, 21);
            this.connectionList.TabIndex = 28;
            //
            // disconnectButton
            //
            this.disconnectButton.Location = new System.Drawing.Point(6, 71);
            this.disconnectButton.Name = "disconnectButton";
            this.disconnectButton.Size = new System.Drawing.Size(191, 23);
            this.disconnectButton.TabIndex = 29;
            this.disconnectButton.Text = "Trennen";
            this.disconnectButton.UseVisualStyleBackColor = true;
            //
            // label1
            //
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 27;
            this.label1.Text = "Verbindung";
            //
            // ConnectionDialog
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(235, 378);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConnectionDialog";
            this.Text = "NAS-Verbindung";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private LogicalDriveDropdownList driveList;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.MaskedTextBox passwordTextBox;
        private System.Windows.Forms.TextBox usernameTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox shareTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.Button disconnectButton;
        private LogicalDriveDropdownList connectionList;

    }
}