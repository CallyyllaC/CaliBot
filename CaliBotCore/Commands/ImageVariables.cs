using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaliBotCore.Commands
{
    public class ImageVariables : InteractiveBase
    {
        [Command("GetMultiplier", RunMode = RunMode.Async)]
        [Summary("Get font multiplier")]
        public async Task MyMultiplier()
        {
            IUser user = Context.Message.Author;
            await ReplyAsync("", false, Embed.GetEmbed($"**Get Multiplier**", $"{ Program.Users.GetValueOrDefault(user.Id).fontMultiplier}", await Program.GetUserColour(user.Id), Program.CurrentName, Program.CurrentUrl));
        }

        [Command("SetMultiplier", RunMode = RunMode.Async)]
        [Summary("Set font multiplier")]
        public async Task Multiplier(float multi)
        {
            IUser user = Context.Message.Author;
            var myuser = Program.Users.GetValueOrDefault(user.Id);
            myuser.fontMultiplier = multi;
            myuser.changed = true;
            await Program.EditUser(myuser);
            await ReplyAsync("", false, Embed.GetEmbed($"**Set Multiplier**", "Done!", await Program.GetUserColour(user.Id), Program.CurrentName, Program.CurrentUrl));
        }

        [Command("GetColour", RunMode = RunMode.Async)]
        [Summary("Get colour of font")]
        public async Task MyColour()
        {
            IUser user = Context.Message.Author;
            await ReplyAsync("", false, Embed.GetEmbed($"**Get Colour**", Program.Users.GetValueOrDefault(user.Id).hexcol, await Program.GetUserColour(user.Id), Program.CurrentName, Program.CurrentUrl));
        }

        [Command("SetColour", RunMode = RunMode.Async)]
        [Summary("Set colour of font")]
        public async Task Colour(string HexValue)
        {
            IUser user = Context.Message.Author;

            try
            {
                Color tmp = new Color(Convert.ToUInt32(HexValue.Replace("#",""),16));
            }
            catch
            {
                await ReplyAndDeleteAsync("", false, Embed.GetEmbed($"**Set Colour**", $"Error! Could not convert uint into a colour", await Program.GetUserColour(user.Id), Program.CurrentName, Program.CurrentUrl), new TimeSpan(0, 0, 30));
                return;
            }

            Program.Users.GetValueOrDefault(user.Id).hexcol = HexValue;
            Program.Users.GetValueOrDefault(user.Id).changed = true;
            await Program.SaveUser(Context.Message.Author.Id);
            await ReplyAndDeleteAsync("", false, Embed.GetEmbed($"**Set Colour**", $"Done!", await Program.GetUserColour(user.Id), Program.CurrentName, Program.CurrentUrl), new TimeSpan(0, 0, 30));

        }
        
        [Command("GetFont", RunMode = RunMode.Async)]
        [Summary("Get font")]
        public async Task MyFont()
        {
            IUser user = Context.Message.Author;
            if (Program.Users.GetValueOrDefault(user.Id).font == null)
            {
                await ReplyAsync("", false, Embed.GetEmbed($"**Get Font**", "You have no saved font!", await Program.GetUserColour(user.Id), Program.CurrentName, Program.CurrentUrl));
            }
            else
            {
                await ReplyAsync("", false, Embed.GetEmbed($"**Get Font**", Program.Users.GetValueOrDefault(user.Id).font, await Program.GetUserColour(user.Id), Program.CurrentName, Program.CurrentUrl));
            }
        }

        [Command("SetFont", RunMode = RunMode.Async)]
        [Summary("Set font")]
        public async Task Font()
        {
            var fonts = Program.Myfonts.Fonts.Families.AsEnumerable();
            IUser user = Context.Message.Author;

            string msg = "*Please reply with the following int:*\n";
            int cnt = 0;
            foreach (var item in fonts)
            {
                msg = msg + "`" + cnt + ". " + item.Name + "`\n";
                cnt++;
            }
            int num = 0;

            await ReplyAndDeleteAsync("", false, Embed.GetEmbed($"**{Program.CurrentName}, please select a font:**", msg, await Program.GetUserColour(user.Id), Program.CurrentName, Program.CurrentUrl), TimeSpan.FromMinutes(1));

            try
            {
                var response = await NextMessageAsync(true, true, TimeSpan.FromMinutes(1));
                num = int.Parse(response.Content);
            }
            catch
            {
                await ReplyAndDeleteAsync("", false, Embed.GetEmbed($"**Set Font**", "You didnt reply before timeout or inputed an invalid number", await Program.GetUserColour(user.Id), Program.CurrentName, Program.CurrentUrl));
                return;
            }

            Program.Users.GetValueOrDefault(user.Id).font = fonts.ElementAt(num).Name;
            Program.Users.GetValueOrDefault(user.Id).changed = true;
            await Program.SaveUser(Context.Message.Author.Id);
            await ReplyAndDeleteAsync("", false, Embed.GetEmbed($"**Set Font**", $"Done!", await Program.GetUserColour(user.Id), Program.CurrentName, Program.CurrentUrl), new TimeSpan(0, 0, 30));
        }

        [Command("SampleFonts", RunMode = RunMode.Async), Summary("Post samples of installed fonts")]
        public async Task SampleFonts()
        {
            foreach (var item in Program.Myfonts.Fonts.Families)
            {
                if (!File.Exists($"{Program.Rootdir}\\Resources\\Fonts\\{item.Name}.png"))
                {
                    await Images.Profile.WriteText(item);
                    await Context.Channel.SendFileAsync($"{Program.Rootdir}\\Resources\\Fonts\\{item.Name}.png", item.Name);
                }
                else
                {
                    await Context.Channel.SendFileAsync($"{Program.Rootdir}\\Resources\\Fonts\\{item.Name}.png", item.Name);
                }
            }
        }
    }
}

