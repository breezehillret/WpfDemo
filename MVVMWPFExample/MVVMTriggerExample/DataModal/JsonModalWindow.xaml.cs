using System.Windows;

namespace MVVMExample.DataModal
{
    /// <summary>
    /// Interaction logic for JsonModalWindow.xaml
    /// </summary>
    public partial class JsonModalWindow : Window
    {
        public JsonModalWindow()
        {
            InitializeComponent();
            DataContext = new JsonModalViewModel();
        }
    }
}
