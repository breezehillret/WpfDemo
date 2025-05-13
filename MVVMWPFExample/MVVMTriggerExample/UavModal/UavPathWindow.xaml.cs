using System.Windows;
using MVVMExample.ViewModels;

namespace MVVMExample.UavModal
{
    public partial class UavPathModal : Window
    {
        public UavPathModal()
        {
            InitializeComponent();
            
            DataContext = new UavPathViewModel();
        }

        // Optional: Handle SizeChanged event to update canvas size
        private void Canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (DataContext is UavPathViewModel vm)
            {
                vm.UpdateCanvasSize(e.NewSize.Width, e.NewSize.Height);
            }
        }
    }
}
