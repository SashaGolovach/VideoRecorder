using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Accord.Video.FFMPEG;
using AForge.Video;
using AForge.Video.DirectShow;
using System.Collections.ObjectModel;
using System.IO;

namespace VideoRecorderLibrary
{
    public class CameraRecorder : BaseRecorder
    {
        private FilterInfo _currentDevice;
        public CameraRecorder(FilterInfo device, Configuration configuration)
        {
            _currentDevice = device;
            _configuration = configuration;
        }

        public override void StartRecording()
        {
            var source = new VideoCaptureDevice(_currentDevice.MonikerString);
            _videoSource = source;
            source.VideoResolution = source.VideoCapabilities[0];
            resolution = new Rectangle()
            {
                Width = source.VideoResolution.FrameSize.Width,
                Height = source.VideoResolution.FrameSize.Height
            };
            base.StartRecording();
        }

        public override void StopRecording()
        {
            base.StopRecording();
        }
    }
}
