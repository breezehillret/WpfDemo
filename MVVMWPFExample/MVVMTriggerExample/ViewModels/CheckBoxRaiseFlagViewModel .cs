using System.ComponentModel;
using System.Media;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace MVVMExample.ViewModels
{
    public class CheckBoxRaiseFlagViewModel : INotifyPropertyChanged
    {
        private bool _isFlagRaised;

        public bool IsFlagRaised
        {
            get { return _isFlagRaised; }
            set
            {
                if (_isFlagRaised != value)
                {
                    _isFlagRaised = value;
                    OnPropertyChanged();
                    PlaySound();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void PlaySound()
        {
            try
            {
                string filePath = (_isFlagRaised) ?
                    "C:\\Users\\Breez\\source\\repos\\MVVMWPFExample\\MVVMTriggerExample\\Star Spangled Banner, National Anthem - Instrumental 01.wav" :
                    "C:\\Users\\Breez\\source\\repos\\MVVMWPFExample\\MVVMTriggerExample\\OTc3NjYzODY5Nzc3Njg_4wx79u4n42Q.mp3";

                MediaPlayer mediaPlayer = new MediaPlayer();
                mediaPlayer.Open(new Uri(filePath));
                mediaPlayer.Play();

                System.Timers.Timer timer = new System.Timers.Timer();
                timer.Interval = 9500;
                timer.Elapsed += (sender, e) =>
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        mediaPlayer.Stop();
                        timer.Stop();
                        timer.Dispose();
                    });
                };
                timer.Start();
            }
           catch (Exception ex)
            {
                Console.WriteLine($"Error playing sound: {ex.Message}");
            }
        }
    }
}
