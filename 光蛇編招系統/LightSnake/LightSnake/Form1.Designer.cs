namespace LightSnake
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.檔案ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LoadAudioMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.讀入ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.匯出模式檔ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.編輯ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.複製keyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.貼上keyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lab_XH_p2 = new System.Windows.Forms.Label();
            this.txt_XH_p2 = new System.Windows.Forms.TextBox();
            this.lab_XH_p1 = new System.Windows.Forms.Label();
            this.txt_XH_p1 = new System.Windows.Forms.TextBox();
            this.lab_XH_Lower = new System.Windows.Forms.Label();
            this.txt_XH_Lower = new System.Windows.Forms.TextBox();
            this.lab_XH_Range = new System.Windows.Forms.Label();
            this.combox_XH = new System.Windows.Forms.ComboBox();
            this.txt_XH_Range = new System.Windows.Forms.TextBox();
            this.lab_XH = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panelWaveform = new System.Windows.Forms.Panel();
            this.playPauseButton = new System.Windows.Forms.Button();
            this.timeLabel = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.檔案ToolStripMenuItem,
            this.編輯ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1582, 27);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 檔案ToolStripMenuItem
            // 
            this.檔案ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LoadAudioMenuItem,
            this.讀入ToolStripMenuItem,
            this.匯出模式檔ToolStripMenuItem});
            this.檔案ToolStripMenuItem.Name = "檔案ToolStripMenuItem";
            this.檔案ToolStripMenuItem.Size = new System.Drawing.Size(53, 23);
            this.檔案ToolStripMenuItem.Text = "檔案";
            // 
            // LoadAudioMenuItem
            // 
            this.LoadAudioMenuItem.Name = "LoadAudioMenuItem";
            this.LoadAudioMenuItem.Size = new System.Drawing.Size(167, 26);
            this.LoadAudioMenuItem.Text = "載入音樂";
            this.LoadAudioMenuItem.Click += new System.EventHandler(this.LoadAudioMenuItem_Click);
            // 
            // 讀入ToolStripMenuItem
            // 
            this.讀入ToolStripMenuItem.Name = "讀入ToolStripMenuItem";
            this.讀入ToolStripMenuItem.Size = new System.Drawing.Size(167, 26);
            this.讀入ToolStripMenuItem.Text = "讀入模式檔";
            // 
            // 匯出模式檔ToolStripMenuItem
            // 
            this.匯出模式檔ToolStripMenuItem.Name = "匯出模式檔ToolStripMenuItem";
            this.匯出模式檔ToolStripMenuItem.Size = new System.Drawing.Size(167, 26);
            this.匯出模式檔ToolStripMenuItem.Text = "匯出模式檔";
            // 
            // 編輯ToolStripMenuItem
            // 
            this.編輯ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.複製keyToolStripMenuItem,
            this.貼上keyToolStripMenuItem});
            this.編輯ToolStripMenuItem.Name = "編輯ToolStripMenuItem";
            this.編輯ToolStripMenuItem.Size = new System.Drawing.Size(53, 23);
            this.編輯ToolStripMenuItem.Text = "編輯";
            // 
            // 複製keyToolStripMenuItem
            // 
            this.複製keyToolStripMenuItem.Name = "複製keyToolStripMenuItem";
            this.複製keyToolStripMenuItem.Size = new System.Drawing.Size(146, 26);
            this.複製keyToolStripMenuItem.Text = "複製key";
            // 
            // 貼上keyToolStripMenuItem
            // 
            this.貼上keyToolStripMenuItem.Name = "貼上keyToolStripMenuItem";
            this.貼上keyToolStripMenuItem.Size = new System.Drawing.Size(146, 26);
            this.貼上keyToolStripMenuItem.Text = "貼上key";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Location = new System.Drawing.Point(0, 30);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1582, 459);
            this.panel1.TabIndex = 2;
            // 
            // groupBox3
            // 
            this.groupBox3.Location = new System.Drawing.Point(1141, 17);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(429, 440);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "groupBox3";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lab_XH_p2);
            this.groupBox2.Controls.Add(this.txt_XH_p2);
            this.groupBox2.Controls.Add(this.lab_XH_p1);
            this.groupBox2.Controls.Add(this.txt_XH_p1);
            this.groupBox2.Controls.Add(this.lab_XH_Lower);
            this.groupBox2.Controls.Add(this.txt_XH_Lower);
            this.groupBox2.Controls.Add(this.lab_XH_Range);
            this.groupBox2.Controls.Add(this.combox_XH);
            this.groupBox2.Controls.Add(this.txt_XH_Range);
            this.groupBox2.Controls.Add(this.lab_XH);
            this.groupBox2.Location = new System.Drawing.Point(352, 17);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(783, 440);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "groupBox2";
            // 
            // lab_XH_p2
            // 
            this.lab_XH_p2.AutoSize = true;
            this.lab_XH_p2.Location = new System.Drawing.Point(538, 76);
            this.lab_XH_p2.Name = "lab_XH_p2";
            this.lab_XH_p2.Size = new System.Drawing.Size(21, 15);
            this.lab_XH_p2.TabIndex = 9;
            this.lab_XH_p2.Text = "p2";
            // 
            // txt_XH_p2
            // 
            this.txt_XH_p2.Location = new System.Drawing.Point(585, 73);
            this.txt_XH_p2.Name = "txt_XH_p2";
            this.txt_XH_p2.Size = new System.Drawing.Size(43, 25);
            this.txt_XH_p2.TabIndex = 8;
            // 
            // lab_XH_p1
            // 
            this.lab_XH_p1.AutoSize = true;
            this.lab_XH_p1.Location = new System.Drawing.Point(420, 76);
            this.lab_XH_p1.Name = "lab_XH_p1";
            this.lab_XH_p1.Size = new System.Drawing.Size(21, 15);
            this.lab_XH_p1.TabIndex = 7;
            this.lab_XH_p1.Text = "p1";
            // 
            // txt_XH_p1
            // 
            this.txt_XH_p1.Location = new System.Drawing.Point(467, 73);
            this.txt_XH_p1.Name = "txt_XH_p1";
            this.txt_XH_p1.Size = new System.Drawing.Size(43, 25);
            this.txt_XH_p1.TabIndex = 6;
            // 
            // lab_XH_Lower
            // 
            this.lab_XH_Lower.AutoSize = true;
            this.lab_XH_Lower.Location = new System.Drawing.Point(305, 76);
            this.lab_XH_Lower.Name = "lab_XH_Lower";
            this.lab_XH_Lower.Size = new System.Drawing.Size(44, 15);
            this.lab_XH_Lower.TabIndex = 5;
            this.lab_XH_Lower.Text = "Lower";
            // 
            // txt_XH_Lower
            // 
            this.txt_XH_Lower.Location = new System.Drawing.Point(352, 73);
            this.txt_XH_Lower.Name = "txt_XH_Lower";
            this.txt_XH_Lower.Size = new System.Drawing.Size(43, 25);
            this.txt_XH_Lower.TabIndex = 4;
            // 
            // lab_XH_Range
            // 
            this.lab_XH_Range.AutoSize = true;
            this.lab_XH_Range.Location = new System.Drawing.Point(193, 76);
            this.lab_XH_Range.Name = "lab_XH_Range";
            this.lab_XH_Range.Size = new System.Drawing.Size(42, 15);
            this.lab_XH_Range.TabIndex = 3;
            this.lab_XH_Range.Text = "Range";
            // 
            // combox_XH
            // 
            this.combox_XH.FormattingEnabled = true;
            this.combox_XH.Items.AddRange(new object[] {
            "ramp",
            "tri",
            "pulse",
            "step"});
            this.combox_XH.Location = new System.Drawing.Point(96, 73);
            this.combox_XH.Name = "combox_XH";
            this.combox_XH.Size = new System.Drawing.Size(62, 23);
            this.combox_XH.TabIndex = 2;
            // 
            // txt_XH_Range
            // 
            this.txt_XH_Range.Location = new System.Drawing.Point(240, 73);
            this.txt_XH_Range.Name = "txt_XH_Range";
            this.txt_XH_Range.Size = new System.Drawing.Size(43, 25);
            this.txt_XH_Range.TabIndex = 1;
            // 
            // lab_XH
            // 
            this.lab_XH.AutoSize = true;
            this.lab_XH.Location = new System.Drawing.Point(19, 76);
            this.lab_XH.Name = "lab_XH";
            this.lab_XH.Size = new System.Drawing.Size(27, 15);
            this.lab_XH.TabIndex = 0;
            this.lab_XH.Text = "XH";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listBox1);
            this.groupBox1.Location = new System.Drawing.Point(12, 17);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(324, 440);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 15;
            this.listBox1.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5"});
            this.listBox1.Location = new System.Drawing.Point(15, 24);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(292, 409);
            this.listBox1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Location = new System.Drawing.Point(0, 524);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1582, 496);
            this.panel2.TabIndex = 3;
            // 
            // panelWaveform
            // 
            this.panelWaveform.Location = new System.Drawing.Point(39, 535);
            this.panelWaveform.Name = "panelWaveform";
            this.panelWaveform.Size = new System.Drawing.Size(1449, 226);
            this.panelWaveform.TabIndex = 3;
            this.panelWaveform.Paint += new System.Windows.Forms.PaintEventHandler(this.panelWaveform_Paint);
            this.panelWaveform.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panelWaveform_MouseClick);
            // 
            // playPauseButton
            // 
            this.playPauseButton.Location = new System.Drawing.Point(72, 498);
            this.playPauseButton.Name = "playPauseButton";
            this.playPauseButton.Size = new System.Drawing.Size(75, 27);
            this.playPauseButton.TabIndex = 4;
            this.playPauseButton.Text = "play/pause";
            this.playPauseButton.UseVisualStyleBackColor = true;
            this.playPauseButton.Click += new System.EventHandler(this.PlayPauseButton_Click);
            // 
            // timeLabel
            // 
            this.timeLabel.AutoSize = true;
            this.timeLabel.Location = new System.Drawing.Point(153, 504);
            this.timeLabel.Name = "timeLabel";
            this.timeLabel.Size = new System.Drawing.Size(41, 15);
            this.timeLabel.TabIndex = 5;
            this.timeLabel.Text = "label1";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(877, 501);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1582, 831);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.timeLabel);
            this.Controls.Add(this.playPauseButton);
            this.Controls.Add(this.panelWaveform);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "光蛇編招系統";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 檔案ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem LoadAudioMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 編輯ToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 讀入ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 匯出模式檔ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 複製keyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 貼上keyToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panelWaveform;
        private System.Windows.Forms.Button playPauseButton;
        private System.Windows.Forms.Label timeLabel;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txt_XH_Range;
        private System.Windows.Forms.Label lab_XH;
        private System.Windows.Forms.ComboBox combox_XH;
        private System.Windows.Forms.Label lab_XH_Range;
        private System.Windows.Forms.Label lab_XH_p2;
        private System.Windows.Forms.TextBox txt_XH_p2;
        private System.Windows.Forms.Label lab_XH_p1;
        private System.Windows.Forms.TextBox txt_XH_p1;
        private System.Windows.Forms.Label lab_XH_Lower;
        private System.Windows.Forms.TextBox txt_XH_Lower;
        private System.Windows.Forms.ListBox listBox1;
    }
}

