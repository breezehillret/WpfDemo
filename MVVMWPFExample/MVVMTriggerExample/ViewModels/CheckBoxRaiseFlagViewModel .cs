﻿using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace MVVMExample.ViewModels
{
    public class CheckBoxRaiseFlagViewModel : OnPropertyChangedBase, INotifyPropertyChanged
    {
        #region Constant Strings
        private const string Anthem = "C:\\Users\\Breez\\source\\repos\\GitHub\\WpfDemo\\MVVMWPFExample\\MVVMTriggerExample\\Star Spangled Banner, National Anthem - Instrumental 01.wav";
        private const string Flinstones = "C:\\Users\\Breez\\source\\repos\\GitHub\\WpfDemo\\MVVMWPFExample\\MVVMTriggerExample\\OTc3NjYzODY5Nzc3Njg_4wx79u4n42Q.mp3";

        #endregion Constant Strings

        #region Properties
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
                    //PlaySoundAsync();
                }
            }
        }

        #endregion Properties

        #region Example of Override
        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName); 
        }

        #endregion Example of Override

        #region Media
        private async Task PlaySoundAsync()
        {
            try
            {
                string filePath = (_isFlagRaised) ? Anthem : Flinstones;

                // Initialize MediaPlayer on the UI thread
                MediaPlayer mediaPlayer = await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    MediaPlayer player = new MediaPlayer();
                    player.Open(new Uri(filePath));
                    return player;
                });

                mediaPlayer.Play();

                // Wait for 8 seconds before stopping the sound
                await Task.Delay(8000);

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

        #endregion Media
    }
}
