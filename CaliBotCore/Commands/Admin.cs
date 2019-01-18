using CaliBotCore.Config;
using CaliBotCore.DataStructures;
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
    [Group("Blacklist")]
    public class Blacklist : InteractiveBase
    {
        [Command("User", RunMode = RunMode.Async), Summary("Blacklist a user")]
        [RequireOwner]
        public async Task BlacklistUser(ulong ID)
        {
            try
            {
                if (!Program.Users.TryGetValue(ID, out User user))
                {
                    await ReplyAsync("", false, Embed.GetEmbed("**Blacklist**", $"Unable to find user!"));
                    return;
                }
                else
                {
                    if (user.Blacklisted)
                    {
                        user.Blacklisted = false;
                        await Program.EditUser(user);
                        await ReplyAsync("", false, Embed.GetEmbed("**Blacklist**", $"User has been unblacklisted!"));
                    }
                    else
                    {
                        user.Blacklisted = true;
                        await Program.EditUser(user);
                        await ReplyAsync("", false, Embed.GetEmbed("**Blacklist**", $"User has been blacklisted!"));
                    }
                }
            }
            catch (Exception e)
            {
                await ReplyAsync("", false, Embed.GetEmbed("**Blacklist**", e.Message));
                return;
            }
        }

        [Command("User", RunMode = RunMode.Async), Summary("Blacklist a user")]
        [RequireOwner]
        public async Task BlacklistUser(IUser UserTag)
        {
            try
            {
                if (!Program.Users.TryGetValue(UserTag.Id, out User user))
                {
                    await ReplyAsync("", false, Embed.GetEmbed("**Blacklist**", $"Unable to find user!"));
                    return;
                }
                else
                {
                    if (user.Blacklisted)
                    {
                        user.Blacklisted = false;
                        await Program.EditUser(user);
                        await ReplyAsync("", false, Embed.GetEmbed("**Blacklist**", $"User has been unblacklisted!"));
                    }
                    else
                    {
                        user.Blacklisted = true;
                        await Program.EditUser(user);
                        await ReplyAsync("", false, Embed.GetEmbed("**Blacklist**", $"User has been blacklisted!"));
                    }
                }
            }
            catch (Exception e)
            {
                await ReplyAsync("", false, Embed.GetEmbed("**Blacklist**", e.Message));
                return;
            }
        }

        [Command("Guild", RunMode = RunMode.Async), Summary("Blacklist a guild")]
        [RequireOwner]
        public async Task BlacklistGuild(ulong ID)
        {
            try
            {
                if (!Program.Users.TryGetValue(ID, out User user))
                {
                    await ReplyAsync("", false, Embed.GetEmbed("**Blacklist**", $"Unable to find guild!"));
                    return;
                }
                else
                {
                    if (user.Blacklisted)
                    {
                        user.Blacklisted = false;
                        await Program.EditUser(user);
                        await ReplyAsync("", false, Embed.GetEmbed("**Blacklist**", $"Guild has been unblacklisted!"));
                    }
                    else
                    {
                        user.Blacklisted = true;
                        await Program.EditUser(user);
                        await ReplyAsync("", false, Embed.GetEmbed("**Blacklist**", $"Guild has been blacklisted!"));
                    }
                }
            }
            catch (Exception e)
            {
                await ReplyAsync("", false, Embed.GetEmbed("**Blacklist**", e.Message));
                return;
            }
        }
    }

    public class Admin : InteractiveBase
    {
        [Command("SendMessage", RunMode = RunMode.Async), Summary("Send a DM via the bot")]
        [Alias("msg")]
        [RequireOwner]
        public async Task SendMessage(ulong UID, [Remainder] string MessageContents)
        {
            IDMChannel chn = null;
            IUser user = null;
            MessageContents = MessageContents.Trim();
            user = Context.Client.GetUser(UID);
            chn = await user.GetOrCreateDMChannelAsync();

            await chn.SendMessageAsync(MessageContents);
            var embed = Embed.GetEmbed($"**Sent to {user.Username}**", MessageContents, Program.CurrentColour, Program.CurrentName, user.GetAvatarUrl());
            await ReplyAsync("", false, embed);

            if (Context.Message.Attachments.Count > 0)
            {
                foreach (var item in Context.Message.Attachments)
                {
                    await ReplyAsync(item.ProxyUrl);
                    await chn.SendMessageAsync(item.ProxyUrl);
                }
            }
        }

        [Command("SetPrefix", RunMode = RunMode.Async), Summary("Set the current guilds prefix")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task SetPrefix(char Prefix)
        {
            var guild = Program.Guilds.GetValueOrDefault(Context.Guild.Id);
            guild.Prefix = Prefix;
            await Program.EditGuild(guild);

            await ReplyAsync("", false, Embed.GetEmbed($"**Set Preftix**", $"Prefix has been changed to '{Prefix}'!"));
        }

        [Command("SetLevelChannel", RunMode = RunMode.Async), Summary("Set a channel to post levelups")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task SetLevelChannel(ITextChannel Channel)
        {
            var guild = Program.Guilds.GetValueOrDefault(Context.Guild.Id);
            guild.LevelupChannel = Channel.Id;
            await Program.EditGuild(guild);
            await ReplyAsync("", false, Embed.GetEmbed($"**Set Leveling Channel**", $"Channel has been changed to {Channel.Name}!"));
        }

        [Command("Rainbow", RunMode = RunMode.Async), Summary("Give user rainbow role")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task Rainbow(IGuildUser User)
        {
            var guildsRainbow = Context.Guild.Roles.Where(X => X.Name.ToLower() == "rainbow").First();
            if (User.RoleIds.Contains(guildsRainbow.Id))
            {
                await User.RemoveRoleAsync(guildsRainbow);
                await ReplyAsync("", false, Embed.GetEmbed("**Rainbow**", $"Removed from user!"));
            }
            else
            {
                await User.AddRoleAsync(guildsRainbow);
                await ReplyAsync("", false, Embed.GetEmbed("**Rainbow**", $"Added to user!"));
            }
        }

        [Command("SetLevelChannel", RunMode = RunMode.Async), Summary("Send \"None\" string to remove channel")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task SetLevelChannel(string text)
        {
            if (text.ToLower() != "none") { return; }
            var guild = Program.Guilds.GetValueOrDefault(Context.Guild.Id);
            guild.LevelupChannel = 0;
            await Program.EditGuild(guild);
            await ReplyAsync("", false, Embed.GetEmbed($"**Set Leveling Channel**", $"Channel has been removed!"));
        }

        [Command("DefaultRole", RunMode = RunMode.Async), Summary("Set a channel to post levelups")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task DefaultRole(IRole Role)
        {
            var guild = Program.Guilds.GetValueOrDefault(Context.Guild.Id);
            var List = guild.DefaultRole.ToList();
            List.Add(Role.Id);
            guild.DefaultRole = List.ToArray();
            await Program.EditGuild(guild);
            await ReplyAsync("", false, Embed.GetEmbed($"**Auto Role**", $"Added {Role.Name} to the AutoRole system!"));
        }

        [Command("Music", RunMode = RunMode.Async), Summary("Enables use of music")]
        [RequireOwner]
        public async Task Music()
        {
            var guild = Program.Guilds.GetValueOrDefault(Context.Guild.Id);
            if (guild.Music)
            {
                guild.Music = false;
                await ReplyAsync("", false, Embed.GetEmbed($"**Music**", $"Music has been disabled on this server!"));
            }
            else
            {
                guild.Music = true;
                await ReplyAsync("", false, Embed.GetEmbed($"**Music**", $"Music has been enabled on this server!"));
            }
            await Program.EditGuild(guild);
        }

        [Command("Activity", RunMode = RunMode.Async), Summary("Toggle guild activity")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task Activity()
        {
            var guild = Program.Guilds.GetValueOrDefault(Context.Guild.Id);
            if (guild.Activity)
            {
                guild.Activity = false;
                await ReplyAsync("", false, Embed.GetEmbed($"**Activity**", $"Activity has been disabled on this server!"));
            }
            else
            {
                guild.Activity = true;
                await ReplyAsync("", false, Embed.GetEmbed($"**Activity**", $"Activity has been enabled on this server!"));
            }
            await Program.EditGuild(guild);
        }

        [Command("Jail", RunMode = RunMode.Async), Summary("Send user to jail")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task Jail(IGuildUser user, int hours = 1)
        {
            if (Program.jail.Contains(user))
            {
                await ReplyAndDeleteAsync("", false, Embed.GetEmbed($"**Jail**", $"User is already in jail!"));
                return;
            }
            Program.jail.Add(user);

            await Context.Channel.SendFileAsync($"{Program.Rootdir}\\Resources\\GotoJail.jpg", $"{user.Mention} you are now in jail for {hours} hours");

            System.Threading.Thread.Sleep(hours * 60 * 60 * 1000);

            Program.jail.Remove(user);

        }

        [Command("Jail", RunMode = RunMode.Async), Summary("Remove user from jail")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task Jail(IGuildUser user)
        {
            if (!Program.jail.Contains(user))
            {
                await ReplyAndDeleteAsync("", false, Embed.GetEmbed($"**Jail**", $"User is not in jail!"));
                return;
            }

            Program.jail.Remove(user);

            await ReplyAndDeleteAsync("", false, Embed.GetEmbed($"**Jail**", $"User has been removed from jail!"));
        }

        [Command("Jail", RunMode = RunMode.Async), Summary("Jail List")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task Jail()
        {
            string message = "";
            foreach (var item in Program.jail)
            {
                message = message + "\n" + item.Mention;
            }
            await ReplyAsync("", false, Embed.GetEmbed("Inmates", message));
        }

        [Command("AddCredits", RunMode = RunMode.Async), Summary("Add credits to user")]
        [RequireOwner]
        public async Task AddCredits(IUser IUser, int input)
        {
            var user = Program.Users.GetValueOrDefault(IUser.Id);
            user.currency = user.currency + input;
            await ReplyAsync("", false, Embed.GetEmbed($"**Credits**", $"{input} credits have been added to {IUser.Username}!"));
            await Program.EditUser(user);
        }

        [Command("RemoveCredits", RunMode = RunMode.Async), Summary("Remove credits to user")]
        [RequireOwner]
        public async Task RemoveCredits(IUser IUser, int input)
        {
            var user = Program.Users.GetValueOrDefault(IUser.Id);
            user.currency = user.currency - input;
            await ReplyAsync("", false, Embed.GetEmbed($"**Credits**", $"{input} credits have been removed from {IUser.Username}!"));
            await Program.EditUser(user);
        }

        [Command("Nep", RunMode = RunMode.Async), Summary("Change Nepmoji")]
        [RequireOwner]
        public async Task Nep(IEmote tmp)
        {
            Program.nepmote = tmp.ToString();
            Config.Config.Save();
            await ReplyAsync("", false, Embed.GetEmbed($"**Credits**", $"{input} credits have been removed from {IUser.Username}!"));
        }

    }
}
