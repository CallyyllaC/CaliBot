using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Timers;

namespace CaliBotCore
{
    class Personas
    {
        private static readonly Timer avatartimer = new Timer();
        //avatar locations and names
        public static Collection<string[]> avatars = new Collection<string[]>();
        DiscordSocketClient client;
        private Random random = new Random();

        //constructor and timer setup
        public Personas(DiscordSocketClient Client)
        {
            client = Client;
            foreach (var item in Directory.GetFiles($"{Program.Rootdir}\\Personas\\", "*.png", SearchOption.AllDirectories))
            {
                string[] newitem = new string[2];
                newitem[0] = Path.GetFileNameWithoutExtension(item);
                newitem[1] = item;
                avatars.Add(newitem);
            }
            Switchavatar();
            avatartimer.Interval = (24* 60 * 60 * 1000);
            avatartimer.AutoReset = true;
            avatartimer.Elapsed += Avatartimer_ElapsedAsync;
            avatartimer.Start();
        }

        //timer elapsed
        private async void Avatartimer_ElapsedAsync(object sender, ElapsedEventArgs e)
        {
            try
            {
                Switchavatar();
            }
            catch (Exception er)
            {
                await Program.Log.WriteToLog(er.Message);
            }
        }

        //Add & Remove
        public async void AddAsync(string[] tmp)
        {
            try
            {
                avatars.Add(tmp);
            }
            catch (Exception e)
            {
                await Program.Log.WriteToLog(e.Message);
            }
        }
        public async void RemoveAsync(string name)
        {
            try
            {
                foreach (var item in avatars)
                {
                    if (item[0] == name)
                    {
                        File.Delete(item[1]);
                        avatars.Remove(item);
                    }
                }
            }
            catch (Exception e)
            {
                await Program.Log.WriteToLog(e.Message);
            }
        }

        //changes bots name and avatar
        private async void Switchavatar()
        {
            int num = random.Next(avatars.Count);
            Discord.Image img = new Discord.Image(avatars[num][1]);
            try
            {
                await client.CurrentUser.ModifyAsync(x =>
                {
                    try { x.Avatar = img; }
                    catch { }
                    try { x.Username = avatars[num][0]; }
                    catch { x.Username = "error"; }
                });
                Console.WriteLine("Complete change avatar request");
            }
            catch { Console.WriteLine("Unable to complete change avatar request"); }
            Program.CurrentName = client.CurrentUser.Username;
            Program.CurrentUrl = client.CurrentUser.GetAvatarUrl();
        }
    }
}
