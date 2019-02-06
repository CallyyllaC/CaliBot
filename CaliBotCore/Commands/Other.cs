using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace CaliBotCore.Commands
{
    public class Other : InteractiveBase
    {
        [Command("appreciateresuhai"), Summary("Appreciate Resuhai")]
        public async Task AppreciateResuhai([Remainder] string input = "")
        {
            string urls = "";
            foreach (var item in Context.Message.Attachments)
            {
                urls = urls + " " + item.Url;
            }
            ulong resuhai = 197166960402759680;
            var channel = await Context.Client.GetUser(resuhai).GetOrCreateDMChannelAsync();
            await channel.SendMessageAsync("", false, Embed.GetEmbed($"**You've got mail!**", $"{Context.Message.Author} has appreciated you!\n\n{input} \n {urls}"));
            await ReplyAsync("", false, Embed.GetEmbed($"**Appreciate Resuhai**", "Thankyou for appreciating Resuhai!"));
        }

        [Command("Birthday"), Summary("Happy Birthday!")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task Birthday(SocketUser user)
        {
            Program.Users.GetValueOrDefault(user.Id).currency = Program.Users.GetValueOrDefault(user.Id).currency + 100;
            await Program.SaveUser(user.Id);
            await user.SendMessageAsync("", false, Embed.GetEmbed($"**You've got mail!**", $"Happy Birthday {user.Username}!\nHave a great day!\nAs a birthday gift I have given you 100 Credits!"));
            await ReplyAsync($"**It's {user.Username}'s Birthday.**\nHappy Birthday to you!.\nHappy Birthday to you!.\nHappy Birthday dear {user.Username}!.\nHappy Birthday to you!.", true);
        }

        [Command("R", RunMode = RunMode.Async), Summary("Remind user after x hours")]
        [Alias("Remind", "Reminder")]
        public async Task Remind(Decimal hours, [Remainder] string msg = "")
        {
            int time = Convert.ToInt32(Math.Round((hours * 60) * 60 * 1000));
            //start
            Timer timer = new Timer { Interval = time, AutoReset = false };
            timer.Elapsed += ReminderAsync;
            timer.Start();
            await ReplyAndDeleteAsync("", false, Embed.GetEmbed($"**Reminder**", $"You will be reminded about {msg}, in {hours} hours!"));

            //internal turn off function
            void ReminderAsync(object sender, ElapsedEventArgs e)
            {
                //send messages
                var dm = Context.User.GetOrCreateDMChannelAsync().Result;
                dm.SendMessageAsync("", false, Embed.GetEmbed($"**You've got mail!**", msg));
            }
        }
    }
}
