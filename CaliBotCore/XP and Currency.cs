using CaliBotCore;
using CaliBotCore.DataStructures;
using CaliBotCore.Functions;
using CaliBotCore.Images;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CaliBotCore
{
    class XP
    {
        public static async void AddXPAsync(SocketMessage message)
        {
            var id = message.Author.Id;
            foreach (var item in Program.Users)
            {
                if (item.Key == id && item.Value.xpcooldown <= 10)
                {
                    var User = item.Value;
                    User.xp++;
                    User.xpcooldown++;
                    var tmp = CheckLevelAsync(User.xp).Result;
                    if (User.level < tmp)
                    {
                        LevelUpAsync(message, User, tmp);
                        User.level = tmp;
                    }
                    await Program.EditUser(User);
                    return;
                }
            }
        }

        private static async void LevelUpAsync(SocketMessage message, User user, int level)
        {
            SocketTextChannel channel = message.Channel as SocketTextChannel;;
            if (Program.Guilds.GetValueOrDefault(channel.Guild.Id).LevelupChannel != 0)
            {
                channel = Program.Client.GetChannel(Program.Guilds.GetValueOrDefault(channel.Guild.Id).LevelupChannel) as SocketTextChannel;
            }
            if (!File.Exists($"{Program.Rootdir}\\Users\\LevelUp\\{user.Id}.gif"))
            {
                try { await channel.SendMessageAsync("", false, Embed.GetEmbed($"Congratulations {message.Author.Username} you are now level {level}!", "You havn't uploaded a levelup image yet")); }
                catch { await channel.SendMessageAsync("", false, Embed.GetEmbed("Get Levelup", "Sorry there was an error")); }
                return;
            }

            var tmp = await channel.SendMessageAsync("", false, Embed.GetEmbed($"Congratulations {message.Author.Username} you are now level {level}!", "Loading image..."));

            try
            {
                int globalRank = await Ranks.GetGlobalRank(message.Author.Id);
                int localRank = await Ranks.GetLocalRank(channel.Guild, message.Author.Id);

                if (!File.Exists($"{Program.Rootdir}\\Users\\LevelUp\\{user.Id}Processed.gif") || Program.Users.GetValueOrDefault(user.Id).changed == true)
                {
                    var tmpimg = await LevelUp.MakeLevelAsync(user.Id);

                    if (tmpimg.Length > 8000000)
                    {
                        await tmp.ModifyAsync(x => x.Embed = Embed.GetEmbed($"Congratulations {message.Author.Username} you are now level {level}!", "Levelup image is too big for discord after processing"));
                        GC.Collect();
                        return;
                    }
                    else
                    {
                        File.WriteAllBytes($"{Program.Rootdir}\\Users\\LevelUp\\{message.Author.Id}Processed.gif", tmpimg);
                        Program.Users.GetValueOrDefault(message.Author.Id).changed = false;
                        await Program.SaveUser(user.Id);
                    }
                }
                try
                {
                    await channel.SendFileAsync($"{Program.Rootdir}\\Users\\LevelUp\\{user.Id}Processed.gif", "", false, Embed.GetEmbed($"Congratulations {message.Author} you are now level {level}!", $"You currently rank:\nGuild: {localRank}\nOverall: {globalRank}"));
                }
                catch
                {
                    await tmp.ModifyAsync(x => x.Embed = Embed.GetEmbed("Get Levelup", "Sorry there was an error"));
                    GC.Collect();
                    return;
                }
                await tmp.DeleteAsync();
            }
            catch
            {
                await tmp.ModifyAsync(x => x.Embed = Embed.GetEmbed($"Congratulations {message.Author.Username} you are now level {level}!", "Sorry there was an error"));
                GC.Collect();
                return;
            }
        }

        private static async Task<int> CheckLevelAsync(double TotalXP)
        {
            double xp = TotalXP;
            double levelish = (Math.Sqrt(xp)) / 2;
            int level = int.Parse(Math.Floor(levelish).ToString());
            await Task.CompletedTask;
            return level;
        }

    }
}
class Currency
{
    private static Random random = new Random();
    public static async void AddCurrencyAsync(ulong id)
    {
        foreach (var item in Program.Users)
        {
            if (item.Key == id && item.Value.doshCooldown <= 1 && random.Next(5) == 1)
            {
                item.Value.currency++;
                item.Value.doshCooldown++;
                await Program.SaveUser(id);
            }
        }
    }
}
