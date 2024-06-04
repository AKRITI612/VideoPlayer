using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using VideoPlayerApplication.Model;
using VideoPlayerApplication.Service;
using VideoPlayerApplication.View;
using VideoPlayerApplication.View.CommonGUI;

namespace VideoPlayerApplication.ViewModel
{
#pragma warning disable CS8601
#pragma warning disable CS8618
#pragma warning disable CS8604
#pragma warning disable CS8600

    public class MainWindowViewModel : ViewModelBase
    {

        #region Private data fields
        private bool _showFav;
        private VideoFileItem _selectedListItem;
        private bool _showHome;
        private bool _isVideoFav;
        private NewMediaElementViewModel _newMediaElementViewModel;
        private VideoFileMgr _videoFileMgrSingleton;
        private readonly string _videoTitle = "Media Player";
        public IDialogService DialogService { get; set; }
        public IVideoCollection VideoCollectionService { get; set; }

        public IShowDialog ShowDialog { get; set; }

        #endregion Private data fields

        #region Public Properties

        public ObservableCollection<VideoFileItem> MyFavourites
        {
            get => VideoFileMgrSingleton.Favouriteslist;
            set
            {
                VideoFileMgrSingleton.Favouriteslist = value;
                OnPropertyChanged();
            }
        }

        public bool ShowFav
        {
            get
            {
                return _showFav;
            }
            set
            {
                _showFav = value;
                OnPropertyChanged();
            }
        }
       
        public bool ShowHome
        {
            get
            {
                return _showHome;
            }
            set
            {
                _showHome = value;
                OnPropertyChanged();
            }
        }

        public VideoFileItem SelectedVideoItem
        {
            get => VideoFileMgrSingleton.SelectedVideoItem;
            set
            {
                VideoFileMgrSingleton.SelectedVideoItem = value;
                OnPropertyChanged();
            }
        }
       
        public bool IsVideoFav
        {
            get => _isVideoFav;
            set
            {
                _isVideoFav = value;
                OnPropertyChanged();
                OnPropertyChanged("FavBtnEnabled");
                OnPropertyChanged("RemoveFavBtnEnabled");
            }

        }

        public string VideoTitle
        {
            get => VideoFileMgrSingleton.VideoTitle;
            set
            {
                VideoFileMgrSingleton.VideoTitle = value;
                OnPropertyChanged();
            }
        }

        public bool IsVideoOpen
        {
            get => VideoFileMgrSingleton.IsVideOpen;
            set
            {
                VideoFileMgrSingleton.IsVideOpen = value;
                OnPropertyChanged();
            }
        }

        public bool RemoveFavBtnEnabled => VideoFileMgrSingleton.IsVideOpen && IsVideoFav;

        public bool FavBtnEnabled => VideoFileMgrSingleton.IsVideOpen && !IsVideoFav;

        public VideoFileItem SelectedListItem
        {
            get
            {
     
                return _selectedListItem;
            }
            set
            {
                _selectedListItem = value;
                SelectedVideoItem = _selectedListItem;
                PlaySelectedVideo(_selectedListItem);
                OnPropertyChanged();
            }
        }
    
        public NewMediaElementViewModel? NewMediaElementViewModel
        {
            get
            {
                return _newMediaElementViewModel;
            }
            set
            {
                _newMediaElementViewModel = value;
                OnPropertyChanged();
            }
        }

        public VideoFileMgr VideoFileMgrSingleton
        {
            get
            {
                return _videoFileMgrSingleton;
            }
            set
            {
                _videoFileMgrSingleton = value;
                OnPropertyChanged();
            }
        }

        private bool _isMediaLoaded;

        public bool IsMediaLoaded
        {
            get
            {
                return _isMediaLoaded;
            }
            set
            {
                _isMediaLoaded = value;
                OnPropertyChanged();
            }
        }

        #endregion Public Properties

        public MainWindowViewModel()
        {
            VideoFileMgrSingleton=VideoFileMgr.Instance;
            NewMediaElementViewModel = null;
            ShowFav = false;
            ShowHome = true;
            IsMediaLoaded = false;
            OpenFileMenuitemCmd = new RelayCommand(OpenFileCommand);
            SaveAsMenuItemCmd = new RelayCommand(SaveAsMenuItemCommand);
            ExitMenuItemCmd = new RelayCommand(ExitMenuItemCommand);
            AddToFavouriteCmd = new RelayCommand(AddToFavCommand);
            RemoveFavouriteCmd = new RelayCommand(RemoveFavouriteCommand);
            GoToHomeCmd = new RelayCommand(GoToHomeCommand);
            AboutFileCmd = new RelayCommand(AboutFileCommand);
            GoToFavCmd = new RelayCommand(GoToFavCommand);
            DownloadVideoCmd = new RelayCommand(DownloadVideoCommand);
            GoToPlayerCmd = new RelayCommand(GoToPlayerCommand);
        }

