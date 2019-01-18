using Discord.Addons.Interactive;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CaliBotCore.Commands
{
    public class NationStates : InteractiveBase
    {
        [Command("Nation"), Summary("Checks on a given nation")]
        public async Task Nation([Remainder] string input = "")
        {
            try { Program.nationStates.CheckFeed(); }
            catch { await ReplyAsync("Error"); }
        }
        [Command("AddNation"), Summary("Checks on a given nation")]
        public async Task AddNation(string input = "")
        {
            try { Program.nationStates.Add(Program.Guilds.GetValueOrDefault(Context.Guild.Id), Context.Channel.Id, input); }
            catch { await ReplyAsync("Error"); }
            await ReplyAsync("Done");
        }
        [Command("RemoveNation"), Summary("Checks on a given nation")]
        public async Task RemoveNation(string input = "")
        {
            try { Program.nationStates.Remove(Program.Guilds.GetValueOrDefault(Context.Guild.Id), input); }
            catch { await ReplyAsync("Error"); }
            await ReplyAsync("Done");
        }

    }
}
