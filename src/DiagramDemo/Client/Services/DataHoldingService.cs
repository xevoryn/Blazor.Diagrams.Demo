using System;
using System.Collections.Generic;

namespace DiagramDemo.Client.Services
{
    public static class DataHoldingService
    {
        public static event Action UpdateSidebar;

        public static List<string> Log { get; } = new List<string>();

        public static void Add (string logEntry)
        {
            Log.Add(logEntry);
            UpdateSidebar?.Invoke();
        }
    }
}
