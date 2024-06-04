using System;

namespace VideoPlayerApplication.Model
{
   
    [Serializable]
    public class VideoFileItem
    {        
        #pragma warning disable CS8618 

        public VideoFileItem()
        {

        }

        public VideoFileItem(string VideoName, string VideoType,string VideoLocation,string VideoSize)
        {
            this.VideoName = VideoName;
            this.VideoType = VideoType;
            this.VideoLocation = VideoLocation;
            this.VideoSize = VideoSize;
        }

        /// <summary>
        /// Video Name 
        /// </summary>
        public string VideoName { get; set; }

        /// <summary>
        /// Video Type
        /// </summary>
        public string VideoType { get; set; }

        /// <summary>
        /// Video Location
        /// </summary>
        public string VideoLocation { get; set; }

        /// <summary>
        /// Video Size
        /// </summary>
        public string VideoSize { get; set; }
    }
}
