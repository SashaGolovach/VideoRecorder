﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Collections.ObjectModel;
using System.IO;
using Ozeki.Camera;
using Ozeki.Media;
using System.Diagnostics;
using System.Threading;
using VideoRecorderLibrary.Config;
using VideoRecorderLibrary.Managers;

namespace VideoRecorderLibrary
{
    public class CameraRecorder 
    {
        MediaConnector _connector;
        MPEG4Recorder _recorder;
        IVideoSender _videoSender;
        CameraRecorderConfiguration _configuration;
        public string _filePath;
        Stopwatch _firstFrameTime;

        public CameraRecorder(IVideoSender videoSender, CameraRecorderConfiguration configuration)
        {
            if (!Directory.Exists(configuration.VideosFolderPath))
                Directory.CreateDirectory(configuration.VideosFolderPath);
            _configuration = configuration;
            _videoSender = videoSender;
            _connector = new MediaConnector();
        }

        public void StartRecording()
        {
            _firstFrameTime = new Stopwatch();
            _firstFrameTime.Start();
            _filePath = _configuration.VideosFolderPath + _configuration.GenerateFileName() + _configuration.VideoFormat;
            _recorder = new MPEG4Recorder(_filePath);
            _recorder.MultiplexFinished += recorder_MultiplexFinished;
            _connector.Connect(_videoSender, _recorder.VideoRecorder);
        }

        public void StopRecording()
        {
            if (_videoSender == null) return;
            _connector.Disconnect(_videoSender, _recorder.VideoRecorder);
            _recorder.Multiplex();

            VideoFilesManager.CheckMemoryLimitForAllVideos(_configuration.VideosFolderPath);
        }

        private void recorder_MultiplexFinished(object sender, VoIPEventArgs<bool> e)
        {
            _recorder.MultiplexFinished -= recorder_MultiplexFinished;
            _recorder.Dispose();
        }
    }
}
