using System;
using System.Collections.Generic;
using System.Text;
using CaliBotCore.Config;

namespace CaliBotCore.DataStructures
{
    class Updater
    {
        public static void Save(Bot bot)
        {
            Json.CreateJson("Bot_Properties", $"{Program.Rootdir}\\Config", bot);
        }
        public static Bot Load()
        {
            try
            {
                return Json.CreateObject<Bot>($"{Program.Rootdir}\\Config\\Bot_Properties.json");
            }
            catch
            {
                return null;
            }
        }

        public static Bot Reset(Bot tmp)
        {
            tmp.channelId = 0;
            tmp.messageId = 0;
            tmp.updated = false;
            tmp.restart = false;
            tmp.guildId = 0;
            Save(tmp);
            return tmp;
        }
    }

    [Serializable]
    class Bot
    {
        public bool updated = false;
        public bool restart = false;
        public ulong guildId = 0;
        public ulong channelId = 0;
        public ulong messageId = 0;
    }
}
