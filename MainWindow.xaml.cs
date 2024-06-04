using System;
using System.Windows;
using VideoPlayerApplication.Service;
using VideoPlayerApplication.ViewModel;

namespace VideoPlayerApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {           
            InitializeComponent();
            MainWindowViewModel? vm = DataContext as MainWindowViewModel;
            vm.DialogService = new FileDialogService();
            vm.VideoCollectionService = new VideoCollectionService();
            vm.ShowDialog = new ShowWindowDialog();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainWindowViewModel vm)
            {
                vm.LoadSettings();
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Close();
            if (DataContext is MainWindowViewModel vm)
            {
                vm.SaveSettings();
            }
            MediaPlayerSettings.SaveMediaPlayerSettings();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateLayout();
        }
    }
}
