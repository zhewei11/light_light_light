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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panelWaveform = new System.Windows.Forms.Panel();
            this.playPauseButton = new System.Windows.Forms.Button();
            this.timeLabel = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
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
            this.groupBox3.Location = new System.Drawing.Point(976, 17);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(594, 440);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "groupBox3";
            // 
            // groupBox2
            // 
            this.groupBox2.Location = new System.Drawing.Point(352, 17);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(601, 440);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "groupBox2";
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(12, 17);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(324, 440);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
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
    }
}

