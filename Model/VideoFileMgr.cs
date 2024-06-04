using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace VideoPlayerApplication.Model
{

#pragma warning disable CS8618

    public class VideoFileMgr
    {
        private readonly string _videoTitle = "Media Player";      

        private static VideoFileMgr _mInst;
        public static VideoFileMgr Instance
        {
            get => _mInst ??= new VideoFileMgr();

            private set => _mInst = value;
        }

        public string VideoTitle { get ; set ; }
        public string ConfigFilePath { get ; set ; }
        public bool IsVideOpen { get ; set ; }
        public List<string> SupportedFiles { get; set; }
        public VideoFileItem SelectedVideoItem 
        { 
          get;
          set ; 
        }
        public ObservableCollection<VideoFileItem> Favouriteslist { get ; set ; }

        private VideoFileMgr()
        {           
            VideoTitle = _videoTitle;
            IsVideOpen = false;
            Favouriteslist = new ObservableCollection<VideoFileItem>();
            SupportedFiles = new List<string>();
        }

        /// <summary>
        ///  Set the supported File Types.
        /// </summary>
        /// <param name="pAllOptions"></param>
        public void SetVideoPlayerInfo(MediaPlayerInfo pAllOptions)
        {
            SupportedFiles = pAllOptions.SupportedVideoFileTypes.Split(',').ToList();
        }
    }
}
