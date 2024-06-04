using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using VideoPlayerApplication.Model;
using VideoPlayerApplication.View.CommonGUI;

namespace VideoPlayerApplication.ViewModel
{
    #pragma warning disable CS8601
    #pragma warning disable CS8618

    public class NewMediaElementViewModel : ViewModelBase
    {
        #region Private data fields

        private DispatcherTimer _timer;
        private MediaElement _mediaElementObject;
        private bool _isVideoPlaying;
        private double _sliderMaximum;
        private double _sliderValue;
        private double _volumeLevel;
        private bool _isMuted;
        private bool _isVideoPaused;
        private string _videoTimeRemainingText;
        private bool _isMediaLoaded;
        private VideoFileMgr _videoFileMgrSingleton;

        #endregion Private data fields

        #region Public Properties
        public MediaElement MediaElementObject
        {
            get
            {
                return _mediaElementObject;
            }
            set
            {
                _mediaElementObject = value;
                OnPropertyChanged();
            }
        }

        public VideoFileItem SelectedVideoItem
        {
            get => VideoFileMgrSingleton.SelectedVideoItem;
            set
            {
                if(value==null)
                {
                    return;
                }
                VideoFileMgrSingleton.SelectedVideoItem = value;
                LoadAndPlayMediaElement();
                OnPropertyChanged();
            }
        }

        public bool IsVideoPlaying
        {
            get => _isVideoPlaying;
            set
            {
                _isVideoPlaying = value;
                OnPropertyChanged();
            }
        }
        
        public double SliderMaximum
        {
            get => _sliderMaximum;
            set
            {
                _sliderMaximum = value;
                OnPropertyChanged("SliderMaximum");
            }
        }

        public double SliderValue
        {
            get => _sliderValue;
            set
            {
                _sliderValue = value;

                if (MediaElementObject != null && MediaElementObject.HasVideo)
                {
                    MediaElementObject.Position = TimeSpan.FromSeconds(_sliderValue);
                }

                OnPropertyChanged("SliderValue");
            }
        }

        public double VolumeLevel
        {
            get => _volumeLevel;
            set
            {
                _volumeLevel = value;
                if (MediaElementObject != null)
                {
                    MediaElementObject.Volume = _volumeLevel;
                }
                OnPropertyChanged();
                OnPropertyChanged("VolumeLevelDisplay");
            }
        }

        public string VolumeLevelDisplay => ((int)VolumeLevel) + "%";

        public bool IsMuted
        {
            get => _isMuted;
            set
            {
                _isMuted = value;
                if (MediaElementObject != null)
                {
                    MediaElementObject.IsMuted = IsMuted;
                }
                OnPropertyChanged();
            }
        }

        public bool IsVideoPaused
        {
            get => _isVideoPaused;
            set
            {
                _isVideoPaused = value;
                OnPropertyChanged("IsVideoPaused");
            }
        }

        public string VideoTimeRemainingText
        {
            get => _videoTimeRemainingText;
            set
            {
                _videoTimeRemainingText = value;
                OnPropertyChanged("VideoTimeRemainingText");
            }
        }

