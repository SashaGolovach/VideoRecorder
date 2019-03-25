using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Accord.Video.FFMPEG;
using AForge.Video;

namespace VideoRecorderLibrary
{
    public class ScreenRecorder : BaseRecorder
    {
        public ScreenRecorder(Configuration configuration)
        {
            _configuration = configuration;
            resolution = new Rectangle();
            foreach (var screen in System.Windows.Forms.Screen.AllScreens)
                resolution = Rectangle.Union(resolution, screen.Bounds);
        }

        public override void StartRecording()
        {
                _videoSource = new ScreenCaptureStream(resolution);
                base.StartRecording();
        }

        public override void StopRecording()
        {
            base.StopRecording();
        }
    }
}
