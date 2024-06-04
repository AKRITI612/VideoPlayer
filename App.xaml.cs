using System;
using System.Windows;
using VideoPlayerApplication.Model;
using VideoPlayerApplication.Service;

namespace VideoPlayerApplication
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            string fileName = AppDomain.CurrentDomain.BaseDirectory + @"\" + "UserConfig.xml";

            //Load application settings.
            MediaPlayerSettings.settingsFilePath = AppDomain.CurrentDomain.BaseDirectory + "VideoPlayerSettings.xml";
            MediaPlayerSettings.LoadMediaPlayerSettings();

            //Set the Favourites Config file path
            VideoFileMgr.Instance.ConfigFilePath = fileName;
        }
    }
}
