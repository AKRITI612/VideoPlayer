using System;
using System.IO;
using System.Xml.Serialization;
using VideoPlayerApplication.Model;
using VideoPlayerApplication.StatusLogging;

namespace VideoPlayerApplication.Service
{
    public static class MediaPlayerSettings
    {
        public static string? settingsFilePath;

        /// <summary>
        /// Load Media player Setting file.
        /// </summary>
        public static void LoadMediaPlayerSettings()
        {
            try
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(MediaPlayerInfo));
                using (TextReader reader = new StreamReader(settingsFilePath))
                {
                    if (deserializer.Deserialize(reader) is MediaPlayerInfo allVideoOption)
                    {
                        VideoFileMgr.Instance.SetVideoPlayerInfo(allVideoOption);
                    }
                }
            }
            catch (Exception)
            {
                Logger.Instance.AddEntry("Unable to load the Media Player settings file.");
            }
        }

        /// <summary>
        /// Save media player settings file.
        /// </summary>
        public static void SaveMediaPlayerSettings()
        {
            try
            {
                MediaPlayerInfo allVideoOptions = new MediaPlayerInfo
                {
                    SupportedVideoFileTypes = ".mp3,.mp4"
                };
                XmlSerializer serializer = new XmlSerializer(allVideoOptions.GetType());
                
                using (TextWriter writer = new StreamWriter(settingsFilePath))
                {
                    serializer.Serialize(writer, allVideoOptions);
                }               
            }
            catch (Exception)
            {
                Logger.Instance.AddEntry("Unable to save the Media player settings file.");
            }
        }

    }
}
