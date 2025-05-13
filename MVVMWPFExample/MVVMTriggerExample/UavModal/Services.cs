namespace MVVMExample.UavModal
{
    public class WindowService : IWindowService
    {
        public void ShowUavPathModal()
        {
            var window = new UavPathModal();
            window.ShowDialog();
        }
    }
}