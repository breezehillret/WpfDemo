using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Shapes;


namespace MVVMExample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isDragging;
        private Point startPoint;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ViewModels.MainViewModel();
        }

        ///// <summary>
        /// RadioButton Checked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void RadioButton_Checked(object sender, RoutedEventArgs e)
        //{
        //    RadioButton radioButton = sender as RadioButton;

        //    if (radioButton != null && radioButton.IsChecked == true)
        //    {
        //        RadioButtonLabel.Content = radioButton.Content;
        //        var color = RadioButtonLabel.Content.ToString();
        //        switch (color.ToLower())
        //        {
        //            case "red":
        //                RadioButtonLabel.Foreground = System.Windows.Media.Brushes.Red;
        //                RadioButtonLabel.Background = System.Windows.Media.Brushes.Cyan;
        //                break;
        //            case "green":
        //                RadioButtonLabel.Foreground = System.Windows.Media.Brushes.Green;
        //                RadioButtonLabel.Background = System.Windows.Media.Brushes.LightGray;
        //                break;
        //            case "blue":
        //                RadioButtonLabel.Foreground = System.Windows.Media.Brushes.Blue;
        //                RadioButtonLabel.Background = System.Windows.Media.Brushes.Yellow;
        //                break;
        //            default:
        //                break;
        //        }
        //        RadioButtonLabel.Background = System.Windows.Media.Brushes.White;
        //    }
        //}
        private void Car_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var car = sender as Path;
            var canvas = car.Parent as Canvas;
            if (car != null && canvas != null)
            {
                isDragging = true;
                startPoint = e.GetPosition(canvas);
                var carPosition = e.GetPosition(car);
                startPoint.X -= carPosition.X; // Adjust startPoint to be relative to car
                startPoint.Y -= carPosition.Y; // Adjust startPoint to be relative to car
                car.CaptureMouse();
            }
        }
        private void Car_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            var car = sender as Path;
            if (car != null && car.Parent is Canvas canvas && isDragging)
            {
                var currentPosition = e.GetPosition(canvas);
                var offset = currentPosition - startPoint;
                Canvas.SetLeft(car, Canvas.GetLeft(car) + offset.X);
                Canvas.SetTop(car, Canvas.GetTop(car) + offset.Y);
                startPoint = currentPosition;
            }
        }
        private void Car_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var car = sender as Path;
            if (car != null)
            {
                isDragging = false;
                car.ReleaseMouseCapture();
            }
        }
    }
}