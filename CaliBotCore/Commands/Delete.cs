using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaliBotCore.Commands
{
    public class Delete : InteractiveBase
    {
        [Command("Del")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [Summary("Delete messages from a user in the last x messages")]
        public async Task Del(IUser user, int input = 10)
        {
            var User = user ?? Context.Message.Author;
            ISocketMessageChannel channel = Context.Channel as ISocketMessageChannel;
            var messages = await channel.GetMessagesAsync(input+1).FlattenAsync();
            var messages2 = new List<IUserMessage>();
            foreach (var item in messages)
            {
                messages2.Add(item as IUserMessage);
            }
            var delete = messages2.Where(x => x.Author.Id == User.Id);
            await ReplyAndDeleteAsync("", false, Embed.GetEmbed($"**Delete Summary**", $"I have deleted {input} of {User.Username}'s messages, as requested by {Context.Message.Author}"), new TimeSpan(0, 0, 30));

            foreach (var item in delete)
            {
                try
                {
                    await item.DeleteAsync();
                }
                catch { }
            }

            try { await Context.Message.DeleteAsync(); }
            catch { }
        }

        [Command("Del")]
        [Summary("Delete your own messages in the last x messages")]
        public async Task Del(int input = 10)
        {
            var User = Context.Message.Author;
            SocketTextChannel channel = Context.Channel as SocketTextChannel;
            var messages = await channel.GetMessagesAsync(input+1).FlattenAsync();
            var messages2 = new List<IUserMessage>();
            foreach (var item in messages)
            {
                messages2.Add(item as IUserMessage);
            }
            var delete = messages2.Where(x => x.Author.Id == User.Id);
            await ReplyAndDeleteAsync("", false, Embed.GetEmbed($"**Delete Summary**", $"I have deleted {input} of {User.Username}'s messages, as requested by {Context.Message.Author}"), new TimeSpan(0, 0, 30));

            foreach (var item in delete)
            {
                try
                {
                    await item.DeleteAsync();
                }
                catch { }
            }
        }

        [Command("Del All")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [Summary("Delete messages from everyone in the last x messages")]
        public async Task DelAll(int input = 10)
        {
            SocketTextChannel channel = Context.Channel as SocketTextChannel;
            var messages = await channel.GetMessagesAsync(input+1).FlattenAsync();
            var messages2 = new List<IUserMessage>();
            foreach (var item in messages)
            {
                messages2.Add(item as IUserMessage);
            }
            await ReplyAndDeleteAsync("", false, Embed.GetEmbed($"**Delete Summary**", $"I have deleted {input} messages, as requested by {Context.Message.Author}"), new TimeSpan(0, 0, 30));

            foreach (var item in messages2)
            {
                try
                {
                    await item.DeleteAsync();
                }
                catch { }
            }
        }
    }
}
