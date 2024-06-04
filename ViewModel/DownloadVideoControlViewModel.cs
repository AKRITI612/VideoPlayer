using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Input;
using VideoPlayerApplication.Model;
using VideoPlayerApplication.StatusLogging;
using VideoPlayerApplication.View.CommonGUI;

namespace VideoPlayerApplication.ViewModel
{

    public class DownloadVideoControlViewModel:ViewModelBase
    {
        public event Action RequestClose;

        #region Private data fields
        private string _url;
        private int _downloadProgress;
        private bool _isDownloading;
        private string _downloadTextStatus;
        private VideoFileItem _downloadVideoItem;
        private VideoFileMgr _videoFileMgrSingleton;
        #endregion Private data fields

        #region Public Properties
        public string Url
        {
            get => _url;
            set
            {
                _url = value;
                OnPropertyChanged();
            }
        }

        public int DownloadProgress
        {
            get => _downloadProgress;
            set
            {
                _downloadProgress = value;
                OnPropertyChanged();
            }
        }


        public bool IsDownloading
        {
            get => _isDownloading;
            set
            {
                _isDownloading = value;
                OnPropertyChanged();
            }
        }


        public string DownloadTextStatus
        {
            get => _downloadTextStatus;
            set
            {
                _downloadTextStatus = value;
                OnPropertyChanged();
            }
        }


        public VideoFileItem DownloadVideoItem
        {
            get => _downloadVideoItem;
            set
            {
                _downloadVideoItem = value;
                OnPropertyChanged();
            }
        }

        public VideoFileMgr VideoFileMgrSingleton
        {
            get => _videoFileMgrSingleton;
            set
            {
                _videoFileMgrSingleton = value;
                OnPropertyChanged();
            }
        }
        #endregion Public Properties

        public DownloadVideoControlViewModel()
        {
            VideoFileMgrSingleton = VideoFileMgr.Instance;
            DownloadTextStatus = string.Empty;
            DownVideoCmd = new RelayCommand(DownloadVideoCommand);
            PlayDownoadedVideoCmd = new RelayCommand(PlayDownoadedVideoCommand);
        }

        #region ICommands
        public ICommand DownVideoCmd { get; }

        public ICommand PlayDownoadedVideoCmd { get; }
        #endregion ICommands

        #region Command Methods
        /// <summary>
        ///  Download Video ICommand
        /// </summary>
        /// <param name="obj"></param>
        private async void DownloadVideoCommand(object obj)
        {
            try
            {
                if(Url == null)
                {
                    return;
                }
                var supportedFiles = string.Join(",", VideoFileMgrSingleton.SupportedFiles);
                var path = AppDomain.CurrentDomain.BaseDirectory;
                var extension = Path.GetExtension(Url);
                string? filename = Path.GetFileName(Url);
                var outputPath = Path.Combine(path, filename);
                var videoFile = new VideoFileItem(filename, Path.GetExtension(outputPath), Path.GetDirectoryName(path), string.Empty);
                if (!CheckIfFileTypeIsSupported(extension))
                {
                    MessageBox.Show($"Video file type not supported.Please choose a file of type {supportedFiles}.", "Unsupported Type");
                    return;
                }
                IsDownloading = true;
                DownloadTextStatus = "Downloading...";
                DownloadVideoItem = videoFile;
                using (WebClient client = new WebClient())
                {
                    client.DownloadProgressChanged += (sender, e) =>
                      {
                          DownloadProgress = e.ProgressPercentage;
                      };
                    await client.DownloadFileTaskAsync(new Uri(Url), outputPath);
                }
                IsDownloading = false;
                DownloadTextStatus = "Download Completed...";
                
            }
            catch (Exception)
            {
                Logger.Instance.AddEntry($"Unable to Download the file.");
            }
        }       

        /// <summary>
        /// Sets the selected video and closes the window.
        /// </summary>
        /// <param name="obj"></param>
        private void PlayDownoadedVideoCommand(object obj)
        {
            VideoFileMgrSingleton.SelectedVideoItem = DownloadVideoItem;   
            SetFileSize(Path.Combine(DownloadVideoItem.VideoLocation+"\\"+ DownloadVideoItem.VideoName));
            RequestClose?.Invoke();
        }
        #endregion Command Methods

        /// <summary>
        /// Check If file is Supported
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool CheckIfFileTypeIsSupported(string type)
        {
            return VideoFileMgr.Instance.SupportedFiles.Contains(type);
        }

        /// <summary>
        /// Get File Size
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public void SetFileSize(string filePath)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(filePath);
                long fileSize = fileInfo.Length;
                string fileSizeString = $"{fileSize / 1024} KB";
                VideoFileMgrSingleton.SelectedVideoItem.VideoSize = fileSizeString;
            }
            catch (Exception ex)
            {
                Logger.Instance.AddEntry(ex.ToString());
            }
        }
    }
}
