using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AForge.Video.DirectShow;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VideoRecorderLibrary;

namespace VideoRecorderLibraryTest
{
    [TestClass]
    public class CameraAndScreenVideoTests
    {
        const int time = 1_000;
        public void ScreenVideoRecordingTest()
        {
            var recorder = new ScreenRecorder();
            recorder.StartRecording();
            Thread.Sleep(time);
            recorder.StopRecording();
        }

        public void CameraVideoRecordingTest(FilterInfo c)
        {  
            var recorder = new CameraRecorder(c);
            recorder.StartRecording();
            Thread.Sleep(time);
            recorder.StopRecording();
        }

        [TestMethod]
        public void CameraAndScreenVideoRecordingTest()
        {
            var tList = new List<Thread>();
            tList.Add(new Thread(ScreenVideoRecordingTest));
            foreach(var c in VideoDevicesManager.GetAllVideoDevices())
            {
                tList.Add(new Thread(() => CameraVideoRecordingTest(c)));
                Thread.Sleep(1000);
                Console.WriteLine(c.MonikerString);
            }
            foreach (var t in tList)
                t.Start();
            Thread.Sleep(time);
            Assert.IsTrue(true);
        }
    }
}
