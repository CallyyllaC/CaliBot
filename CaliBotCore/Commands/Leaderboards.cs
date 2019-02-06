using CaliBotCore.DataStructures;
using CaliBotCore.Functions;
using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CaliBotCore.Commands
{
    class Leaderboards
    {

        public class Rankings : InteractiveBase
        {
            [Command("Global Rank XP", RunMode = RunMode.Async), Summary("Get Global Rank of user xp")]
            public async Task GlobalRank(ulong ID = 0)
            {
                if (ID == 0)
                {
                    ID = Context.Message.Author.Id;
                }
                var tmp = await Ranks.GetGlobalRank(ID);
                await ReplyAsync("", false, Embed.GetEmbed("Global Rank", $"You rank {tmp}"));
            }

            [Command("Global Rank XP", RunMode = RunMode.Async), Summary("Get Global Rank of user xp")]
            public async Task GlobalRank(IUser UserTag)
            {
                ulong id = UserTag.Id;
                var tmp = await Ranks.GetGlobalRank(id);
                await ReplyAsync("", false, Embed.GetEmbed("Global Rank", $"You rank {tmp}"));
            }

            [Command("Guild Rank XP", RunMode = RunMode.Async), Summary("Get Guild Rank of user xp")]
            public async Task LocalRank(ulong ID = 0)
            {
                if (ID == 0)
                {
                    ID = Context.Message.Author.Id;
                }
                var tmp = await Ranks.GetLocalRank(Context.Guild, ID);
                await ReplyAsync("", false, Embed.GetEmbed("Local Rank", $"You rank {tmp}"));
            }

            [Command("Guild Rank XP", RunMode = RunMode.Async), Summary("Get Guild Rank of user xp")]
            public async Task LocalRank(IUser UserTag)
            {
                ulong id = UserTag.Id;
                var tmp = await Ranks.GetLocalRank(Context.Guild, id);
                await ReplyAsync("", false, Embed.GetEmbed("Local Rank", $"You rank {tmp}"));
            }

            [Command("Global Rank Credits", RunMode = RunMode.Async), Summary("Get Global Rank of user credits")]
            public async Task GlobalCredits(ulong ID = 0)
            {
                if (ID == 0)
                {
                    ID = Context.Message.Author.Id;
                }
                var tmp = await Ranks.GetGlobalDosh(ID);
                await ReplyAsync("", false, Embed.GetEmbed("Global Rank", $"You rank {tmp}"));
            }

            [Command("Global Rank Credits", RunMode = RunMode.Async), Summary("Get Global Rank of user credits")]
            public async Task GlobalCredits(IUser UserTag)
            {
                ulong id = UserTag.Id;
                var tmp = await Ranks.GetGlobalDosh(id);
                await ReplyAsync("", false, Embed.GetEmbed("Global Rank", $"You rank {tmp}"));
            }

            [Command("Guild Rank Credits", RunMode = RunMode.Async), Summary("Get Guild Rank of user credits")]
            public async Task LocalCredits(ulong ID = 0)
            {
                if (ID == 0)
                {
                    ID = Context.Message.Author.Id;
                }
                var tmp = await Ranks.GetLocalDosh(Context.Guild, ID);
                await ReplyAsync("", false, Embed.GetEmbed("Local Rank", $"You rank {tmp}"));
            }

            [Command("Guild Rank Credits", RunMode = RunMode.Async), Summary("Get Guild Rank of user credits")]
            public async Task LocalCredits(IUser UserTag)
            {
                ulong id = UserTag.Id;
                var tmp = await Ranks.GetLocalDosh(Context.Guild, id);
                await ReplyAsync("", false, Embed.GetEmbed("Local Rank", $"You rank {tmp}"));
            }

            [Command("Global XP", RunMode = RunMode.Async), Summary("Get Global Leaderboard of user xp")]
            public async Task GlobalRank()
            {
                var tmp = await Ranks.GetGlobal();

                var pages = new PaginatedMessage() { Pages = GroupOf10XP(Context, tmp), Title = "Global XP Leaderboard", Color = new Color(await Program.GetUserColour(Context.Message.Author.Id)), Options = new PaginatedAppearanceOptions { DisplayInformationIcon = false, JumpDisplayOptions = JumpDisplayOptions.Never } };
                pages.Options.DisplayInformationIcon = false;
                await PagedReplyAsync(pages, false);
            }

            [Command("Guild XP", RunMode = RunMode.Async), Summary("Get Guild Leaderboard of user xp")]
            public async Task LocalRank()
            {
                var tmp = await Ranks.GetLocal(Context.Guild);

                var pages = new PaginatedMessage() { Pages = GroupOf10XP(Context, tmp), Title = "Local XP Leaderboard", Color = new Color(await Program.GetUserColour(Context.Message.Author.Id)), Options = new PaginatedAppearanceOptions { DisplayInformationIcon = false, JumpDisplayOptions = JumpDisplayOptions.Never } };
                pages.Options.DisplayInformationIcon = false;
                await PagedReplyAsync(pages, false);
            }

            [Command("Global Credits", RunMode = RunMode.Async), Summary("Get Global Leaderboard of user credits")]
            public async Task GlobalCredits()
            {
                var tmp = await Ranks.GetGlobalDosh();

                var pages = new PaginatedMessage() { Pages = GroupOf10Dosh(Context, tmp), Title = "Global Credits Leaderboard", Color = new Color(await Program.GetUserColour(Context.Message.Author.Id)), Options = new PaginatedAppearanceOptions { DisplayInformationIcon = false, JumpDisplayOptions = JumpDisplayOptions.Never } };
                pages.Options.DisplayInformationIcon = false;
                await PagedReplyAsync(pages, false);
            }

            [Command("Guild Credits", RunMode = RunMode.Async), Summary("Get Guild Leaderboard of user credits")]
            public async Task LocalCredits()
            {
                var tmp = await Ranks.GetLocalDosh(Context.Guild);

                var pages = new PaginatedMessage() { Pages = GroupOf10Dosh(Context, tmp), Title = "Local Credits Leaderboard", Color = new Color(await Program.GetUserColour(Context.Message.Author.Id)), Options = new PaginatedAppearanceOptions { DisplayInformationIcon = false, JumpDisplayOptions = JumpDisplayOptions.Never } };
                pages.Options.DisplayInformationIcon = false;
                await PagedReplyAsync(pages, false);
            }

            [Command("Neps", RunMode = RunMode.Async), Summary("Get current Neps")]
            public async Task Neps()
            {
                int counter = 0;

                foreach (var item in Program.Guilds)
                {
                    counter = counter + item.Value.Nep;
                }
                try
                {
                    var emote = GuildEmote.Parse(Program.nepmote).Url;
                    await ReplyAsync("", false, Embed.GetEmbed("Neps!", $"The current ammount of neps in this server is {counter}, the current amount of neps overall is {Program.Guilds.GetValueOrDefault(Context.Guild.Id).Nep}", await Program.GetUserColour(Context.Message.Author.Id), "Nep Nep", emote));
                }
                catch
                {
                    await ReplyAsync("", false, Embed.GetEmbed("Neps!", $"There was an error calculating neps"));
                }
            }
        }

        public static List<string> GroupOf10XP(SocketCommandContext Context, List<User> tmp)
        {

            var NewPages = new List<string>();
            int cnt = 1;

            foreach (var item in tmp)
            {
                try
                {
                    NewPages.Add($"{cnt}. {Context.Client.GetUser(item.Id).Username}   {item.xp}");
                }
                catch
                {
                    NewPages.Add($"{cnt}. Error {item.xp}");
                }
                cnt++;
            }

            var tmp1 = SplitList(NewPages);
            var tmp2 = ReList(tmp1);


            return tmp2;
        }

        public static List<string> GroupOf10Dosh(SocketCommandContext Context, List<User> tmp)
        {

            var NewPages = new List<string>();
            int cnt = 1;

            foreach (var item in tmp)
            {
                try
                {
                    NewPages.Add($"{cnt}. {Context.Client.GetUser(item.Id).Username}   {item.currency}");
                }
                catch
                {
                    NewPages.Add($"{cnt}. Error {item.currency}");
                }
                cnt++;
            }

            var tmp1 = SplitList(NewPages);
            var tmp2 = ReList(tmp1);


            return tmp2;
        }

        public static IEnumerable<List<string>> SplitList(List<string> locations)
        {
            for (int i = 0; i < locations.Count; i += 10)
            {
                yield return locations.GetRange(i, Math.Min(10, locations.Count - i));
            }
        }

        public static List<string> ReList(IEnumerable<List<string>> locations)
        {
            var finallist = new List<string>();
            foreach (var item2 in locations)
            {
                string final = "";
                foreach (var item in item2)
                {
                    final = final + "\n" + item;
                }
                finallist.Add(final);
            }
            return finallist;
        }
    }
}
