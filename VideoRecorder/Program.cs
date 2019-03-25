using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VideoRecorderLibrary;
using AForge.Video.DirectShow;
using System.IO;

namespace VideoRecorder
{
    class Program
    {
        static int time = 15_000;
        public static void ScreenVideoRecording()
        {
            var configuration = new Configuration()
            {
                VideoSourceName = "Screen",
                VideoMaxDuration = new TimeSpan(0, 0, 4)
            };
            using (var recorder = new ScreenRecorder(configuration))
            {
                recorder.StartRecording();
                Thread.Sleep(time);
                recorder.StopRecording();
            }
        }
        public static void CameraVideoRecording(FilterInfo c)
        {
            var configuration = new Configuration()
            {
                VideoSourceName = "Cam",
                VideoMaxDuration = new TimeSpan(0, 0, 4)
            };
            using (var recorder = new CameraRecorder(c, configuration))
            {
                recorder.StartRecording();
                Thread.Sleep(time);
                recorder.StopRecording();
            }
        }
        static void Main(string[] args)
        {
            Directory.SetCurrentDirectory(Directory.GetCurrentDirectory() + "\\videos\\");
            var tList = new List<Thread>();
            tList.Add(new Thread(ScreenVideoRecording));
            foreach (var c in VideoDevicesManager.GetAllVideoDevices())
            {
                tList.Add(new Thread(() => CameraVideoRecording(c)));
                Console.WriteLine(c.MonikerString);
            }
            foreach (var t in tList)
            {
                t.Start();
                Thread.Sleep(1000);
            }
        }
    }
}
