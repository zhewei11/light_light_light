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
using System.Drawing.Drawing2D;

namespace LightSnake
{
    public partial class Form1 : Form
    {
        private List<float> waveformSamples;
        private List<PointF> fullResolutionWaveform;
        private int sampleRate;
        private List<KeyValuePair<int, string>> keys = new List<KeyValuePair<int, string>>();
        private Bitmap waveformBitmap;
        private float zoomFactor = 1.0f;
        private HScrollBar hScrollBar;
        private WaveOutEvent outputDevice;
        private AudioFileReader audioFile;
        private Timer playbackTimer;

        public Form1()
        {
            InitializeComponent();
            InitializeScrollBar();
            InitializePlaybackControls();
        }

        private void InitializeScrollBar()
        {
            hScrollBar = new HScrollBar();
            hScrollBar.Dock = DockStyle.Bottom;
            hScrollBar.Visible = false;
            hScrollBar.Scroll += HScrollBar_Scroll;
            panelWaveform.Controls.Add(hScrollBar);
        }

        private void InitializePlaybackControls()
        {
            playPauseButton.Text = "Play";
            playbackTimer = new Timer();
            playbackTimer.Interval = 100; // Update every 100ms
            playbackTimer.Tick += PlaybackTimer_Tick;
        }

        private void PlayPauseButton_Click(object sender, EventArgs e)
        {
            if (outputDevice == null)
            {
                if (audioFile != null)
                {
                    outputDevice = new WaveOutEvent();
                    outputDevice.Init(audioFile);
                    outputDevice.Play();
                    playPauseButton.Text = "Pause";
                    playbackTimer.Start();
                }
            }
            else if (outputDevice.PlaybackState == PlaybackState.Playing)
            {
                outputDevice.Pause();
                playPauseButton.Text = "Play";
                playbackTimer.Stop();
            }
            else if (outputDevice.PlaybackState == PlaybackState.Paused)
            {
                outputDevice.Play();
                playPauseButton.Text = "Pause";
                playbackTimer.Start();
            }
        }

        private void PlaybackTimer_Tick(object sender, EventArgs e)
        {
            if (audioFile != null)
            {
                TimeSpan currentTime = audioFile.CurrentTime;
                timeLabel.Text = $"{currentTime.Minutes:D2}:{currentTime.Seconds:D2}:{currentTime.Milliseconds:D3}";
                panelWaveform.Invalidate(); // Redraw to update playback indicator
            }
        }

