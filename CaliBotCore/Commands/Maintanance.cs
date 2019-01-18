using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using CaliBotCore.DataStructures;
using System.Linq;
using CaliBotCore.Images;

namespace CaliBotCore.Commands
{
    public class Maintanance : InteractiveBase
    {
        [Command("Restart", RunMode = RunMode.Async), Summary("Restart the bot")]
        [RequireOwner]
        public async Task Restart()
        {
            var message = await ReplyAsync("", false, Embed.GetEmbed("**Bot Restart**", $"BRB!"));
            Program.Bot_Properties.restart = true;
            Program.Bot_Properties.guildId = Context.Guild.Id;
            Program.Bot_Properties.channelId = Context.Channel.Id;
            Program.Bot_Properties.messageId = message.Id;
            Updater.Save(Program.Bot_Properties);
            Environment.Exit(60000);
        }

        [Command("Update", RunMode = RunMode.Async), Summary("Update the bot")]
        [RequireOwner]
        public async Task Update(string LatestVersion)
        {
            var message = await ReplyAsync("", false, Embed.GetEmbed("**Bot Update**", $"Downloading the files!, Won't be long!"));

            string url = $"https://github.com/{Program.OwnerGithub}/{Program.BotRepo}/releases/download/{LatestVersion}/Release.zip";
            string path = $"Release.zip";
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            using (var client = new System.Net.Http.HttpClient())
            {
                var contents = client.GetByteArrayAsync(url).Result;
                File.WriteAllBytes(path, contents);
            }
            Program.Bot_Properties.updated = true;
            Program.Bot_Properties.guildId = Context.Guild.Id;
            Program.Bot_Properties.channelId = Context.Channel.Id;
            Program.Bot_Properties.messageId = message.Id;
            Updater.Save(Program.Bot_Properties);

            Program.Ver = LatestVersion;
            Config.Config.Save();

            await message.ModifyAsync(x => x.Embed = Embed.GetEmbed("**Bot Update**", $"There we go, brb!"));
            Environment.Exit(60001);
        }

        [Command("AddAvatar", RunMode = RunMode.Async), Summary("Add a new persona to the bot")]
        [Alias("ChangeAvatar")]
        [RequireOwner]
        public async Task Changeavatar(string PersonaName)
        {
            var img = Context.Message.Attachments.ElementAt(0);
            File.WriteAllBytes($"{Program.Rootdir}\\Personas\\{PersonaName}.png", await GetFromWeb.PullImageFromWebAsync(img.ProxyUrl));
            Discord.Image img2 = new Discord.Image($"{Program.Rootdir}\\Personas\\{PersonaName}.png");
            Program.persona.AddAsync(new string[] { PersonaName, $"{Program.Rootdir}\\Personas\\{PersonaName}.png" });
            try
            {
                await Context.Client.CurrentUser.ModifyAsync(x =>
                {
                    x.Avatar = img2;
                    x.Username = PersonaName;
                });
                await ReplyAndDeleteAsync("", false, Embed.GetEmbed("**Avatar**", $"Avatar changed!"), new TimeSpan(0, 0, 15));
            }
            catch { await ReplyAndDeleteAsync("", false, Embed.GetEmbed("**Avatar**", $"Avatar added but could not be changed right now!"), new TimeSpan(0, 0, 15)); }
            img = null;
        }

        [Command("Removeavatar", RunMode = RunMode.Async), Summary("Remove a persona from the bot")]
        [Alias("Deleteavatar")]
        [RequireOwner]
        public async Task Deleteavatar(string PersonaName)
        {
            Program.persona.RemoveAsync(PersonaName);
            await ReplyAndDeleteAsync("", false, Embed.GetEmbed("**Avatar**", $"Avatar removed!"), new TimeSpan(0, 0, 15));
        }
        
        [Command("ChangeLog", RunMode = RunMode.Async), Summary("Get the ChangeLog")]
        public async Task ChangeLog()
        {
            using (Stream strem = new FileStream("//Bot//ChangeLog.txt", FileMode.Open))
            {
                await Context.Channel.SendFileAsync(strem, "ChangeLog.txt");
            }
        }


    }
}
