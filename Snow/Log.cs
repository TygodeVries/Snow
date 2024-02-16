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
            Console.WriteLine($"[{DateTime.Now.ToShortTimeString()}] {msg}");
        }

        public static void Err(string err)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[{DateTime.Now.ToShortTimeString()}] {err}");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
