using System;
using NAudio;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio.Wave;

namespace LightSnake
{
    public partial class Form1 : Form
    {
        private List<float> waveformSamples;
        private int sampleRate;
        private List<KeyValuePair<int, string>> keys = new List<KeyValuePair<int, string>>();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void LoadAudioMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "音頻文件|*.mp3;*.wav;*.m4a;*.aac|所有文件|*.*";
                openFileDialog.Title = "選擇音頻文件";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    try
                    {
                        LoadAudioFile(filePath);

                        // 更新UI元素
                        UpdateUIAfterAudioLoad();

                        // 顯示成功消息
                        MessageBox.Show("音頻文件已成功載入", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"載入音頻文件時發生錯誤：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void UpdateUIAfterAudioLoad()
        {
            // 啟用相關的UI控件
            //playPauseButton.Enabled = true;
            //stopButton.Enabled = true;
            //timelineTrackBar.Enabled = true;

            // 設置時間軸的最大值
            if (waveformSamples != null && sampleRate > 0)
            {
                //timelineTrackBar.Maximum = waveformSamples.Count / sampleRate;
            }

            // 重繪波形圖
            panelWaveform.Invalidate();
        }
        private void LoadAudioFile(string filePath)
        {
            using (var reader = new AudioFileReader(filePath))
            {
                waveformSamples = new List<float>();
                sampleRate = reader.WaveFormat.SampleRate;
                var buffer = new float[reader.WaveFormat.SampleRate * reader.WaveFormat.Channels];
                int read;
                while ((read = reader.Read(buffer, 0, buffer.Length)) > 0)
                {
                    waveformSamples.AddRange(buffer.Take(read));
                }
            }
            panelWaveform.Invalidate(); // 重新繪製波形圖
        }

        private void panelWaveform_Paint(object sender, PaintEventArgs e)
        {
            if (waveformSamples == null || waveformSamples.Count == 0)
            {
                return;
            }

            var g = e.Graphics;
            var pen = new Pen(Color.Blue);
            var midHeight = panelWaveform.Height / 2;
            var widthScale = (float)panelWaveform.Width / waveformSamples.Count;

            // 繪製波形圖
            for (int i = 0; i < waveformSamples.Count - 1; i++)
            {
                var x1 = i * widthScale;
                var y1 = midHeight - (waveformSamples[i] * midHeight);
                var x2 = (i + 1) * widthScale;
                var y2 = midHeight - (waveformSamples[i + 1] * midHeight);
                g.DrawLine(pen, x1, y1, x2, y2);
            }

            // 繪製時間軸
            var timePen = new Pen(Color.Gray);
            for (int i = 0; i < panelWaveform.Width; i += 50)
            {
                var time = (i / widthScale) / sampleRate;
                g.DrawLine(timePen, i, 0, i, panelWaveform.Height);
                g.DrawString(time.ToString("0.0") + "s", this.Font, Brushes.Black, i, 0);
            }

            // 繪製Key
            var keyPen = new Pen(Color.Red);
            foreach (var key in keys)
            {
                var x = key.Key * widthScale;
                g.DrawLine(keyPen, x, 0, x, panelWaveform.Height);
                g.DrawString(key.Value, this.Font, Brushes.Red, x, midHeight);
            }
        }

        private void panelWaveform_MouseClick(object sender, MouseEventArgs e)
        {
            if (waveformSamples == null || waveformSamples.Count == 0)
            {
                return;
            }

            var time = (e.X / (float)panelWaveform.Width) * waveformSamples.Count / sampleRate;
            keys.Add(new KeyValuePair<int, string>((int)(time * sampleRate), "Key"));
            panelWaveform.Invalidate(); // 重新繪製波形圖
        }
    }

}
