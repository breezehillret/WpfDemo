using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;

namespace MVVMExample.Controllers
{
    public class FlagController : INotifyPropertyChanged
    {
        private bool _isFlagRaised;

        #region Constant Strings
        private const string Anthem = "C:\\Users\\Breez\\source\\repos\\GitHub\\WpfDemo\\MVVMWPFExample\\MVVMTriggerExample\\Star Spangled Banner, National Anthem - Instrumental 01.wav";
        private const string Flinstones = "C:\\Users\\Breez\\source\\repos\\GitHub\\WpfDemo\\MVVMWPFExample\\MVVMTriggerExample\\OTc3NjYzODY5Nzc3Njg_4wx79u4n42Q.mp3";
        #endregion

        public bool IsFlagRaised
        {
            get => _isFlagRaised;
            private set
            {
                if (_isFlagRaised != value)
                {
                    _isFlagRaised = value;
                    OnPropertyChanged(nameof(IsFlagRaised));
                    _ = PlaySoundAsync(value); // Fire and forget sound effect
                }
            }
        }

        public ICommand RaiseFlagCommand { get; }
        public ICommand LowerFlagCommand { get; }

        public FlagController()
        {
            RaiseFlagCommand = new RelayCommand(_ => ToggleFlag(true));
            LowerFlagCommand = new RelayCommand(_ => ToggleFlag(false));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void ToggleFlag(bool isRaised)
        {
            IsFlagRaised = isRaised; // Trigger property setter, which plays the sound
        }

        private async Task PlaySoundAsync(bool isRaised)
        {
            try
            {
                string filePath = isRaised ? Anthem : Flinstones;

                // Initialize MediaPlayer on the UI thread
                MediaPlayer mediaPlayer = await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    MediaPlayer player = new MediaPlayer();
                    player.Open(new Uri(filePath));
                    return player;
                });

                mediaPlayer.Play();

                // Wait for 9 seconds before stopping the sound
                await Task.Delay(9000);

                // Stop and close the MediaPlayer on the UI thread
                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    mediaPlayer.Stop();
                    mediaPlayer.Close();
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error playing sound: {ex.Message}");
            }
        }
    }
}
