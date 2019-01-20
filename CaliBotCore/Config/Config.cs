using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CaliBotCore;
using CaliBotCore.Config;
using System.Threading.Tasks;

namespace CaliBotCore.Config
{
    class Config
    {
        //Make sure all the directories and files are here
        public static void Save()
        {
            Configfile newconfig = new Configfile() { botid = Program.Botid, googleApi = Program.GoogleApi, ownerID = Program.OwnerID, token = Program.Token, nepmote = Program.nepmote, OwnerGithub = Program.OwnerGithub, BotRepo = Program.BotRepo };
            Json.CreateJson("config", $"{Program.Rootdir}\\Config", newconfig);
        }

        public static async Task FindDirectoriesAsync(Log log)
        {
            //Create Directories
            if (!Directory.Exists($"{Program.Rootdir}\\Config"))
                Directory.CreateDirectory($"{Program.Rootdir}\\Config");

            if (!Directory.Exists($"{Program.Rootdir}\\Guilds"))
                Directory.CreateDirectory($"{Program.Rootdir}\\Guilds");

            if (!Directory.Exists($"{Program.Rootdir}\\Music"))
                Directory.CreateDirectory($"{Program.Rootdir}\\Music");

            if (!Directory.Exists($"{Program.Rootdir}\\Personas"))
                Directory.CreateDirectory($"{Program.Rootdir}\\Personas");

            if (!Directory.Exists($"{Program.Rootdir}\\Resources"))
                Directory.CreateDirectory($"{Program.Rootdir}\\Resources");

            if (!Directory.Exists($"{Program.Rootdir}\\Resources\\ffmpeg"))
                Directory.CreateDirectory($"{Program.Rootdir}\\Resources\\ffmpeg");

            if (!Directory.Exists($"{Program.Rootdir}\\Resources\\Fonts"))
                Directory.CreateDirectory($"{Program.Rootdir}\\Resources\\Fonts");

            if (!Directory.Exists($"{Program.Rootdir}\\Resources\\gifsicle"))
                Directory.CreateDirectory($"{Program.Rootdir}\\Resources\\gifsicle");

            if (!Directory.Exists($"{Program.Rootdir}\\Resources\\youtube-dl"))
                Directory.CreateDirectory($"{Program.Rootdir}\\Resources\\youtube-dl");

            if (!Directory.Exists($"{Program.Rootdir}\\Users"))
                Directory.CreateDirectory($"{Program.Rootdir}\\Users");

            if (!Directory.Exists($"{Program.Rootdir}\\Waifus"))
                Directory.CreateDirectory($"{Program.Rootdir}\\Waifus");

            if (!File.Exists($"{Program.Rootdir}\\Config\\config.json"))
            {
                await log.WriteToLog("Config not found, creating");
                Configfile file = new Configfile();
                Json.CreateJson("config", $"{Program.Rootdir}\\Config", file);
            }
            else
                await log.WriteToLog("Config found");

            await Task.CompletedTask;
        }
    }

    [Serializable]
    class Configfile
    {
        public string OwnerGithub = "CallyyllaC";
        public string BotRepo = "CaliBot";
        public string token = "null";
        public string googleApi = "null";
        public ulong botid = 0;
        public ulong ownerID = 0;
        public string nepmote = "<a:TopNep:393785805845299200>";
    }

}