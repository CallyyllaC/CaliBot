﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;

namespace CaliBotCore.Commands
{
    public class Info : InteractiveBase
    {

        [Command("ListUsersID"), Summary("List users ID's")]
        [Alias("lu", "LstUid")]
        [RequireOwner]
        public async Task LstUid()
        {
            string output = $"Users in Guild: {Context.Guild.Name}\n";
            var users = Context.Guild.Users;
            IDMChannel chn = Program.OwnerChannel;
            foreach (var item in users)
            {
                output = $"{output}\n{item.Username} : {item.Id}\n";
            }
            foreach (var item in Embed.GetEmbeds($"**User ID's**", output, Program.CurrentColour, Program.CurrentName, Context.Guild.IconUrl))
            {
                await chn.SendMessageAsync("", false, item);
            }
        }

        [Command("av"), Summary("Get users avatar")]
        [Alias("avatar")]
        public async Task Avatar(IUser newuser = null)
        {
            var user = newuser ?? Context.Message.Author;
            var embed = Embed.GetEmbed($"**{user.Username}'s Avatar**", "", Program.CurrentColour, Program.CurrentName, Program.CurrentUrl, user.GetAvatarUrl(ImageFormat.Auto,2048));
            await ReplyAsync("", false, embed);
        }

        [Command("Emoji"), Alias("Emote"), Summary("Gets guild emojis")]
        public async Task Emojis(ulong id = 0)
        {
            if (id == 0)
            {
                id = Context.Guild.Id;
            }
            var emotes = Context.Client.GetGuild(id).Emotes;
            string tmp = "";
            foreach (var item in emotes)
            {
                tmp = $"{tmp} {item}";
            }
            foreach (var item in Embed.GetEmbeds($"**User ID's**", tmp, Program.CurrentColour, Program.CurrentName, Context.Guild.IconUrl))
            {
                await ReplyAsync("", false, item);
            }
        }

        [Command("Emoji"), Alias("Emote"), Summary("Get guilds emojis")]
        public async Task Emoji(string input)
        {
            Emote emote = Emote.Parse(input);
            await ReplyAsync(emote.Url);
        }

        [Command("role"), Summary("Get role info")]
        [Alias("roleinfo")]
        public async Task Role(IRole role)
        {
            await ReplyAsync("", false, Embed.GetEmbed($"**{role.Name}'s Info**", $"Name: {role.Name} \nCurrent colour: {role.Color} \nCreated: {role.CreatedAt} \nRole ID: {role.Id} \nIs seperated: {role.IsHoisted} \nIs mentionable: {role.IsMentionable} \nPosition: {role.Position}", Program.CurrentColour, Program.CurrentName, Context.Guild.IconUrl));
        }

        [Command("listroles"), Summary("List all roles")]
        [Alias("list roles", "lstrole")]
        public async Task Role()
        {
            List<string> roleCollection = new List<string>();
            var Roles = Context.Guild.Roles;
            foreach (var item in Roles)
            {
                string tmp = $"{item.Name}: {item.Id} \n";
                roleCollection.Add(tmp);
            }

            await ReplyAsync("", false, Embed.GetEmbed($"**{Context.Guild.Name}'s Roles**", String.Join(String.Empty, roleCollection.ToArray()), Program.CurrentColour, Program.CurrentName, Context.Guild.IconUrl));
        }

        [Command("uid"), Summary("Get id of user")]
        [Alias("id")]
        public async Task Getid(IUser newuser = null)
        {
            var user = newuser ?? Context.Message.Author;
            await ReplyAsync("", false, Embed.GetEmbed($"**{user.Username}**", user.Id.ToString(), Program.CurrentColour, Program.CurrentName, user.GetAvatarUrl(ImageFormat.Auto, 2048)));
        }

        [Command("idu"), Summary("Get user from id")]
        [Alias("u")]
        public async Task Getuser(ulong id)
        {
            var user = Context.Guild.GetUser(id);
            await ReplyAsync("", false, Embed.GetEmbed($"**{user.Nickname}**", $"{user.Username}{user.Discriminator}", Program.CurrentColour, Program.CurrentName, user.GetAvatarUrl(ImageFormat.Auto, 2048)));
        }

        [Command("channel"), Summary("Get channel info")]
        [Alias("channelinfo")]
        public async Task ChannelInfo(ITextChannel channel = null)
        {
            if (channel == null) { channel = Context.Channel as ITextChannel; }
            await ReplyAsync("", false, Embed.GetEmbed($"**{channel.Name}'s Info**", $"**Displaying information about {channel.Name}**\n**Channel ID:** {channel.Id} \n**Created:** {channel.CreatedAt} \n**Is NSFW:** {channel.IsNsfw}", Program.CurrentColour, Program.CurrentName, Context.Guild.IconUrl));
        }

        [Command("user"), Summary("Get user info")]
        [Alias("userinfo", "info")]
        public async Task UserInfo(IUser user = null)
        {
            var userInfo = user ?? Context.Message.Author;

            string playing = "Nothing";
            string playingtype = "doing";
            try
            {
                playing = userInfo.Activity.Name;
                playingtype = userInfo.Activity.Type.ToString();
            }
            catch { }

            await ReplyAsync("", false, Embed.GetEmbed($"**{userInfo.Username}**", $"**Displaying information about {userInfo.Mention}** \n**Username:** {userInfo.Username} \n**User ID:** {userInfo.Id} \n**Users current status:** {userInfo.Status} \n**User is currently {playingtype.ToLower()}:**  {playing} \n**User account was created:** {userInfo.CreatedAt} \n**Is user a bot:** {userInfo.IsBot} \n", Program.CurrentColour, Program.CurrentName, userInfo.GetAvatarUrl(ImageFormat.Auto, 2048)));
        }

        [Command("serverinfo"), Summary("Get server info")]
        [Alias("server", "guildinfo", "guild")]
        public async Task Serverinfo()
        {
            var userInfo = Context.Guild;
            string name = "None";
            try { name = userInfo.AFKChannel.Name; }
            catch { }

            await ReplyAsync("", false, Embed.GetEmbed($"**{userInfo.Name}'s Info**", $"**Server Name: {userInfo.Name} **\n**Server ID:** {userInfo.Id} \n**Server was created:** {userInfo.CreatedAt} \n**Current voice region:** {userInfo.VoiceRegionId} \n**Afk Channel:** {name} **with timeout:** {userInfo.AFKTimeout} \n**Features:**\n {string.Join("\n", userInfo.Features)} \n**Emotes:**\n {string.Join(", ", userInfo.Emotes)} \n**Roles:**\n {string.Join("\n", userInfo.Roles).Replace(@"@everyone", @"Everyone (Default Role)")}", Program.CurrentColour, Program.CurrentName, Context.Guild.IconUrl));
        }

        [Command("CalcLevel"), Summary("Calculate level that xp will give")]
        public async Task CalcLevel(int TotalXP)
        {
            double xp = TotalXP;
            double levelish = (Math.Sqrt(xp)) / 2;
            int level = int.Parse(Math.Floor(levelish).ToString());
            await ReplyAsync("", false, Embed.GetEmbed($"**Calc Level**", level.ToString(), Program.CurrentColour, Program.CurrentName, Program.CurrentUrl));
        }

        [Command("CalcXP"), Summary("Calculate xp that level requires")]
        public async Task CalcXP(int Totallevel)
        {
            var tmp = Totallevel * 2;
            tmp = tmp * tmp;
            await ReplyAsync("", false, Embed.GetEmbed($"**Calc XP**", tmp.ToString(), Program.CurrentColour, Program.CurrentName, Program.CurrentUrl));
        }
    }
}