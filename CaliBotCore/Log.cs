using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaliBotCore
{
    class Log
    {
        public Task WriteToLog(Exception e)
        {
            Console.WriteLine($"{DateTime.Now.ToShortTimeString()}:\t\tException Passthrough\t\tt{e.Message}");
            return Task.CompletedTask;
        }

        public Task WriteToLog(string e)
        {
            Console.WriteLine($"{DateTime.Now.ToShortTimeString()}:\t\tManual Passthrough\t\t{e}");
            return Task.CompletedTask;
        }

        public Task WriteToLog(LogMessage e)
        {
            switch (e.Severity)
            {
                case LogSeverity.Critical:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;
                case LogSeverity.Debug:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    break;
                case LogSeverity.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogSeverity.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogSeverity.Info:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
            }
            Console.WriteLine($"{DateTime.Now.ToShortTimeString()}:\t\t{e.Severity}\t\t{e.Message}");
            Console.ForegroundColor = ConsoleColor.Gray;
            return Task.CompletedTask;
        }
    }
}
