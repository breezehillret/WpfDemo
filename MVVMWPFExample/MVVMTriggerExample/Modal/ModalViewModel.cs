using GalaSoft.MvvmLight;
using MVVMExample.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Management.Automation;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace MVVMExample.Modal
{
    public class ModalViewModel : OnPropertyChangedBase, INotifyPropertyChanged
    {
        #region privates
        private readonly Window _ownerWindow;
        private Storyboard _bounceAnimation;
        private Storyboard _bounceAnimationOpposite;
        #endregion

        #region public
        public string DisplayPsScript => PSSCRIPT;

        public const string PSSCRIPT = "Get-Process | Select-Object -Property Name, Id, CPU, WorkingSet, StartTime, Path";

        #endregion

        #region properties
        private ObservableCollection<string> _processInfo;
        public ObservableCollection<string> ProcessInfo
        {
            get { return _processInfo; }
            set
            {
                _processInfo = value;
                OnPropertyChanged(nameof(ProcessInfo));
            }
        }

        public ICommand BuildSolutionCommand { get; set; }
        public ICommand RunPowerShellCommand { get; set; }
        #endregion

        public Brush PenBrush { get; set; }

        public ModalViewModel(Window ownerWindow)
        {
            _ownerWindow = ownerWindow;
            InitializePenBrush();

            ProcessInfo = new ObservableCollection<string>();
            RunPowerShellCommand = new RelayCommand(RunPowerShellScript);
            BuildSolutionCommand = new RelayCommand(ExecuteBuildSolution);
        }

        private void InitializePenBrush()
        {
            PenBrush = new SolidColorBrush(Colors.Black);
        }

        public void ShowModal()
        {
            ModalWindow modalWindow = new ModalWindow
            {
                Owner = _ownerWindow
            };

            modalWindow.DataContext = this;

            // Register the Loaded event to initialize animations
            modalWindow.Loaded += ModalWindow_Loaded;

            // Show the modal dialog
            modalWindow.ShowDialog();
        }

        private void ModalWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var modalWindow = sender as ModalWindow;

            // Find and start animations when the modal window loads
            _bounceAnimation = (Storyboard)modalWindow.FindResource("BounceAnimation");
            _bounceAnimation.Begin(modalWindow, true);

            _bounceAnimationOpposite = (Storyboard)modalWindow.FindResource("BounceAnimationOpposite");
            _bounceAnimationOpposite.Begin(modalWindow, true);
        }

        private void RunPowerShellScript(object parameter)
        {
            ProcessInfo.Clear();

            // Create a PowerShell instance
            using (PowerShell powerShell = PowerShell.Create())
            {
                // Add a PowerShell script. For demonstration, we'll list all processes.
                powerShell.AddScript(PSSCRIPT);

                // Execute the PowerShell script and iterate over output objects
                foreach (PSObject result in powerShell.Invoke())
                {
                    // Append each line to ProcessInfo to show in UI
                    ProcessInfo.Add(result.ToString());
                }
            }
        }

        private async void ExecuteBuildSolution(object parameter)
        {
            var scriptPath = @"C:\Users\Breez\source\repos\GitHub\WpfDemo\MVVMWPFExample\MVVMTriggerExample\Modal\BuildSolution.ps1";
            var processStartInfo = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                Arguments = $"-ExecutionPolicy Bypass -File \"{scriptPath}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true, // Capture error output as well
                CreateNoWindow = true
            };

            try
            {
                ProcessInfo.Clear(); // Clear previous output

                var process = Process.Start(processStartInfo);
                if (process != null)
                {
                    // Asynchronously read output and error
                    process.OutputDataReceived += (sender, e) =>
                    {
                        if (!string.IsNullOrEmpty(e.Data))
                        {
                            // Update ProcessInfo on UI thread immediately for each new line of output
                            Application.Current.Dispatcher.Invoke(() => ProcessInfo.Add(e.Data));
                        }
                    };

                    process.ErrorDataReceived += (sender, e) =>
                    {
                        if (!string.IsNullOrEmpty(e.Data))
                        {
                            // Update ProcessInfo on UI thread for error output
                            Application.Current.Dispatcher.Invoke(() => ProcessInfo.Add($"Error: {e.Data}"));
                        }
                    };

                    // Start reading the output and error streams asynchronously
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();

                    // Wait for the process to exit
                    await Task.Run(() => process.WaitForExit());
                }
                else
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        ProcessInfo.Add("Failed to start the build process.");
                    });
                }
            }
            catch (Exception ex)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    ProcessInfo.Add($"Error: {ex.Message}");
                });
            }
        }
    }
}
