using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DiagramDemo.Shared
{
    public static class PerfLoggr
    {
        private static readonly Stopwatch s_sw = new();
        private static readonly List<string> s_log = new();
        public static bool LiveLogging { get; set; } = false;

        public static void Start()
        {
            s_log.Clear();
            s_sw.Restart();
        }

        public static void Log(string msg)
        {
            var logMsg = $"{s_sw.ElapsedMilliseconds} - {msg}";
            s_log.Add(logMsg);

            if (LiveLogging)
                Console.WriteLine(logMsg);
        }

        public static void Write()
        {
            foreach (var line in s_log)
                Console.WriteLine(line);
        }
    }
}
