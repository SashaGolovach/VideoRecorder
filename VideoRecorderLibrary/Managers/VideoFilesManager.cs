using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoRecorderLibrary.Managers
{
    public static class VideoFilesManager
    {
        private static long memoryLimit;
        private static TimeSpan timeLimit;
        public static long MemoryLimit
        {
            get { return memoryLimit; }
            set
            {
                if (value <= 0)
                    throw new Exception("Value can't be negative");
                memoryLimit = value;
            }
        }

        public static TimeSpan TimeLimit
        {
            get { return TimeLimit; }
            set
            {
                if (value.Equals(TimeSpan.Zero))
                    throw new Exception("Value can't be 0");
                TimeLimit = value;
            }
        }

        public static void CheckMemoryLimitForAllVideos(string videosFolderPath)
        {
            DirectoryInfo videoFilesDirectory = new DirectoryInfo(videosFolderPath);
            FileInfo[] videoFiles = videoFilesDirectory.GetFiles();
            long videoFilesSize = 0;
            foreach (FileInfo file in videoFiles)
            {
                videoFilesSize += file.Length;
            }
            if (memoryLimit != 0)
            {
                while (videoFilesSize >= memoryLimit)
                {
                    videoFilesSize -= videoFiles[0].Length;
                    videoFiles[0].Delete();
                }
            }
        }

        public static void CheckTimeLimitForAllVideos(string videosFolderPath)
        {
        }
    }
}
