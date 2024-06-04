using System;
using VideoPlayerApplication.Model;

namespace VideoPlayerApplication.ViewModel
{
    public class AboutDialogViewModel:ViewModelBase
    {
        public AboutDialogViewModel()
        {
            AboutText = string.Empty; ;
            CreateAboutText();
        }

        public string AboutText { get; private set; }

        /// <summary>
        /// Get video File Information from VideoFileMgr.
        /// </summary>
        public void CreateAboutText()
        {
            string fileName = VideoFileMgr.Instance.SelectedVideoItem.VideoName;
            string filetype = VideoFileMgr.Instance.SelectedVideoItem.VideoType;
            string fileLocation = VideoFileMgr.Instance.SelectedVideoItem.VideoLocation;
            string fileSize = VideoFileMgr.Instance.SelectedVideoItem.VideoSize;
            AboutText = "File Name: " + fileName + Environment.NewLine
                + "File Type: " + filetype + Environment.NewLine
                + "File Location: " + fileLocation + Environment.NewLine
                + "File Size: " + fileSize + Environment.NewLine;
        }
    }
}
