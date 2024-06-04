using System.Windows;

namespace VideoPlayerApplication.Service
{
    public interface IShowDialog
    {
        bool ShowDialog<T>() where T : Window, new();
    }
}