        #region ICommands
        public ICommand OpenFileMenuitemCmd { get; }
        public ICommand SaveAsMenuItemCmd { get; }
        public ICommand ExitMenuItemCmd { get; }
        public ICommand AddToFavouriteCmd { get; }
        public ICommand RemoveFavouriteCmd { get; }
        public ICommand GoToHomeCmd { get; }
        public ICommand GoToFavCmd { get; }
        public ICommand GoToPlayerCmd { get; }
        public ICommand AboutFileCmd { get; set; }
        public ICommand DownloadVideoCmd { get; set; }

        #endregion ICommands

        #region Command Methods

        /// <summary>
        ///  Invokes the Open file dialog to select the video.
        /// </summary>
        /// <param name="obj"></param>
        public void OpenFileCommand(object obj)
        {
            string path = string.Empty;
            string fileFilter = FileFilterCombiner(VideoFileMgrSingleton.SupportedFiles);
            if (DialogService.OpenFileDialog(fileFilter) == true)
            {
                path = DialogService.FilePath;
            }
            
            if (string.IsNullOrEmpty(path)) {
                return;
            }
            string fileName = Path.GetFileName(path);
            string fileType = Path.GetExtension(path);
            string Location = Path.GetDirectoryName(path);
            var videoFile = new VideoFileItem(fileName, fileType, Location, GetFileSize(path));
            IsVideoFav = false;
            //Play the video and change the view.          
            PlaySelectedVideo(videoFile);           
        }

        /// <summary>
        /// Save the current video file.
        /// </summary>
        /// <param name="obj"></param>
        public void SaveAsMenuItemCommand(object obj)
        {
            string path = string.Empty;
            string fileFilter = FileFilterCombiner(VideoFileMgrSingleton.SupportedFiles);
            if (DialogService.SaveFileDialog(fileFilter) == true)
            {
                path = DialogService.FilePath;
            }
            if (string.IsNullOrEmpty(path))
            {
                return;
            }
            byte[] mp4Bytes = File.ReadAllBytes(Path.Combine(SelectedVideoItem.VideoLocation, SelectedVideoItem.VideoName));
            File.WriteAllBytes(path, mp4Bytes);
            string fileName = Path.GetFileName(path);
            string fileType = Path.GetExtension(path);
            string Location = Path.GetDirectoryName(path);
            var videoFile = new VideoFileItem(fileName, fileType, Location, GetFileSize(path)); 
            IsVideoFav = false;

            //Play the video and change the view. 
            PlaySelectedVideo(videoFile);
        }

        /// <summary>
        ///  Shuts down the application.
        /// </summary>
        /// <param name="obj"></param>
        public void ExitMenuItemCommand(object obj)
        {
            SaveSettings();
            Application.Current.Shutdown();
        }

        /// <summary>
        ///  Add the current video to favourites list.
        /// </summary>
        /// <param name="obj"></param>
        public void AddToFavCommand(object obj)
        {                      
            if (IsVideoFav){
                return;
            }
            IsVideoFav = true;
            MyFavourites.Add(SelectedVideoItem);          
        }

        /// <summary>
        ///  Remove the current video from favouites list.
        /// </summary>
        /// <param name="obj"></param>
        public void RemoveFavouriteCommand(object obj)
        {
            if (IsVideoFav)
            {
                IsVideoFav = false;
                MyFavourites.Remove(MyFavourites.Where(i => i.VideoName == SelectedVideoItem.VideoName && i.VideoLocation == SelectedVideoItem.VideoLocation).Single());
            }          
        }

        /// <summary>
        /// Go to home page and unload the video from the player.
        /// </summary>
        /// <param name="obj"></param>
        public void GoToHomeCommand(object obj)
        {
            ShowHome = true;
            ShowFav = false;
            IsVideoOpen = false;
            NewMediaElementViewModel = null;
            VideoTitle = _videoTitle;
            IsMediaLoaded = false;
        }

        /// <summary>
        /// Go to favourites page and displays the list of favourites video.
        /// </summary>
        /// <param name="obj"></param>
        public void GoToFavCommand(object obj)
        {
            PauseIfVideoIsPlaying();
            IsVideoOpen = false;
            ShowFav = true;
            ShowHome = false;            
        }

