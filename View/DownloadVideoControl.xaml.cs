using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using VideoPlayerApplication.ViewModel;

namespace VideoPlayerApplication.View
{
    /// <summary>
    /// Interaction logic for DownloadVideoControl.xaml
    /// </summary>
    public partial class DownloadVideoControl : Window
    {
        public DownloadVideoControl()
        {
            InitializeComponent();
            DownloadVideoControlViewModel obj = new DownloadVideoControlViewModel();
            obj.RequestClose += Obj_RequestClose;
            DataContext = obj;
        }

        private void Obj_RequestClose()
        {
            DialogResult = true;
            Close();
        }

        private void DownloadDialog_Closed(object sender, EventArgs e)
        {

        }
    }
}
