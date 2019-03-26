using Accord.Video.FFMPEG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VideoRecorderLibrary.Managers
{
    internal static class VideoFileWritersManager
    {
        public static VideoFileWriter GetVideoFileWriter()
        {
            Thread.Sleep(1000);
            return new VideoFileWriter();
        }
    }
}
