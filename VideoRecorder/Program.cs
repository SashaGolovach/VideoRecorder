using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VideoRecorderLibrary;
using AForge.Video.DirectShow;
using System.IO;
using Serilog;
using VideoRecorderLibrary.Managers;
using VideoRecorderLibrary.Server;
using Ozeki.Camera;
using Ozeki.Media;
using VideoRecorderLibrary.Config;

namespace VideoRecorder
{
    class Program
    {
        static CameraRecorder rec;
        static Timer timer;
        static readonly WebCamera cam = new WebCamera(WebCameraFactory.GetDevices()[0]);
        public static void Renew(object s)
        {
            if(rec != null)
                rec.StopRecording();

            var configuration = new CameraRecorderConfiguration()
            {
                VideosFolderPath = Directory.GetCurrentDirectory() + @"\Camera\",
                VideoFormat = ".mpeg4"
            };

            rec = new CameraRecorder(cam, configuration);
            rec.StartRecording();
        }

        public static void ScreenVideoRecording()
        {
            var configuration = new ScreenRecorderConfiguration()
            {
                VideosFolderPath = Directory.GetCurrentDirectory() + @"\Screen\",
                VideoFormat = ".avi",
                VideoMaxDuration = new TimeSpan(0, 0, 10)
            };
            var recorder = new ScreenRecorder(configuration);
            recorder.StartRecording();
        }

        static void Main(string[] args)
        {
            cam.Start();
            VideoFilesManager.MemoryLimit = 10;

            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console()
            .CreateLogger();

            timer = new Timer(new TimerCallback(Renew), 0, 0, 10_000);

            VideoBroadcatingServer server;
            server = new VideoBroadcatingServer(cam);
            server.StartStreaming();            

            ScreenVideoRecording();

            Console.ReadLine();
        }
    }
}