        /// <summary>
        ///  Displays information about the current video file.
        /// </summary>
        /// <param name="obj"></param>
        public void AboutFileCommand(object obj)
        {
            PauseIfVideoIsPlaying();
            ShowDialog.ShowDialog<AboutDialog>();
        }

        /// <summary>
        /// Invokes the download video file dialog.
        /// </summary>
        /// <param name="obj"></param>
        public void DownloadVideoCommand(object obj)
        {
            PauseIfVideoIsPlaying();
            if (ShowDialog.ShowDialog<DownloadVideoControl>())
            {
                PlaySelectedVideo(VideoFileMgrSingleton.SelectedVideoItem);
            }
        }

        /// <summary>
        /// Go to the media player page.
        /// </summary>
        /// <param name="obj"></param>
        public void GoToPlayerCommand(object obj)
        {
            ShowFav = false;
            ShowHome = false;
            IsVideoOpen = true;
            OnPropertyChanged("isVideoSelected");
        }

        #endregion Command Methods

        #region Miscellaneous methods

        /// <summary>
        /// Checks if the video is a fav video;
        /// </summary>
        /// <param name="item"></param>
        public void CheckIfVideoIsfavourite(VideoFileItem item)
        {
            foreach(VideoFileItem value in MyFavourites)
            {
                if(value.VideoName.Equals(item.VideoName) && value.VideoLocation.Equals(item.VideoLocation))
                {
                    IsVideoFav = true;
                }
            }
        }

        /// <summary>
        ///  Play selected video.
        /// </summary>
        /// <param name="item"></param>
        private void PlaySelectedVideo(VideoFileItem item)
        {
            var supportedFiles = string.Join(",", VideoFileMgrSingleton.SupportedFiles);
            if (!CheckIfFileTypeIsSupported(item.VideoType))
            {
                MessageBox.Show($"Video file type not supported.Please choose a file of type {supportedFiles}.", "Unsupported Type");
                return;
            }
            VideoTitle = item.VideoName;
            ShowFav = false;
            IsVideoOpen = true;
            ShowHome = false;
            NewMediaElementViewModel ??= new NewMediaElementViewModel();
            NewMediaElementViewModel.SelectedVideoItem = item;
            IsMediaLoaded = true;
            CheckIfVideoIsfavourite(item);
            OnPropertyChanged("FavBtnEnabled");
            OnPropertyChanged("RemoveFavBtnEnabled");
        }

        /// <summary>
        /// Pause if the selected video is playing.
        /// </summary>
        private void PauseIfVideoIsPlaying()
        {
            if (NewMediaElementViewModel != null)
            {
                if (NewMediaElementViewModel.IsVideoPlaying)
                {
                    NewMediaElementViewModel.IsVideoPlaying = false;
                    NewMediaElementViewModel.Pause();
                }
            }
        }

        /// <summary>
        /// Save Favourites items
        /// </summary>
        public void SaveSettings()
        {
            // call the service method to serialize and save the contents
            VideoCollectionService.Write(VideoFileMgrSingleton.ConfigFilePath, MyFavourites.ToList());
        }

        /// <summary>
        ///  Load Favourite items.
        /// </summary>
        public void LoadSettings()
        {         
            var favList = VideoCollectionService.Read(VideoFileMgrSingleton.ConfigFilePath);
            if (favList == null)
            {
                return;
            }
            foreach (VideoFileItem item in favList)
            {
                MyFavourites.Add(item);
            }
        }

        /// <summary>
        /// Check if the selected video file type is supported or not.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool CheckIfFileTypeIsSupported(string type)
        {
            return VideoFileMgrSingleton.SupportedFiles.Contains(type);
        }

        /// <summary>
        /// Get the file Size
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public string GetFileSize(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            long fileSize = fileInfo.Length;
            string fileSizeString = $"{fileSize / 1024} KB";
            return fileSizeString;
        }

        /// <summary>
        /// Create a list if supported video files.
        /// </summary>
        /// <param name="pFilters"></param>
        /// <returns></returns>
        private static string FileFilterCombiner(List<string> pFilters)
        {
            string extensions = $@"*{string.Join(";*", pFilters.Where(pExtension => !string.IsNullOrEmpty(pExtension)))}";
            return $@"{"Video Files"} ({extensions})|{extensions}";
        }

        #endregion Miscellaneous methods
    }
}
