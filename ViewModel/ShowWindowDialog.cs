using System.Windows;
using VideoPlayerApplication.Service;

namespace VideoPlayerApplication.ViewModel
{
    public class ShowWindowDialog : IShowDialog
    {
        public bool ShowDialog<T>() where T : Window, new()
        {
            var win = new T();
            return win.ShowDialog() == true;
        }
    }
}
