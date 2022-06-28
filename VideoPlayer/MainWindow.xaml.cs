using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Threading;

namespace VideoPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
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
            if (VideoPlayer.Source != null)
            {
                if (VideoPlayer.NaturalDuration.HasTimeSpan)
                    lblStatus.Content = String.Format("{0} / {1}", VideoPlayer.Position.ToString(@"mm\:ss"), VideoPlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss"));
            }
            else
                lblStatus.Content = "No file selected...";
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
