using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memory.Timers
{
    public class Timer : IDisposable
    {
        public static List<string> ProcessTime = new List<string>();
        public static StringBuilder ReportBuilder = new StringBuilder();
        public static string Report;
        private Stopwatch timer;
        public static int LastStage;
        public static long TotalTime;
        public Timer(string value)
        {
            timer = new Stopwatch();
            timer.Start();
            ProcessTime.Add(value);
            LastStage = ProcessTime.Count();
        }

        public static Timer Start()
        {
            return Start("*");
        }

        public static Timer Start(string value)
        {
            return new Timer(value);
        }

        ~Timer()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public void Dispose(bool isDisposed)
        {
            if (isDisposed)
            {
                timer.Stop();
                var currentStage = ProcessTime.Count();
                var currentTime = timer.ElapsedMilliseconds;
                var timeString = $": {currentTime.ToString()}\n";
                var element = ProcessTime[currentStage - 1].PadLeft((currentStage - 1) * 4 + 1).PadRight(20);
                if (LastStage > currentStage)
                {
                    var rest = "Rest".PadLeft((currentStage + 1) * 4).PadRight(20);
                    var restTimeString = $": {(currentTime - TotalTime).ToString()}\n";
                    ReportBuilder.Append(rest + restTimeString);
                    ReportBuilder.Insert(0, element + timeString);
                }
                else
                    ReportBuilder.Append(element + timeString);
                ProcessTime.RemoveAt(currentStage - 1);
                Report = ReportBuilder.ToString();
                if (currentStage - 1 == 0) ReportBuilder.Clear();
                TotalTime += currentTime;
            }
        }
    }
}
