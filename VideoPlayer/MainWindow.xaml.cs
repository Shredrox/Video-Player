using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace VideoPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool sliderDrag = false;

        public MainWindow()
        {
            InitializeComponent();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if ((VideoPlayer.Source != null) && VideoPlayer.NaturalDuration.HasTimeSpan && (!sliderDrag))
            {
                VideoDuration.Minimum = 0;
                VideoDuration.Maximum = VideoPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                VideoDuration.Value = VideoPlayer.Position.TotalSeconds;
            }
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            VideoPlayer.Pause();
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            VideoPlayer.Play();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            VideoPlayer.Stop();
        }

        private void VideoDuration_MouseDown(object sender, MouseButtonEventArgs e)
        {
            int SliderValue = (int)VideoDuration.Value;
            TimeSpan ts = new TimeSpan(0, 0, 0, 0, SliderValue);
            VideoPlayer.Position = ts;
        }

        private void VideoDuration_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            sliderDrag = true;
        }

        private void VideoDuration_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            sliderDrag = false;
            VideoPlayer.Position = TimeSpan.FromSeconds(VideoDuration.Value);
        }

        private void VideoDuration_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            VideoDurationText.Text = TimeSpan.FromSeconds(VideoDuration.Value).ToString(@"mm\:ss");
        }

        private void LoadVideoFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();

            if(fileDialog.ShowDialog() == true)
            {
                if (fileDialog.FileName.Contains("file://"))
                {
                    string[] pathSplit = fileDialog.FileName.Split(new string[] { "///" }, StringSplitOptions.None);
                    fileDialog.FileName = pathSplit[1];
                }

                VideoPlayer.Source = new Uri(fileDialog.FileName, UriKind.RelativeOrAbsolute);
            }
        }
    }
}