        private void HScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            panelWaveform.Invalidate();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private async void LoadAudioMenuItem_Click(object sender, EventArgs e)
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
                        await LoadAudioFileAsync(filePath);
                        UpdateUIAfterAudioLoad();
                        MessageBox.Show("音頻文件已成功載入", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"載入音頻文件時發生錯誤：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private async Task LoadAudioFileAsync(string filePath)
        {
            await Task.Run(() =>
            {
                if (audioFile != null)
                {
                    audioFile.Dispose();
                }
                audioFile = new AudioFileReader(filePath);

                waveformSamples = new List<float>();
                sampleRate = audioFile.WaveFormat.SampleRate;
                var buffer = new float[sampleRate * audioFile.WaveFormat.Channels];
                int read;
                while ((read = audioFile.Read(buffer, 0, buffer.Length)) > 0)
                {
                    waveformSamples.AddRange(buffer.Take(read));
                }

                audioFile.Position = 0; // Reset position for playback
                CreateFullResolutionWaveform();
            });
        }

        private void CreateFullResolutionWaveform()
        {
            fullResolutionWaveform = new List<PointF>();
            float maxValue = waveformSamples.Max(Math.Abs);
            for (int i = 0; i < waveformSamples.Count; i++)
            {
                fullResolutionWaveform.Add(new PointF(i, waveformSamples[i] / maxValue));
            }
        }

        private async void UpdateUIAfterAudioLoad()
        {
            await CreateWaveformBitmapAsync();
            playPauseButton.Enabled = true;
            panelWaveform.Invalidate();
        }

        private async Task CreateWaveformBitmapAsync()
        {
            await Task.Run(() =>
            {
                var downsampledPoints = DownsampleWaveform((int)(panelWaveform.Width * zoomFactor));
                var bitmap = new Bitmap((int)(panelWaveform.Width * zoomFactor), panelWaveform.Height);
                using (var g = Graphics.FromImage(bitmap))
                {
                    g.Clear(Color.White);
                    DrawWaveform(g, downsampledPoints);
                }

                this.Invoke((MethodInvoker)delegate {
                    if (waveformBitmap != null)
                    {
                        waveformBitmap.Dispose();
                    }
                    waveformBitmap = bitmap;
                    UpdateScrollBar();
                });
            });
        }

        private void UpdateScrollBar()
        {
            if (waveformBitmap.Width > panelWaveform.Width)
            {
                hScrollBar.Visible = true;
                hScrollBar.Maximum = waveformBitmap.Width - panelWaveform.Width;
                hScrollBar.LargeChange = panelWaveform.Width;
                hScrollBar.SmallChange = panelWaveform.Width / 10;
            }
            else
            {
                hScrollBar.Visible = false;
            }
        }

        private List<PointF> DownsampleWaveform(int targetPointCount)
        {
            var result = new List<PointF>();
            float step = (float)fullResolutionWaveform.Count / targetPointCount;

            for (float i = 0; i < fullResolutionWaveform.Count; i += step)
            {
                int startIndex = (int)i;
                int endIndex = Math.Min(startIndex + (int)step, fullResolutionWaveform.Count);
                float maxValue = float.MinValue;
                float minValue = float.MaxValue;

                for (int j = startIndex; j < endIndex; j++)
                {
                    maxValue = Math.Max(maxValue, fullResolutionWaveform[j].Y);
                    minValue = Math.Min(minValue, fullResolutionWaveform[j].Y);
                }

                result.Add(new PointF(i, maxValue));
                result.Add(new PointF(i, minValue));
            }

            return result;
        }

        private void DrawWaveform(Graphics g, List<PointF> points)
        {
            var midHeight = panelWaveform.Height / 2;
            var widthScale = (float)panelWaveform.Width * zoomFactor / fullResolutionWaveform.Count;

            using (var path = new GraphicsPath())
            {
                for (int i = 0; i < points.Count; i += 2)
                {
                    float x = points[i].X * widthScale;
                    float y1 = midHeight - (points[i].Y * midHeight);
                    float y2 = midHeight - (points[i + 1].Y * midHeight);
                    path.AddLine(x, y1, x, y2);
                }
                g.DrawPath(Pens.Blue, path);
            }
        }

        private void panelWaveform_Paint(object sender, PaintEventArgs e)
        {
            if (waveformSamples == null || waveformSamples.Count == 0 || waveformBitmap == null)
            {
                return;
            }

            int sourceX = hScrollBar.Visible ? hScrollBar.Value : 0;
            e.Graphics.DrawImage(waveformBitmap, new Rectangle(0, 0, panelWaveform.Width, panelWaveform.Height),
                                 new Rectangle(sourceX, 0, panelWaveform.Width, waveformBitmap.Height),
                                 GraphicsUnit.Pixel);

            // 繪製時間軸
            var timePen = new Pen(Color.Gray);
            var widthScale = (float)waveformBitmap.Width / waveformSamples.Count;
            for (int i = 0; i < waveformBitmap.Width; i += 50)
            {
                var x = i - sourceX;
                if (x >= 0 && x < panelWaveform.Width)
                {
                    var time = (i / widthScale) / sampleRate;
                    e.Graphics.DrawLine(timePen, x, 0, x, panelWaveform.Height);
                    e.Graphics.DrawString(time.ToString("0.0") + "s", this.Font, Brushes.Black, x, 0);
                }
            }

            // 繪製Key
            var keyPen = new Pen(Color.Red);
            foreach (var key in keys)
            {
                var x = key.Key * widthScale - sourceX;
                if (x >= 0 && x < panelWaveform.Width)
                {
                    e.Graphics.DrawLine(keyPen, x, 0, x, panelWaveform.Height);
                    e.Graphics.DrawString(key.Value, this.Font, Brushes.Red, x, panelWaveform.Height / 2);
                }
            }

            // 繪製播放指標
            if (audioFile != null)
            {
                float playbackPosition = (float)audioFile.Position / audioFile.Length * waveformBitmap.Width;
                float x = playbackPosition - sourceX;
                if (x >= 0 && x < panelWaveform.Width)
                {
                    e.Graphics.DrawLine(new Pen(Color.Green, 2), x, 0, x, panelWaveform.Height);
                }
            }
        }

        public void panelWaveform_MouseClick(object sender, MouseEventArgs e)
        {
            if (waveformSamples == null || waveformSamples.Count == 0 || audioFile == null)
            {
                return;
            }

            float clickPosition = (e.X + hScrollBar.Value) / (float)waveformBitmap.Width;
            long newPosition = (long)(clickPosition * audioFile.Length);
            audioFile.Position = newPosition;

            // 更新播放位置
            //if (outputDevice != null && outputDevice.PlaybackState == PlaybackState.Playing)
            //{
                //outputDevice.Stop();
                //outputDevice.Play();
            //}

            // 添加Key
            var time = clickPosition * waveformSamples.Count / sampleRate;
            keys.Add(new KeyValuePair<int, string>((int)(time * sampleRate), "Key"));

            panelWaveform.Invalidate();
        }

        private void ZoomIn()
        {
            zoomFactor *= 1.2f;
            UpdateWaveform();
        }

        private void ZoomOut()
        {
            zoomFactor /= 1.2f;
            if (zoomFactor < 1.0f) zoomFactor = 1.0f;
            UpdateWaveform();
        }

        private async void UpdateWaveform()
        {
            await CreateWaveformBitmapAsync();
            panelWaveform.Invalidate();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.Add))
            {
                ZoomIn();
                return true;
            }
            else if (keyData == (Keys.Control | Keys.Subtract))
            {
                ZoomOut();
                return true;
            }
            else if (keyData == Keys.Space)
            {
                PlayPauseButton_Click(this, EventArgs.Empty);
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            outputDevice?.Dispose();
            audioFile?.Dispose();
        }

    }
}