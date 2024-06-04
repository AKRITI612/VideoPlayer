using System.Xml.Serialization;

namespace VideoPlayerApplication.Model
{
    [XmlRoot("MediaPlayerSettings")]
    public class MediaPlayerInfo
    {
        private static readonly string DefaultVersionNumber = "1.0";

        /// <summary>
        /// Version number of settings.
        /// Make sure to document changes while upgrading version number.
        /// </summary>
        public string VersionNumber
        {           
            get;           
            set;
        }

        public string SupportedVideoFileTypes
        {
            get;
            set;
        }
       
        /// <summary>
        /// Application Simulation Options
        /// </summary>       
        public MediaPlayerInfo()
        {
            VersionNumber = DefaultVersionNumber;
            SupportedVideoFileTypes = "";
        }
    }
}
