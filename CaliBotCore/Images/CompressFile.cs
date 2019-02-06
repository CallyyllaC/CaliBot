using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CaliBotCore.Images
{
    class CompressFile
    {
        private static string gifsicle = $"{Program.Rootdir}\\Resources\\gifsicle\\gifsicle.exe";

        public static async Task CompressGifAsync(ulong Id, string location)
        {
            Process process = new Process();
            process.StartInfo.FileName = gifsicle;
            process.StartInfo.Arguments = $"−b --unoptimize --colors 256 --lossy=50 --color-method diversity −O3 −−no−extensions {location}";
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            Console.WriteLine("Started Gifsicle");
            process.WaitForExit();
            Console.WriteLine("Gifsicle Finished");
            await Task.CompletedTask;
        }

        public static async Task<byte[]> CompressGifAsync(byte[] image)
        {
            var file = await CheckForDupesAndSave(image);
            Process process = new Process();
            process.StartInfo.FileName = gifsicle;
            process.StartInfo.Arguments = $"−b --unoptimize --colors 256 --lossy=50 --color-method diversity −O3 −−no−extensions {file}";
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            Console.WriteLine("Started Gifsicle");
            process.WaitForExit();
            Console.WriteLine("Gifsicle Finished");
            var tmp = await File.ReadAllBytesAsync(file);
            File.Delete(file);
            return tmp;
        }

        public static async Task<string> CheckForDupesAndSave(byte[] image)
        {
            int i = 0;
            string name = $"tmp{i}";
            while (File.Exists($"{Program.Rootdir}\\Resources\\{name}.gif"))
            {
                i++;
                name = $"tmp{i}";
            }
            await File.WriteAllBytesAsync($"{Program.Rootdir}\\Resources\\{name}.gif", image);
            await Task.CompletedTask;
            return $"{Program.Rootdir}\\Resources\\{name}.gif";
        }
    }
}
