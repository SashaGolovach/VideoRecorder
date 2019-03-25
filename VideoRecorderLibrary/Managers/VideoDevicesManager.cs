using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoRecorderLibrary
{
    public static class VideoDevicesManager
    {
        public static ObservableCollection<FilterInfo> GetAllVideoDevices()
        {
            var videoDevices = new ObservableCollection<FilterInfo>();
            var devices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo device in devices)
                videoDevices.Add(device);
            return videoDevices;
        }
    }
}
