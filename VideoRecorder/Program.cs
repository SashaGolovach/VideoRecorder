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

namespace VideoRecorder
{
    class Program
    {
        public static void ScreenVideoRecording()
        {
            var configuration = new Configuration()
            {
                VideosFolderPath = Directory.GetCurrentDirectory() + @"\Screen\",
                VideoMaxDuration = new TimeSpan(0, 0, 4)
            };
            var recorder = new ScreenRecorder(configuration);
            recorder.StartRecording();
        }

        public static void CameraVideoRecording(FilterInfo c)
        {
            var configuration = new Configuration()
            {
                VideosFolderPath = Directory.GetCurrentDirectory() + @"\Camera\",
                VideoMaxDuration = new TimeSpan(0, 0, 4)
            };
            var recorder = new CameraRecorder(c, configuration);
            recorder.StartRecording();
        }

        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console()
            .CreateLogger();

            var tList = new List<Thread>();
            tList.Add(new Thread(ScreenVideoRecording));
            foreach (var c in VideoDevicesManager.GetAllVideoDevices())
            {
                tList.Add(new Thread(() => CameraVideoRecording(c)));
                Console.WriteLine(c.MonikerString);
            }
            //tList[0].Start();
            VideoFilesManager.MemoryLimit = 10_000_000;

            foreach (var t in tList)
            {
                t.Start();
                Thread.Sleep(1000);
            }
        }
    }
}
