using Discord.Addons.Interactive;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CaliBotCore.Functions;
using System.Linq;
using System.Text.RegularExpressions;

namespace CaliBotCore.Commands
{
    public class Waifu : InteractiveBase
    {

        [Command("AddWaifu", RunMode = RunMode.Async)]
        [Summary("WARNING BETA FUNCTION")]
        public async Task AddWaifu(int input)
        {
            if (Context.Message.Author.Id != Program.OwnerID && Program.Users.GetValueOrDefault(Context.Message.Author.Id).lastWaifu == DateTime.Now.ToShortDateString())
            {
                await ReplyAsync("", false, Embed.GetEmbed("**Oops something went wrong**", "You can only do this once per day!"));
                return;
            }
            if (Program.Users.GetValueOrDefault(Context.Message.Author.Id).currency < 100 && Program.Users.GetValueOrDefault(Context.Message.Author.Id).lastWaifu != "")
            {
                await ReplyAsync("", false, Embed.GetEmbed("**Oops something went wrong**", "Looks like you don't have enough credits to use this!"));
                return;
            }

            var result = await Weeb.GetCharacter(input);

            //check to see if taken
            if (Program.Waifus.ContainsKey(input))
            {
                var Owner = Program.Waifus.GetValueOrDefault(input).Owner;
                if (Owner != 0)
                {
                    await ReplyAsync($"Sorry but this waifu has already been taken by {Context.Client.GetUser(Owner).Username}.");
                }
                else
                {
                    Program.Waifus.Remove(input, out Functions.Waifu Temp);
                    Temp.Owner = Context.Message.Author.Id;
                    Program.Waifus.GetOrAdd(input, Temp);
                    await Functions.Waifu.CreateWaifuObject(Temp);
                }
            }
            else
            {
                await Functions.Waifu.CreateWaifuObject(new Functions.Waifu(input, result.Data.Attributes.Name, Context.Message.Author.Id));
            }
            if (Context.Message.Author.Id != Program.OwnerID || Program.Users.GetValueOrDefault(Context.Message.Author.Id).lastWaifu != "")
            {
                Program.Users.GetValueOrDefault(Context.Message.Author.Id).currency = Program.Users.GetValueOrDefault(Context.Message.Author.Id).currency - 100;
            }
            Program.Users.GetValueOrDefault(Context.Message.Author.Id).lastWaifu = DateTime.Now.ToShortDateString();
            await Program.SaveUser(Context.Message.Author.Id);
            await ReplyAsync("Waifu has been successfully added.");
        }

        [Command("RemoveWaifu", RunMode = RunMode.Async)]
        [Summary("WARNING BETA FUNCTION")]
        public async Task RemoveWaifu(int input)
        {
            if (Program.Waifus.ContainsKey(input) && Program.Waifus.GetValueOrDefault(input).Owner == Context.Message.Author.Id)
            {
                Program.Waifus.Remove(input, out Functions.Waifu Temp);
                Temp.Owner = 0;
                Program.Waifus.GetOrAdd(input, Temp);
                await Functions.Waifu.CreateWaifuObject(Temp);
            }
            else
            {
                await ReplyAsync("Error. They are not your Waifu.");
            }

            await ReplyAsync("Waifu has been successfully removed.");
        }

        [Command("RemoveWaifu", RunMode = RunMode.Async)]
        [Summary("WARNING BETA FUNCTION")]
        public async Task RemoveWaifu(string input)
        {
            foreach (var item in Program.Waifus)
            {
                if (item.Value.Name.ToLower() == input.ToLower())
                {
                    if (item.Value.Owner == Context.Message.Author.Id)
                    {
                        Program.Waifus.Remove(item.Value.Id, out Functions.Waifu Temp);
                        Temp.Owner = 0;
                        Program.Waifus.GetOrAdd(item.Value.Id, Temp);
                        await Functions.Waifu.CreateWaifuObject(Temp);
                    }
                    else
                    {
                        await ReplyAsync("Error. They are not your Waifu.");
                    }
                    break;
                }
            }

            await ReplyAsync("Waifu has been successfully removed.");
        }

        [Command("MyWaifu", RunMode = RunMode.Async)]
        [Alias("MyWaifus", "Waifus")]
        [Summary("WARNING BETA FUNCTION")]
        public async Task MyWaifu()
        {
            var List = new List<Functions.Waifu>();
            var Id = Context.Message.Author.Id;
            var IdList = new List<int>();
            foreach (var item in Program.Waifus)
            {
                if (item.Value.Owner == Id)
                {
                    if (item.Value.Owner == Context.Message.Author.Id)
                    {
                        IdList.Add(item.Value.Id);
                    }
                }
            }
            foreach (var item in IdList)
            {
                var tmp = await Weeb.GetCharacter(item);
                var result = tmp.Data.Attributes;
                await ReplyAsync("", false, Embed.GetEmbed($"**{result.Name ?? ""}**", "", result.Image.Original ?? ""));
            }
            if (IdList.Count() == 0)
            {
                await ReplyAsync("Sorry but you don't have any yet.");
            }
        }

        [Command("MyWaifu", RunMode = RunMode.Async)]
        [Alias("MyWaifus", "Waifus")]
        [Summary("WARNING BETA FUNCTION")]
        public async Task MyWaifu([Remainder] string input)
        {
            int id = 0;
            foreach (var item in Program.Waifus)
            {
                if(item.Value.Owner == Context.Message.Author.Id)
                {
                    if(item.Value.Name.ToLower() == input.ToLower())
                    {
                        id = item.Value.Id;
                        break;
                    }
                }
            }
            if (id == 0)
            {
                await ReplyAsync("Error. They are not your Waifu.");
                return;
            }

            var tmp = await Weeb.GetCharacter(id);
            var result = tmp.Data.Attributes;
            int counter = 0;
            foreach (var item in Embed.GetEmbeds($"**{result.Name ?? ""}**", $"{Regex.Replace(Regex.Replace(result.Description ?? "", @"<[^>]+>|&nbsp;", "").Trim(), @"\s{2,}", " ")}", result.Image.Original ?? ""))
            {
                if (counter == 0)
                {
                    await ReplyAsync("", false, item);
                }
                else
                {

                    await ReplyAsync("", false, item);
                }
                counter++;
            }
        }
        
        [Command("RandomWaifu", RunMode = RunMode.Async)]
        [Summary("WARNING BETA FUNCTION")]
        public async Task RandomWaifu()
        {
            var r = new Random();
            while (true)
            {
                try
                {
                    var ran = r.Next(0, 500000);
                    var tmp = await Weeb.GetCharacter(ran);
                    var result = tmp.Data.Attributes;
                    if (result.Name == "")
                    {
                        break;
                    }
                    await ReplyAsync("", false, Embed.GetEmbed($"**{result.Name ?? ""}** ({tmp.Data.Id ?? ""})", $"{Regex.Replace(Regex.Replace(result.Description ?? "", @"<[^>]+>|&nbsp;", "").Trim(), @"\s{2,}", " ")}", result.Image.Original ?? ""));
                    return;
                }
                catch (Exception)
                {
                    //loop
                }
            }
        }
    }
}
