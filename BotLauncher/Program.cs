using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;

namespace BotLauncher
{
    class Program
    {
        public static string OwnerGithub = "CallyyllaC";
        public static string BotRepo = "CaliBot";
        public static string Ver = "0.6";

        static void Main(string[] args)
        {
            if (!Directory.Exists("Bot") || !File.Exists("Bot/CaliBotCore.exe"))
            {
                Directory.CreateDirectory("Bot");
                Console.WriteLine("Bot not found, Bot directory created, Once the bot launches, please ensure that it is updated to the latest version using the update command");

                string url = @"https://github.com/{OwnerGithub}/{BotRepo}/releases/download/{Ver}/Release.zip";
                string path = $"Release.zip";
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                using (var client = new System.Net.Http.HttpClient())
                {
                    var contents = client.GetByteArrayAsync(url).Result;
                    File.WriteAllBytes(path, contents);
                }

                ZipFile.ExtractToDirectory(@"Release.zip", "Bot", true);
            }

            var bot = Restart();
            while (true)
            {
                bot.WaitForExit();
                var exit = bot.ExitCode;
                Console.WriteLine($"Bot exited with code: {exit}");
                if (exit == 60000)
                {
                    Console.WriteLine($"Restarting...");
                    bot = Restart();
                }
                else if (exit == 60001)
                {
                    Console.WriteLine($"Updating");
                    ZipFile.ExtractToDirectory(@"Release.zip", "Bot", true);
                    bot = Restart();
                }
                else
                {
                    Console.WriteLine("Unspecified exit");
                    Console.ReadKey();
                }
            }

        }

        private static Process Restart()
        {
            var bot = Process.Start("Bot/CaliBotCore.exe");
            return bot;
        }
    }
}
