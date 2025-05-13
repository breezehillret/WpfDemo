using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace MVVMExample.ViewModels
{
    public class UavPathViewModel : INotifyPropertyChanged
    {
        public ICommand MoveLeftCommand { get; }
        public ICommand MoveRightCommand { get; }
        public ICommand MoveForwardCommand { get; }
        public ICommand MoveBackwardCommand { get; }
        public ICommand ResetPositionCommand { get; }
        public ICommand LoiterCommand { get; }

        private double _x;
        private double _y;
        private double _canvasWidth = 600;
        private double _canvasHeight = 400;
        private string _currentDirection = "Right";

        private readonly DispatcherTimer _timer;
        private PointCollection _trailPoints;

        public UavPathViewModel()
        {
            MoveLeftCommand = new RelayCommand(_ => SetDirection("Left"));
            MoveRightCommand = new RelayCommand(_ => SetDirection("Right"));
            MoveForwardCommand = new RelayCommand(_ => SetDirection("Forward"));
            MoveBackwardCommand = new RelayCommand(_ => SetDirection("Backward"));
            ResetPositionCommand = new RelayCommand(_ => ResetPosition());
            LoiterCommand = new RelayCommand(_ => SetDirection("Loiter"));

            _x = _canvasWidth / 2;
            _y = _canvasHeight / 2;

            TrailPoints = new PointCollection
            {
                new Point(_x + 10, _y + 10) // center of UAV
            };

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(100)
            };
            _timer.Tick += OnTimerTick;
            _timer.Start();
        }

        public double UavPositionX
        {
            get => _x;
            set
            {
                _x = value;
                OnPropertyChanged();
            }
        }

        public double UavPositionY
        {
            get => _y;
            set
            {
                _y = value;
                OnPropertyChanged();
            }
        }

        public PointCollection TrailPoints
        {
            get => _trailPoints;
            private set
            {
                _trailPoints = value;
                OnPropertyChanged();
            }
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            MoveUav();
        }

        private void MoveUav()
        {
            const double step = 5;

            switch (_currentDirection)
            {
                case "Left": _x -= step; break;
                case "Right": _x += step; break;
                case "Forward": _y -= step; break;
                case "Backward": _y += step; break;
                case "Loiter": return; // Do nothing
            }

            _x = Math.Max(0, Math.Min(_canvasWidth - 20, _x));
            _y = Math.Max(0, Math.Min(_canvasHeight - 20, _y));

            UavPositionX = _x;
            UavPositionY = _y;

            var newPoints = new PointCollection(TrailPoints)
            {
                new Point(_x + 10, _y + 10)
            };

            TrailPoints = newPoints;
        }

        private void SetDirection(string direction)
        {
            _currentDirection = direction;
        }

        private void ResetPosition()
        {
            _x = _canvasWidth / 2;
            _y = _canvasHeight / 2;

            UavPositionX = _x;
            UavPositionY = _y;

            TrailPoints = new PointCollection
            {
                new Point(_x + 10, _y + 10)
            };
        }

        public void UpdateCanvasSize(double width, double height)
        {
            _canvasWidth = width;
            _canvasHeight = height;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
