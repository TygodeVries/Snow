using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Admin
{
    public class Log
    {
        static List<LogEntry> logEntries = new List<LogEntry>();

        public static void Send(string msg)
        {
            Console.WriteLine($"[{DateTime.Now.ToShortTimeString()}] {msg}");
            logEntries.Add(new LogEntry(false, DateTime.Now, msg));
        }

        public static void Err(string err)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[{DateTime.Now.ToShortTimeString()}] {err}");
            Console.ForegroundColor = ConsoleColor.White;

            logEntries.Add(new LogEntry(true, DateTime.Now, err));
        }
    }

    public class LogEntry
    {
        bool isError;
        DateTime time;
        string msg;

        public LogEntry(bool isError, DateTime time, string msg)
        {
            this.isError = isError;
            this.time = time;
            this.msg = msg;
        }
    }
}
