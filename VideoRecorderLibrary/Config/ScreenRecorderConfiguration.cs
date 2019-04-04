using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoRecorderLibrary
{
    public class ScreenRecorderConfiguration
    {
        public string VideoFormat { get; set; }
        public TimeSpan? VideoMaxDuration { get; set; }
        public string VideosFolderPath { get; set; }
        public string GenerateFileName()
        {
            var date = DateTime.Now;
            return String.Join(".", date.Day, date.Month, date.Year, date.Hour, date.Minute, date.Second);
        }
    }
}