        public bool IsMediaLoaded
        {
            get { return _isMediaLoaded; }
            set
            {
                _isMediaLoaded = value;
                OnPropertyChanged("IsMediaLoaded");
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

        public NewMediaElementViewModel()
        {
            VideoFileMgrSingleton=VideoFileMgr.Instance;
            VolumeLevel = 1;
            VideoTimeRemainingText = "00:00/00:00";
            IsVideoPlaying = false;
            IsMediaLoaded = false;
            Stop();
            StartVideoCmd = new RelayCommand(StartVideoCommand);
            PauseVideoCmd = new RelayCommand(PauseVideoCommand);
            StopVideoCmd = new RelayCommand(StopVideoCommand);
            IncreaseVolumeCmd = new RelayCommand(IncreaseVolumeCommand);
            DecreaseVolumeCmd = new RelayCommand(DecreaseVolumeCommand);
            MuteVolumeCmd = new RelayCommand(MuteVolumeCommand);
        }

        #region ICommands

        public ICommand StartVideoCmd { get; set; }
        public ICommand PauseVideoCmd { get; set; }
        public ICommand StopVideoCmd { get; set; }
        public ICommand IncreaseVolumeCmd { get; set; }
        public ICommand DecreaseVolumeCmd { get; set; }
        public ICommand MuteVolumeCmd { get; set; }

        #endregion ICommands

        #region Command Methods

        /// <summary>
        /// Start Video Button clicked.
        /// </summary>
        /// <param name="obj"></param>
        public void StartVideoCommand(object obj)
        {
            Play();            
        }
        
        /// <summary>
        ///  Pause video button clicked
        /// </summary>
        /// <param name="obj"></param>
        public void PauseVideoCommand(object obj)
        {
            Pause();           
        }

        /// <summary>
        /// Stop video Button clicked.
        /// </summary>
        /// <param name="obj"></param>
        public void StopVideoCommand(object obj)
        {
            Stop();
        }

        /// <summary>
        /// Play video.(create media element object)
        /// </summary>
        public void Play()
        {
            try
            {
                IsVideoPlaying = true;
                _timer = new DispatcherTimer();
                if (MediaElementObject != null)
                {
                    MediaElementObject.Play();
                    _timer.Interval = TimeSpan.FromSeconds(1);
                    _timer.Tick += Timer_Tick;
                    _timer.Start();
                    IsVideoPaused = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Play() Exception: " + ex);
            }
        }

        /// <summary>
        /// Pause the video.
        /// </summary>
        public void Pause()
        {
            try
            {
                if (MediaElementObject != null)
                {
                    MediaElementObject.Pause();
                    IsVideoPaused = true;
                    IsVideoPlaying = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Pause() Exception: " + ex);
            }
        }

        /// <summary>
        /// Stop the video(Unload the media element)
        /// </summary>
        public void Stop()
        {
            try
            {
                if (MediaElementObject != null)
                {
                    MediaElementObject.Stop();
                    MediaElementObject.Close();
                    IsVideoPaused = true;
                    _timer.Stop();
                    _timer = new DispatcherTimer();
                }

                VideoTimeRemainingText = "00:00/00:00";
                SliderValue = 0;
                SliderMaximum = 100;
                IsVideoPlaying = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Stop() Exception: " + ex);
            }
        }

        /// <summary>
        /// Timer to track the video progress.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (MediaElementObject != null)
                {
                    try
                    {
                        if ((MediaElementObject.Source != null) && MediaElementObject.NaturalDuration.HasTimeSpan)
                        {
                            SliderMaximum = MediaElementObject.NaturalDuration.TimeSpan.TotalSeconds;
                            SliderValue = MediaElementObject.Position.TotalSeconds;
                            VideoTimeRemainingText = MediaElementObject.Position.ToString(@"mm\:ss") + "/" +
                                                     MediaElementObject.NaturalDuration.TimeSpan.ToString(@"mm\:ss");
                        }
                    }
                    catch
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Timer_Tick() Exception: " + ex);
            }
        }

        /// <summary>
        /// Load the mediaElement.
        /// </summary>
        private void LoadAndPlayMediaElement()
        {
            try
            {
                MediaElementObject = new MediaElement
                {
                    Source = new Uri(VideoFileMgrSingleton.SelectedVideoItem.VideoLocation + "\\" + VideoFileMgrSingleton.SelectedVideoItem.VideoName),
                    LoadedBehavior = MediaState.Manual,
                    UnloadedBehavior = MediaState.Close,
                    Volume = 5.0
                };
                SliderValue = 0;
                SliderMaximum = 100;
                IsMediaLoaded = true;
                Play();
            }
            catch (Exception ex)
            {
                Console.WriteLine("LoadAndPlayMediaElement() Exception: " + ex);
            }
        }

        /// <summary>
        /// Increase the volume.
        /// </summary>
        /// <param name="obj"></param>
        public void IncreaseVolumeCommand(object obj)
        {
            VolumeLevel += 5;
        }

        /// <summary>
        /// Decrease Volume.
        /// </summary>
        /// <param name="obj"></param>
        public void DecreaseVolumeCommand(object obj)
        {
            VolumeLevel -= 5;
            if (VolumeLevel < 0.0)
            {
                VolumeLevel = 0.0;
            }
        }

        /// <summary>
        /// Mute the media element.
        /// </summary>
        /// <param name="obj"></param>
        public void MuteVolumeCommand(object obj)
        {
            IsMuted = true;
            VolumeLevel = 0;
        }

        #endregion Command Methods

    }
}
