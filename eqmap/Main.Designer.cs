namespace eqmap
{
    partial class Main
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
            this.panelBottomMainScreen = new System.Windows.Forms.Panel();
            this.labelMapCoordinates = new System.Windows.Forms.Label();
            this.panelLeftMainScreen = new System.Windows.Forms.Panel();
            this.tabControlMainPageLeft = new System.Windows.Forms.TabControl();
            this.tabPageFirstMainPageLeft = new System.Windows.Forms.TabPage();
            this.panelZone = new System.Windows.Forms.Panel();
            this.buttonZone = new System.Windows.Forms.Button();
            this.listboxZone = new System.Windows.Forms.ListBox();
            this.tabPageSecondMainPageLeft = new System.Windows.Forms.TabPage();
            this.textBoxCharacterInfo = new System.Windows.Forms.TextBox();
            this.panelRightMainScreen = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panelLeftMainScreen.SuspendLayout();
            this.tabControlMainPageLeft.SuspendLayout();
            this.tabPageFirstMainPageLeft.SuspendLayout();
            this.panelZone.SuspendLayout();
            this.tabPageSecondMainPageLeft.SuspendLayout();
            this.panelRightMainScreen.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panelBottomMainScreen
            // 
            this.panelBottomMainScreen.BackColor = System.Drawing.SystemColors.Control;
            this.panelBottomMainScreen.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottomMainScreen.Location = new System.Drawing.Point(0, 730);
            this.panelBottomMainScreen.Name = "panelBottomMainScreen";
            this.panelBottomMainScreen.Size = new System.Drawing.Size(1083, 42);
            this.panelBottomMainScreen.TabIndex = 0;
            // 
            // labelMapCoordinates
            // 
            this.labelMapCoordinates.AutoSize = true;
            this.labelMapCoordinates.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelMapCoordinates.Location = new System.Drawing.Point(74, 6);
            this.labelMapCoordinates.Name = "labelMapCoordinates";
            this.labelMapCoordinates.Size = new System.Drawing.Size(68, 15);
            this.labelMapCoordinates.TabIndex = 0;
            this.labelMapCoordinates.Text = "Co-ordinates";
            this.labelMapCoordinates.Visible = false;
            // 
            // panelLeftMainScreen
            // 
            this.panelLeftMainScreen.Controls.Add(this.tabControlMainPageLeft);
            this.panelLeftMainScreen.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeftMainScreen.Location = new System.Drawing.Point(0, 0);
            this.panelLeftMainScreen.Name = "panelLeftMainScreen";
            this.panelLeftMainScreen.Size = new System.Drawing.Size(179, 730);
            this.panelLeftMainScreen.TabIndex = 6;
            // 
            // tabControlMainPageLeft
            // 
            this.tabControlMainPageLeft.Controls.Add(this.tabPageFirstMainPageLeft);
            this.tabControlMainPageLeft.Controls.Add(this.tabPageSecondMainPageLeft);
            this.tabControlMainPageLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMainPageLeft.Location = new System.Drawing.Point(0, 0);
            this.tabControlMainPageLeft.Name = "tabControlMainPageLeft";
            this.tabControlMainPageLeft.SelectedIndex = 0;
            this.tabControlMainPageLeft.Size = new System.Drawing.Size(179, 730);
            this.tabControlMainPageLeft.TabIndex = 6;
            // 
            // tabPageFirstMainPageLeft
            // 
            this.tabPageFirstMainPageLeft.Controls.Add(this.panelZone);
            this.tabPageFirstMainPageLeft.Location = new System.Drawing.Point(4, 22);
            this.tabPageFirstMainPageLeft.Name = "tabPageFirstMainPageLeft";
            this.tabPageFirstMainPageLeft.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageFirstMainPageLeft.Size = new System.Drawing.Size(171, 704);
            this.tabPageFirstMainPageLeft.TabIndex = 0;
            this.tabPageFirstMainPageLeft.Text = "Zones";
            this.tabPageFirstMainPageLeft.UseVisualStyleBackColor = true;
            // 
            // panelZone
            // 
            this.panelZone.Controls.Add(this.buttonZone);
            this.panelZone.Controls.Add(this.listboxZone);
            this.panelZone.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelZone.Location = new System.Drawing.Point(3, 3);
            this.panelZone.Name = "panelZone";
            this.panelZone.Size = new System.Drawing.Size(165, 192);
            this.panelZone.TabIndex = 4;
            // 
            // buttonZone
            // 
            this.buttonZone.Enabled = false;
            this.buttonZone.Location = new System.Drawing.Point(58, 160);
            this.buttonZone.Name = "buttonZone";
            this.buttonZone.Size = new System.Drawing.Size(72, 26);
            this.buttonZone.TabIndex = 3;
            this.buttonZone.Text = "Zone";
            this.buttonZone.UseVisualStyleBackColor = true;
            // 
            // listboxZone
            // 
            this.listboxZone.FormattingEnabled = true;
            this.listboxZone.Location = new System.Drawing.Point(3, 7);
            this.listboxZone.Name = "listboxZone";
            this.listboxZone.Size = new System.Drawing.Size(159, 147);
            this.listboxZone.TabIndex = 2;
            // 
            // tabPageSecondMainPageLeft
            // 
            this.tabPageSecondMainPageLeft.Controls.Add(this.textBoxCharacterInfo);
            this.tabPageSecondMainPageLeft.Location = new System.Drawing.Point(4, 22);
            this.tabPageSecondMainPageLeft.Name = "tabPageSecondMainPageLeft";
            this.tabPageSecondMainPageLeft.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSecondMainPageLeft.Size = new System.Drawing.Size(171, 704);
            this.tabPageSecondMainPageLeft.TabIndex = 1;
            this.tabPageSecondMainPageLeft.Text = "Character";
            this.tabPageSecondMainPageLeft.UseVisualStyleBackColor = true;
            // 
            // textBoxCharacterInfo
            // 
            this.textBoxCharacterInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxCharacterInfo.Location = new System.Drawing.Point(3, 3);
            this.textBoxCharacterInfo.Multiline = true;
            this.textBoxCharacterInfo.Name = "textBoxCharacterInfo";
            this.textBoxCharacterInfo.ReadOnly = true;
            this.textBoxCharacterInfo.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxCharacterInfo.Size = new System.Drawing.Size(165, 698);
            this.textBoxCharacterInfo.TabIndex = 0;
            // 
            // panelRightMainScreen
            // 
            this.panelRightMainScreen.AutoScroll = true;
            this.panelRightMainScreen.Controls.Add(this.pictureBox1);
            this.panelRightMainScreen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRightMainScreen.Location = new System.Drawing.Point(179, 0);
            this.panelRightMainScreen.Name = "panelRightMainScreen";
            this.panelRightMainScreen.Size = new System.Drawing.Size(904, 730);
            this.panelRightMainScreen.TabIndex = 7;
            // 
            // pictureBox1
            //             
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(904, 730);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1083, 772);
            this.Controls.Add(this.panelRightMainScreen);
            this.Controls.Add(this.panelLeftMainScreen);
            this.Controls.Add(this.panelBottomMainScreen);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Main";
            this.Text = "eqemu - scripting engine";
            this.panelLeftMainScreen.ResumeLayout(false);
            this.tabControlMainPageLeft.ResumeLayout(false);
            this.tabPageFirstMainPageLeft.ResumeLayout(false);
            this.panelZone.ResumeLayout(false);
            this.tabPageSecondMainPageLeft.ResumeLayout(false);
            this.tabPageSecondMainPageLeft.PerformLayout();
            this.panelRightMainScreen.ResumeLayout(false);
            this.panelRightMainScreen.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.Panel panelBottomMainScreen;
        private System.Windows.Forms.Label labelMapCoordinates;
        private System.Windows.Forms.Panel panelLeftMainScreen;
        private System.Windows.Forms.TabControl tabControlMainPageLeft;
        private System.Windows.Forms.TabPage tabPageFirstMainPageLeft;
        private System.Windows.Forms.Panel panelZone;
        private System.Windows.Forms.Button buttonZone;
        private System.Windows.Forms.ListBox listboxZone;
        private System.Windows.Forms.TabPage tabPageSecondMainPageLeft;
        private System.Windows.Forms.TextBox textBoxCharacterInfo;
        private System.Windows.Forms.Panel panelRightMainScreen;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

