using System.ComponentModel;

namespace MVVMExample.ViewModels
{
    public class OnPropertyChangedBase
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
