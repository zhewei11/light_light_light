using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using NAudio.Wave;
using Newtonsoft.Json;

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
        private string copiedKeyData = null;


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

            outputDevice = new WaveOutEvent();
            outputDevice.PlaybackStopped += OutputDevice_PlaybackStopped; // 订阅播放结束事件
        }
        private void OutputDevice_PlaybackStopped(object sender, StoppedEventArgs e)
        {
            playbackTimer.Stop();
            playPauseButton.Text = "Play";
            audioFile.Position = 0; // 重置播放位置
            UpdateTimeLabel();
            panelWaveform.Invalidate();
        }


        private void PlayPauseButton_Click(object sender, EventArgs e)
        {
            if (outputDevice == null)
            {
                outputDevice = new WaveOutEvent();
                outputDevice.PlaybackStopped += OutputDevice_PlaybackStopped; // 订阅播放结束事件
            }

            if (audioFile != null)
            {
                if (outputDevice.PlaybackState == PlaybackState.Stopped || outputDevice.PlaybackState == PlaybackState.Paused)
                {
                    outputDevice.Init(audioFile);
                    outputDevice.Play();
                    playPauseButton.Text = "Pause";
                    playbackTimer.Start();
                }
                else if (outputDevice.PlaybackState == PlaybackState.Playing)
                {
                    outputDevice.Pause();
                    playPauseButton.Text = "Play";
                    playbackTimer.Stop();
                }
            }
        }


        private void PlaybackTimer_Tick(object sender, EventArgs e)
        {
            if (audioFile != null)
            {
                UpdateTimeLabel();
                panelWaveform.Invalidate(); // Redraw to update playback indicator
            }
        }
        private void UpdateTimeLabel()
        {
            if (audioFile != null)
            {
                TimeSpan currentTime = audioFile.CurrentTime;
                timeLabel.Text = $"{currentTime.Minutes:D2}:{currentTime.Seconds:D2}:{currentTime.Milliseconds:D3}";
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
            var widthScale = (float)waveformBitmap.Width / (float)audioFile.TotalTime.TotalMilliseconds;

            for (double ms = 0; ms < audioFile.TotalTime.TotalMilliseconds; ms += 1000) // 每1秒畫一條線
            {
                float x = (float)(ms * widthScale) - sourceX;
                if (x >= 0 && x < panelWaveform.Width)
                {
                    e.Graphics.DrawLine(timePen, x, 0, x, panelWaveform.Height);
                    e.Graphics.DrawString(TimeSpan.FromMilliseconds(ms).ToString(@"mm\:ss"), this.Font, Brushes.Black, x, 0);
                }
            }

            // 繪製Key，只顯示模式
            var keyPen = new Pen(Color.Red);
            foreach (var key in keys)
            {
                var keyData = JsonConvert.DeserializeObject<dynamic>(key.Value);
                string mode = keyData.mode;
                float x = (float)(key.Key * widthScale) - sourceX;
                if (x >= 0 && x < panelWaveform.Width)
                {
                    e.Graphics.DrawLine(keyPen, x, 0, x, panelWaveform.Height);
                    e.Graphics.DrawString(mode, this.Font, Brushes.Red, x, panelWaveform.Height / 2);
                }
            }

            // 繪製播放指標
            if (audioFile != null)
            {
                float playbackPosition = (float)(audioFile.CurrentTime.TotalMilliseconds * widthScale) - sourceX;
                if (playbackPosition >= 0 && playbackPosition < panelWaveform.Width)
                {
                    e.Graphics.DrawLine(new Pen(Color.Green, 2), playbackPosition, 0, playbackPosition, panelWaveform.Height);
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
            UpdateTimeLabel();
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

        private void btn_create_key_Click(object sender, EventArgs e)
        {
            try
            {
                // 獲取當前播放指標位置的時間（毫秒）
                int timePosition = (int)(audioFile.CurrentTime.TotalMilliseconds);

                // 獲取選定的模式
                string mode = comboBox_Mode.SelectedItem.ToString();

                // 創建新Key對象
                var newKey = new
                {
                    mode = "MODES_" + mode,
                    start_time = timePosition,
                    duration = 0,
                    XH = new
                    {
                        func = comboBox_XH_func.SelectedIndex,
                        range = int.Parse(txt_XH_Range.Text),
                        lower = int.Parse(txt_XH_Lower.Text),
                        p1 = int.Parse(txt_XH_p1.Text),
                        p2 = int.Parse(txt_XH_p2.Text)
                    },
                    XS = new
                    {
                        func = comboBox_XS_func.SelectedIndex,
                        range = int.Parse(txt_XS_Range.Text),
                        lower = int.Parse(txt_XS_Lower.Text),
                        p1 = int.Parse(txt_XS_p1.Text),
                        p2 = int.Parse(txt_XS_p2.Text)
                    },
                    XV = new
                    {
                        func = comboBox_XV_func.SelectedIndex,
                        range = int.Parse(txt_XV_Range.Text),
                        lower = int.Parse(txt_XV_Lower.Text),
                        p1 = int.Parse(txt_XV_p1.Text),
                        p2 = int.Parse(txt_XV_p2.Text)
                    },
                    YH = new
                    {
                        func = comboBox_YH_func.SelectedIndex,
                        range = int.Parse(txt_YH_Range.Text),
                        lower = int.Parse(txt_YH_Lower.Text),
                        p1 = int.Parse(txt_YH_p1.Text),
                        p2 = int.Parse(txt_YH_p2.Text)
                    },
                    YS = new
                    {
                        func = comboBox_YS_func.SelectedIndex,
                        range = int.Parse(txt_YS_Range.Text),
                        lower = int.Parse(txt_YS_Lower.Text),
                        p1 = int.Parse(txt_YS_p1.Text),
                        p2 = int.Parse(txt_YS_p2.Text)
                    },
                    YV = new
                    {
                        func = comboBox_YV_func.SelectedIndex,
                        range = int.Parse(txt_YV_Range.Text),
                        lower = int.Parse(txt_YV_Lower.Text),
                        p1 = int.Parse(txt_YV_p1.Text),
                        p2 = int.Parse(txt_YV_p2.Text)
                    },
                    p1 = int.Parse(txt_p1.Text),
                    p2 = int.Parse(txt_p2.Text),
                    p3 = int.Parse(txt_p3.Text),
                    p4 = int.Parse(txt_p4.Text)
                };

                // 添加到keys列表
                keys.Add(new KeyValuePair<int, string>(timePosition, JsonConvert.SerializeObject(newKey)));
                UpdateListBoxEffect();
                panelWaveform.Invalidate();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating key: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private async void LoadFile_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "JSON文件|*.json|所有文件|*.*";
                openFileDialog.Title = "選擇JSON文件";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    try
                    {
                        // 使用Task.Run來異步讀取文件內容
                        string jsonContent = await Task.Run(() => File.ReadAllText(filePath));
                        var loadedKeys = JsonConvert.DeserializeObject<List<dynamic>>(jsonContent);

                        keys.Clear();
                        foreach (var key in loadedKeys)
                        {
                            int startTime = key.start_time;
                            string serializedKey = JsonConvert.SerializeObject(key);
                            keys.Add(new KeyValuePair<int, string>(startTime, serializedKey));
                        }

                        UpdateListBoxEffect();
                        panelWaveform.Invalidate();
                        MessageBox.Show("JSON文件已成功載入", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"載入JSON文件時發生錯誤：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void UpdateListBoxEffect()
        {
            listBox_effect.Items.Clear();

            var sortedKeys = keys
                .Select(k => new { Time = k.Key, Data = JsonConvert.DeserializeObject<dynamic>(k.Value) })
                .OrderBy(k => k.Time)
                .ToList();

            foreach (var key in sortedKeys)
            {
                listBox_effect.Items.Add($"{((String)key.Data.mode).Substring(6)}, Time: {key.Time}ms");
            }
        }
        private async void Export_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "JSON文件|*.json|所有文件|*.*";
                saveFileDialog.Title = "匯出JSON文件";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveFileDialog.FileName;
                    try
                    {
                        var sortedKeys = keys.OrderBy(k => k.Key).ToList();

                        for (int i = 0; i < sortedKeys.Count; i++)
                        {
                            var keyData = JsonConvert.DeserializeObject<dynamic>(sortedKeys[i].Value);
                            if (i < sortedKeys.Count - 1)
                            {
                                keyData.duration = sortedKeys[i + 1].Key - sortedKeys[i].Key;
                            }
                            else
                            {
                                keyData.duration = (int)audioFile.TotalTime.TotalMilliseconds - sortedKeys[i].Key;
                            }
                            sortedKeys[i] = new KeyValuePair<int, string>(sortedKeys[i].Key, JsonConvert.SerializeObject(keyData));
                        }

                        var keysToExport = sortedKeys.Select(k => JsonConvert.DeserializeObject<dynamic>(k.Value)).ToList();
                        string jsonContent = JsonConvert.SerializeObject(keysToExport, Formatting.Indented);

                        // 使用Task.Run來異步寫入文件內容
                        await Task.Run(() => File.WriteAllText(filePath, jsonContent));
                        MessageBox.Show("JSON文件已成功匯出", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"匯出JSON文件時發生錯誤：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void ListBox_effect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox_effect.SelectedIndex == -1) return;

            var selectedItem = listBox_effect.SelectedItem.ToString();
            var selectedKeyTime = int.Parse(selectedItem.Split(',')[1].Split(':')[1].Replace("ms", "").Trim());

            var selectedKey = keys.FirstOrDefault(k => k.Key == selectedKeyTime);
            if (selectedKey.Equals(default(KeyValuePair<int, string>))) return;

            var keyData = JsonConvert.DeserializeObject<dynamic>(selectedKey.Value);

            comboBox_Mode.SelectedItem = ((string)keyData.mode).Substring(6);
            txt_XH_Range.Text = ((int)keyData.XH.range).ToString();
            txt_XH_Lower.Text = ((int)keyData.XH.lower).ToString();
            txt_XH_p1.Text = ((int)keyData.XH.p1).ToString();
            txt_XH_p2.Text = ((int)keyData.XH.p2).ToString();
            comboBox_XH_func.SelectedIndex = (int)keyData.XH.func;

            txt_XS_Range.Text = ((int)keyData.XS.range).ToString();
            txt_XS_Lower.Text = ((int)keyData.XS.lower).ToString();
            txt_XS_p1.Text = ((int)keyData.XS.p1).ToString();
            txt_XS_p2.Text = ((int)keyData.XS.p2).ToString();
            comboBox_XS_func.SelectedIndex = (int)keyData.XS.func;

            txt_XV_Range.Text = ((int)keyData.XV.range).ToString();
            txt_XV_Lower.Text = ((int)keyData.XV.lower).ToString();
            txt_XV_p1.Text = ((int)keyData.XV.p1).ToString();
            txt_XV_p2.Text = ((int)keyData.XV.p2).ToString();
            comboBox_XV_func.SelectedIndex = (int)keyData.XV.func;

            txt_YH_Range.Text = ((int)keyData.YH.range).ToString();
            txt_YH_Lower.Text = ((int)keyData.YH.lower).ToString();
            txt_YH_p1.Text = ((int)keyData.YH.p1).ToString();
            txt_YH_p2.Text = ((int)keyData.YH.p2).ToString();
            comboBox_YH_func.SelectedIndex = (int)keyData.YH.func;

            txt_YS_Range.Text = ((int)keyData.YS.range).ToString();
            txt_YS_Lower.Text = ((int)keyData.YS.lower).ToString();
            txt_YS_p1.Text = ((int)keyData.YS.p1).ToString();
            txt_YS_p2.Text = ((int)keyData.YS.p2).ToString();
            comboBox_YS_func.SelectedIndex = (int)keyData.YS.func;

            txt_YV_Range.Text = ((int)keyData.YV.range).ToString();
            txt_YV_Lower.Text = ((int)keyData.YV.lower).ToString();
            txt_YV_p1.Text = ((int)keyData.YV.p1).ToString();
            txt_YV_p2.Text = ((int)keyData.YV.p2).ToString();
            comboBox_YV_func.SelectedIndex = (int)keyData.YV.func;

            txt_p1.Text = ((int)keyData.p1).ToString();
            txt_p2.Text = ((int)keyData.p2).ToString();
            txt_p3.Text = ((int)keyData.p3).ToString();
            txt_p4.Text = ((int)keyData.p4).ToString();

            // 計算文件位置
            long bytePosition = (long)((double)selectedKeyTime / 1000 * audioFile.WaveFormat.SampleRate * audioFile.WaveFormat.Channels * audioFile.WaveFormat.BitsPerSample / 8);
            bytePosition = Math.Max(0, Math.Min(bytePosition, audioFile.Length)); // 確保位置在有效範圍內

            audioFile.Position = bytePosition;
            UpdateTimeLabel();
            panelWaveform.Invalidate();
        }
        private void CopyKeyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var currentPosition = (int)(audioFile.CurrentTime.TotalMilliseconds);
            var currentKey = keys.FirstOrDefault(k => k.Key == currentPosition);

            if (currentKey.Equals(default(KeyValuePair<int, string>)))
            {
                MessageBox.Show("當前時間沒有key", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            copiedKeyData = currentKey.Value;
            MessageBox.Show("key已複製", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void PasteKeyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (copiedKeyData == null)
            {
                MessageBox.Show("沒有複製的key", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var currentPosition = (int)(audioFile.CurrentTime.TotalMilliseconds);
            var existingKey = keys.FirstOrDefault(k => k.Key == currentPosition);

            if (!existingKey.Equals(default(KeyValuePair<int, string>)))
            {
                var result = MessageBox.Show("當前時間已經有key，是否覆寫?", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.No)
                {
                    return;
                }
                keys.Remove(existingKey);
            }

            var keyData = JsonConvert.DeserializeObject<dynamic>(copiedKeyData);
            keyData.start_time = currentPosition;
            keys.Add(new KeyValuePair<int, string>(currentPosition, JsonConvert.SerializeObject(keyData)));

            UpdateListBoxEffect();
            panelWaveform.Invalidate();
            MessageBox.Show("key已貼上", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void DeleteKeyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var currentPosition = (int)(audioFile.CurrentTime.TotalMilliseconds);
            var existingKey = keys.FirstOrDefault(k => k.Key == currentPosition);

            if (existingKey.Equals(default(KeyValuePair<int, string>)))
            {
                MessageBox.Show("當前時間沒有key", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            keys.Remove(existingKey);
            UpdateListBoxEffect();
            panelWaveform.Invalidate();
            MessageBox.Show("key已刪除", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                CopyKeyToolStripMenuItem_Click(this, new EventArgs());
            }
            else if (e.Control && e.KeyCode == Keys.V)
            {
                PasteKeyToolStripMenuItem_Click(this, new EventArgs());
            }
            else if (e.KeyCode == Keys.Delete)
            {
                DeleteKeyToolStripMenuItem_Click(this, new EventArgs());
            }
        }

    }
}
