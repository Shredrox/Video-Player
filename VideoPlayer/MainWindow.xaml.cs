using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
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

            DispatcherTimer uiTimer = new DispatcherTimer();
            uiTimer.Interval = TimeSpan.FromSeconds(3);
            uiTimer.Tick += UItimer_Tick;
            uiTimer.Start();
        }

        void timer_Tick(object? sender, EventArgs e)
        {
            if ((VideoPlayer.Source != null) && VideoPlayer.NaturalDuration.HasTimeSpan && (!sliderDrag))
            {
                VideoDuration.Minimum = 0;
                VideoDuration.Maximum = VideoPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                VideoDuration.Value = VideoPlayer.Position.TotalSeconds;
            }
        }

        void UItimer_Tick(object? sender, EventArgs e)
        {
            if (isVideoFullscreen && DurationGrid.Visibility == Visibility.Visible && ControlsPanel.Visibility == Visibility.Visible)
            {
                DurationGrid.Visibility = Visibility.Hidden;
                ControlsPanel.Visibility = Visibility.Hidden;
            }
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if(PlayPauseIcon.Kind == MaterialDesignThemes.Wpf.PackIconKind.PlayCircle)
            {
                VideoPlayer.Play();
                PlayPauseIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.PauseCircle;
            }
            else if(PlayPauseIcon.Kind == MaterialDesignThemes.Wpf.PackIconKind.PauseCircle)
            {
                VideoPlayer.Pause();
                PlayPauseIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.PlayCircle;
            }
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

        private bool isVideoFullscreen = false;
        private void FullscreenButton_Click(object sender, RoutedEventArgs e)
        {
            if (FullscreenIcon.Kind == MaterialDesignThemes.Wpf.PackIconKind.Fullscreen)
            {
                Grid.SetRowSpan(VideoPlayer, 3);
                MainGrid.Margin = new Thickness(0, 0, 0, 0);
                isVideoFullscreen = true;
                DurationGrid.Visibility = Visibility.Hidden;
                ControlsPanel.Visibility = Visibility.Hidden;
                FullscreenIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.FullscreenExit;
            }
            else if (FullscreenIcon.Kind == MaterialDesignThemes.Wpf.PackIconKind.FullscreenExit)
            {
                Grid.SetRowSpan(VideoPlayer, 1);
                isVideoFullscreen = false;
                MainGrid.Margin = new Thickness(10, 10, 10, 10);
                FullscreenIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Fullscreen;
            }
        }

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            VideoPlayer.Volume = (double)VolumeSlider.Value;
        }

        private bool isFullscreen = false;
        private void VideoPlayer_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2 && !isFullscreen)
            {
                this.WindowStyle = WindowStyle.None;
                this.WindowState = WindowState.Maximized;
                isFullscreen = true;
            }
            else if (e.ClickCount == 2 && isFullscreen) 
            {
                this.WindowStyle = WindowStyle.SingleBorderWindow;
                this.WindowState = WindowState.Normal;
                isFullscreen = false;
            }
        }

        private void VideoPlayer_MouseMove(object sender, MouseEventArgs e)
        {
            if (isVideoFullscreen)
            {
                DurationGrid.Visibility = Visibility.Visible;
                ControlsPanel.Visibility = Visibility.Visible;
            }
        }
    }
}
