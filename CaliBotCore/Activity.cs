using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace CaliBotCore
{
    class Activity
    {
        DiscordSocketClient client;
        private static readonly Timer gametimer = new Timer();
        private Random random = new Random();

        public Activity(DiscordSocketClient Client)
        {
            client = Client;
            gametimer.Interval = 120000;
            gametimer.AutoReset = true;
            gametimer.Elapsed += Gametimer_ElapsedAsync;
            gametimer.Start();

        }

        //change for bot to randomly look active
        public async void SendTypingAsync(SocketMessage message)
        {
            if (random.Next(100) == 1)
            {
                await message.Channel.TriggerTypingAsync();
            }
        }

        //change for bot to randomly look active
        public async void Emoji(IUserMessage message, int chance)
        {
            if (random.Next(chance) == 1)
            {
                await message.AddReactionAsync(Program.Emotes[random.Next(Program.Emotes.Count)]);
            }
        }

        public Task GetDefaultEmojis()
        {
            string[] original = new string[] { "👍🏻", "👎🏻", "👌🏻", "😀", "😄", "😅", "😇", "😉", "🙂", "🙃", "😛", "😏", "😐", "😑", "😒", "🙄", "🤔", "😞", "💢", "😫", "😩", "😮", "😨", "😰", "😦", "😢", "😓", "😭", "💤", "😡", "😔", "😕", "🙁", "🆒", "🆗", "🔥", "🚓" };
            List<IEmote> emotes = new List<IEmote>();
            Program.Emotes.AddRange(emotes);
            return Task.CompletedTask;
        }

        private async void Gametimer_ElapsedAsync(object sender, ElapsedEventArgs e)
        {
            if (!Program.Music)
            {
                try
                {
                    string gm = client.GetUser(Program.OwnerID).Activity.Name ?? "XD";
                    await client.SetGameAsync(gm, null, ActivityType.Watching);
                }
                catch { };
            }
        }


    }
}
