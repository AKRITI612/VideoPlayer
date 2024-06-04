using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoPlayerApplication.Model;

namespace VideoPlayerApplication.Service
{
    public interface IVideoCollection
    {
        List<VideoFileItem> Read(string fileName);
        void Write(string fileName, List<VideoFileItem> item);

    }
}
