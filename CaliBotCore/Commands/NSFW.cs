using CaliBotCore.Functions;
using Discord.Addons.Interactive;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CaliBotCore.Commands
{
    public class NSFW : InteractiveBase
    {
        [Command("Gel")]
        [RequireNsfw]
        [Summary("Search Gelbooru for given tags")]
        public async Task Gel([Remainder]string input)
        {
            if (Program.Users.GetValueOrDefault(Context.Message.Author.Id).currency < 5)
            {
                await ReplyAsync("", false, Embed.GetEmbed("**Oops something went wrong**", "Looks like you don't have enough credits to use this!"));
                return;
            }
            var tmp = await NotSafeForWork.GetGelImageAsync(input);
            if (tmp == null)
            {
                await ReplyAsync("", false, Embed.GetEmbed("**Oops something went wrong**", "Could not find any results for this tag"));
                return;
            }

            if (Context.Message.Author.Id != Program.OwnerID)
            {
                Program.Users.GetValueOrDefault(Context.Message.Author.Id).currency = Program.Users.GetValueOrDefault(Context.Message.Author.Id).currency - 5;
                await Program.SaveUser(Context.Message.Author.Id);
            }

            await ReplyAsync($"http://gelbooru.com/index.php?page=post&s=view&id={tmp.Id}", false, Embed.GetEmbed($"**Gelbooru**", $"Tags: {tmp.Tags}\nRating: {tmp.Rating}\nScore: {tmp.Score}", await Program.GetUserColour(Context.Message.Author.Id), tmp.Id.ToString(), null, tmp.FileUrl));
        }

        [Command("Yan")]
        [RequireNsfw]
        [Summary("Search Yandere for given tags")]
        public async Task Yan([Remainder]string input)
        {
            if (Program.Users.GetValueOrDefault(Context.Message.Author.Id).currency < 5)
            {
                await ReplyAsync("", false, Embed.GetEmbed("**Oops something went wrong**", "Looks like you don't have enough credits to use this!"));
                return;
            }
            var tmp = await NotSafeForWork.GetYanImageAsync(input);
            if (tmp == null)
            {
                await ReplyAsync("", false, Embed.GetEmbed("**Oops something went wrong**", "Could not find any results for this tag"));
                return;
            }

            if (Context.Message.Author.Id != Program.OwnerID)
            {
                Program.Users.GetValueOrDefault(Context.Message.Author.Id).currency = Program.Users.GetValueOrDefault(Context.Message.Author.Id).currency - 5;
                await Program.SaveUser(Context.Message.Author.Id);
            }

            await ReplyAsync("", false, Embed.GetEmbed($"**Yande.re**", $"Tags: {tmp.Tags}\nRating: {tmp.Rating}\nScore: {tmp.Score}", await Program.GetUserColour(Context.Message.Author.Id), tmp.Id.ToString(), null, tmp.FileUrl));
        }

        [Command("NSFW")]
        [RequireNsfw]
        [Summary("Search both for given tags")]
        public async Task Nsfw([Remainder]string input)
        {
            if (Program.Users.GetValueOrDefault(Context.Message.Author.Id).currency < 8)
            {
                await ReplyAsync("", false, Embed.GetEmbed("**Oops something went wrong**", "Looks like you don't have enough credits to use this!"));
                return;
            }
            var tmp = await NotSafeForWork.GetYanImageAsync(input);
            if (tmp != null)
            {
                await ReplyAsync("", false, Embed.GetEmbed($"**Yande.re**", $"Tags: {tmp.Tags}\nRating: {tmp.Rating}\nScore: {tmp.Score}", await Program.GetUserColour(Context.Message.Author.Id), tmp.Id.ToString(), null, tmp.FileUrl));
            }
            else
            {
                await ReplyAsync("", false, Embed.GetEmbed("**Oops something went wrong**", "Could not find any results for this tag"));
            }
            if (Context.Message.Author.Id != Program.OwnerID)
            {
                Program.Users.GetValueOrDefault(Context.Message.Author.Id).currency = Program.Users.GetValueOrDefault(Context.Message.Author.Id).currency - 5;
                await Program.SaveUser(Context.Message.Author.Id);
            }
            var tmp2 = await NotSafeForWork.GetGelImageAsync(input);
            if (tmp2 == null)
            {
                await ReplyAsync("", false, Embed.GetEmbed("**Oops something went wrong**", "Could not find any results for this tag"));
                return;
            }
            await ReplyAsync("", false, Embed.GetEmbed($"**Gelbooru**", $"Tags: {tmp2.Tags}\nRating: {tmp2.Rating}\nScore: {tmp2.Score}", await Program.GetUserColour(Context.Message.Author.Id), tmp2.Id.ToString(), null, tmp2.FileUrl));
        }
    }
}
