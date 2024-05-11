using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow
{
    public class Log
    {
        public static void Send(string msg)
        {
            Console.WriteLine($"(Log) [{DateTime.Now.ToShortTimeString()}] {msg}");
        }

        public static void Err(string err)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"(Err) [{DateTime.Now.ToShortTimeString()}] {err}");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void Warn(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"(Warn) [{DateTime.Now.ToShortTimeString()}] {msg}");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
