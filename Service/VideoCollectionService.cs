using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using VideoPlayerApplication.Model;
using VideoPlayerApplication.StatusLogging;

namespace VideoPlayerApplication.Service
{
    public class VideoCollectionService: IVideoCollection
    {
        /// <summary>
        /// Method to read and deserialize the data
        /// </summary>
        /// <param name="fileName">file name</param>
        /// <returns>canvas model</returns>
        public List<VideoFileItem> Read(string fileName)
        {
            List<VideoFileItem> item = new List<VideoFileItem>();
            try
            {               
                FileStream fs = new FileStream(fileName, FileMode.Open);
                XmlSerializer serializer = new XmlSerializer(typeof(List<VideoFileItem>));
                using (StreamReader reader = new StreamReader(fs))
                {
                    item = (List<VideoFileItem>)serializer.Deserialize(reader);                   
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.AddEntry(ex.ToString());
            }
            return item;
        }

        /// <summary>
        /// Method to write the serialized data in the given location
        /// </summary>
        /// <param name="fileName">file Name</param>
        /// <param name="canvasModel">canvas model</param>
        public void Write(string fileName, List<VideoFileItem> item)
        {
            FileStream fs = new FileStream(fileName, FileMode.Create);
            XmlSerializer serializer = new XmlSerializer(item.GetType());
            using (StreamWriter writer = new StreamWriter(fs))
            {
                serializer.Serialize(writer, item);
            }
        }
    }
}
