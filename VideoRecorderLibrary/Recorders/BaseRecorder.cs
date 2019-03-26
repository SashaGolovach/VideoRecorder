using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using AForge.Video;
using Accord.Video.FFMPEG;
using System.IO;
using System.Diagnostics;
using Serilog;
using VideoRecorderLibrary.Managers;

namespace VideoRecorderLibrary
{
    public class BaseRecorder : IDisposable
    {
        internal Configuration _configuration;
        internal IVideoSource _videoSource;
        internal VideoFileWriter _writer;
        internal bool _recording;
        internal string _fileName;
        internal Rectangle resolution;
        internal Stopwatch _firstFrameTime;

        internal void VideoNewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            try
            {
                if (_recording)
                {
                    if (TimeOrMemoryLimitExceeded())
                    {
                        StopVideoFileWriter();
                        StartNewVideoFileWriter();
                        return;
                    }
                    using (var bitmap = (Bitmap)eventArgs.Frame.Clone())
                    {
                        _writer.WriteVideoFrame(bitmap, _firstFrameTime.Elapsed);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Information(ex.ToString());
            }
        }

        private bool TimeOrMemoryLimitExceeded()
        {

            var duration = _firstFrameTime.Elapsed;
            if (_configuration.VideoMaxDuration != null && duration >= _configuration.VideoMaxDuration)
                return true;
            return false;
        }

        public virtual void StartRecording()
        {
            _videoSource.NewFrame += VideoNewFrame;
            _videoSource.Start();

            StartNewVideoFileWriter();
        }

        private void StartNewVideoFileWriter()
        {
            _firstFrameTime = new Stopwatch();
            _firstFrameTime.Start();

            _fileName = _configuration.GenerateFileName();
            _writer = VideoFileWritersManager.GetVideoFileWriter();//new VideoFileWriter();
            _writer.Open(_fileName + _configuration.VideoFormat, resolution.Width, resolution.Height);

            _recording = true;
        }

        public virtual void StopRecording()
        {
            _recording = false;

            if (_videoSource != null && _videoSource.IsRunning)
            {
                _videoSource.SignalToStop();
                _videoSource.NewFrame -= VideoNewFrame;
            }

            StopVideoFileWriter();
        }

        private void StopVideoFileWriter()
        {
            _recording = false;

            _firstFrameTime.Stop();
            _writer.Close();
            _writer.Dispose();

            MoveRecordedVideo();
        }

        private void MoveRecordedVideo()
        {
            string fileName = _fileName + _configuration.VideoFormat;
            if (!Directory.Exists(_configuration.VideosFolderPath))
                Directory.CreateDirectory(_configuration.VideosFolderPath);
            while (true)
            {
                try
                {
                    File.Move(fileName, _configuration.VideosFolderPath + fileName);
                    break;
                }
                catch (IOException) { }
            }

            
            VideoFilesManager.CheckMemoryLimitForAllVideos(_configuration.VideosFolderPath);
            VideoFilesManager.CheckTimeLimitForAllVideos(_configuration.VideosFolderPath);
        }

        public void Dispose()
        {
            if (_videoSource != null && _videoSource.IsRunning)
            {
                _videoSource.SignalToStop();
            }
            _writer?.Dispose();
        }
    }
}
