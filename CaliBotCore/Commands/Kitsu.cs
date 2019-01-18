using CaliBotCore.Functions;
using Discord.Addons.Interactive;
using Discord.Commands;
using Kitsu.Anime;
using Kitsu.Manga;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CaliBotCore.Commands
{
    public class Kitsu : InteractiveBase
    {

        [Command("Anime", RunMode = RunMode.Async)]
        [Summary("Search Kitsu for an anime or trending")]
        public async Task Anime([Remainder]string input)
        {
            List<AnimeDataModel> results;
            if (input == null)
            {
                results = await Weeb.GetAnimeTrending();

                string msg = "";
                int cnt = 1;
                foreach (var item in results)
                {
                    msg = msg + cnt + ". " + item.Attributes.Titles.EnJp + "\n";
                    cnt++;
                }
                await ReplyAsync("", false, Embed.GetEmbed($"**Currently Trending**", msg));
            }
            else
            {
                input = input.Replace(' ', '-');
                results = await Weeb.GetAnime(input);

                string msg = "*Please reply with the following int:*\n";
                int cnt = 1;
                foreach (var item in results)
                {
                    msg = msg + "`" + cnt + ". " + item.Attributes.Titles.EnJp + "`\n";
                    cnt++;
                }
                int num = 0;

                await ReplyAndDeleteAsync("", false, Embed.GetEmbed($"**Please select a result**", msg), TimeSpan.FromMinutes(1));

                try
                {
                    var response = await NextMessageAsync(true, true, TimeSpan.FromMinutes(1));
                    num = int.Parse(response.Content) - 1;
                }
                catch
                {
                    await ReplyAndDeleteAsync("", false, Embed.GetEmbed($"**Oops something went wrong**", "You didnt reply before timeout or inputed an invalid number"));
                    return;
                }

                var tmp = results[num];
                var tmp1 = tmp.Attributes;
                int counter = 0;
                foreach (var item in Embed.GetEmbeds($"**{tmp1.Titles.EnJp}**", Regex.Replace(Regex.Replace($"Synonyms: {string.Join(", ", tmp1.AbbreviatedTitles ?? Enumerable.Empty<string>())}\nScore: {tmp1.AverageRating}\nAge: {tmp1.AgeRating}\nStatus: {tmp1.Status}\nEpisodes: {tmp1.EpisodeCount.ToString() ?? "Unknown"}\nRuntime: {tmp1.StartDate} - {tmp1.EndDate}\n{tmp1.Synopsis}", @"<[^>]+>|&nbsp;", "").Trim(), @"\s{2,}", " "), tmp1.PosterImage.Original ?? null))
                {
                    if (counter == 0)
                    {
                        await ReplyAsync($"https://kitsu.io/anime/{tmp.Id ?? "error"}", false, item);
                    }
                    else
                    {

                        await ReplyAsync($"", false, item);
                    }
                }
            }
        }

        [Command("Manga", RunMode = RunMode.Async)]
        [Summary("Search Kitsu for a manga or trending")]
        public async Task Manga([Remainder]string input)
        {


            List<MangaDataModel> results;
            if (input == null)
            {
                results = await Weeb.GetMangaTrending();

                string msg = "";
                int cnt = 1;
                foreach (var item in results)
                {
                    msg = msg + cnt + ". " + item.Attributes.Titles.EnJp + "\n";
                    cnt++;
                }
                await ReplyAsync("", false, Embed.GetEmbed($"**Currently Trending**", msg));
            }
            else
            {
                input = input.Replace(' ', '-');
                results = await Weeb.GetManga(input);

                string msg = "*Please reply with the following int:*\n";
                int cnt = 1;
                foreach (var item in results)
                {
                    msg = msg + "`" + cnt + ". " + item.Attributes.Titles.EnJp + "`\n";
                    cnt++;
                }
                int num = 0;

                await ReplyAndDeleteAsync("", false, Embed.GetEmbed($"**Please select a result**", msg), TimeSpan.FromMinutes(1));

                try
                {
                    var response = await NextMessageAsync(true, true, TimeSpan.FromMinutes(1));
                    num = int.Parse(response.Content) - 1;
                }
                catch
                {
                    await ReplyAndDeleteAsync("", false, Embed.GetEmbed($"**Oops something went wrong**", "You didnt reply before timeout or inputed an invalid number"));
                    return;
                }

                var tmp1 = results[num];
                var tmp = tmp1.Attributes;
                int counter = 0;
                foreach (var item in Embed.GetEmbeds($"**{tmp.Titles.EnJp}**", Regex.Replace(Regex.Replace($"Synonyms: {string.Join(", ", tmp.AbbreviatedTitles ?? Enumerable.Empty<string>())}\nScore: {tmp.AverageRating}\nAge: {tmp.AgeRating}\nStatus: {tmp.Status}\nType: {tmp.MangaType}\nChapters: {tmp.ChapterCount.ToString() ?? "Unknown" }\nVolumes: {tmp.VolumeCount.ToString() ?? "Unknown"}\nRuntime: {tmp.StartDate} - {tmp.EndDate}\n{tmp.Synopsis}", @"<[^>]+>|&nbsp;", "").Trim(), @"\s{2,}", " "), tmp.PosterImage.Original ?? null))
                {

                    if (counter == 0)
                    {
                        await ReplyAsync($"https://kitsu.io/manga/{tmp1.Id ?? "error"}", false, item);
                    }
                    else
                    {

                        await ReplyAsync($"", false, item);
                    }
                }

            }
        }

        [Command("Character", RunMode = RunMode.Async)]
        [Summary("Search Kitsu for a character")]
        public async Task Character([Remainder]string input)
        {
            input = input.Replace(' ', '-');
            var results = await Weeb.GetCharacter(input);

            string msg = "*Please reply with the following int:*\n";
            int cnt = 1;
            foreach (var item in results)
            {
                msg = msg + "`" + cnt + ". " + item.Attributes.Name + "`\n";
                cnt++;
            }
            int num = 0;

            await ReplyAndDeleteAsync("", false, Embed.GetEmbed($"**Please select a result**", msg), TimeSpan.FromMinutes(1));

            try
            {
                var response = await NextMessageAsync(true, true, TimeSpan.FromMinutes(1));
                num = int.Parse(response.Content) - 1;
            }
            catch
            {
                await ReplyAndDeleteAsync("", false, Embed.GetEmbed($"**Oops something went wrong**", "You didnt reply before timeout or inputed an invalid number"));
                return;
            }

            var tmp1 = results[num];

            var tmp = tmp1.Attributes;
            int counter = 0;
            var dump = Embed.GetEmbeds($"**{tmp.Name ?? ""}** ({tmp1.Id ?? ""})", $"{Regex.Replace(Regex.Replace(tmp.Description ?? "", @"<[^>]+>|&nbsp;", "").Trim(), @"\s{2,}", " ")}", tmp.Image.Original ?? "");
            foreach (var item in dump)
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


        [Command("Character", RunMode = RunMode.Async)]
        [Summary("Search Kitsu for a character")]
        public async Task Character([Remainder]int input)
        {
            var tmp = await Weeb.GetCharacter(input);
            var result = tmp.Data.Attributes;
            await ReplyAsync("", false, Embed.GetEmbed($"**{result.Name ?? ""}**", $"{Regex.Replace(Regex.Replace(result.Description ?? "", @"<[^>]+>|&nbsp;", "").Trim(), @"\s{2,}", " ")}", result.Image.Original ?? ""));
        }
    }
}
