using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpNation.Diagnostics
{
    public static class GlobalPerformanceLog
    {
        static GlobalPerformanceLog()
        {
            Logs = new List<PerformanceLog>();
        }

        private static List<PerformanceLog> Logs;

        public static PerformanceLog AddPerformanceLog(string name)
        {
            PerformanceLog log = new PerformanceLog(name);

            Logs.Add(log);

            return log;
        }

        public static string[] GetLogData()
        {
            string[] data = new string[Logs.Count];

            for (int i = 0; i < Logs.Count; i++)
            {
                Logs[i].CalculatePerformanceData(120);
                data[i] = i + ") " + Logs[i].ToString();
            }

            return data;
        }

        public static void RemoveLogs()
        {
            Logs.Clear();
        }
    }
}
