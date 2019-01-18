using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaliBotCore.Commands
{
    public class Helper : InteractiveBase
    {
        private readonly CommandService _commands;
        private readonly IServiceProvider _map;
        public Helper(IServiceProvider map, CommandService commands)
        {
            _commands = commands;
            _map = map;
        }

        [Command("Help")]
        [Summary("List the bot's commands")]
        public async Task Help()
        {
            Collection<string> Pages = new Collection<string>();

            var guild = Program.Guilds.GetValueOrDefault(Context.Guild.Id);
            char prefix = guild.Prefix;

            string txt = "Contents:";
            int i = 1;
            foreach (var item in _commands.Modules.Where(m => m.Parent == null))
            {
                if (item.Name != "Help")
                {
                    i++;
                    GetContents(i, item, ref txt);
                }
            }
            Pages.Add(txt);

            foreach (var mod in _commands.Modules.Where(m => m.Parent == null))
            {
                if (mod.Name != "Help")
                {
                    Pages.Add(AddHelp(prefix, mod));
                }
            }

            var options = new PaginatedAppearanceOptions() { DisplayInformationIcon = false, JumpDisplayOptions = 0 };
            var pages = Pages.AsEnumerable<object>();

            var final = new PaginatedMessage() { Title = $"**{Program.CurrentName} Helper!**", Color = new Color(Program.CurrentColour), Options = options, Pages = pages };

            await PagedReplyAsync(final, false);
        }

        private string AddHelp(char p, ModuleInfo module)
        {
            string builder = null;
            foreach (var sub in module.Submodules) AddSubHelp(p, sub, ref builder);
            builder = builder + "\n" + $"**{module.Name}**";
            if (module.Submodules.Count >= 1) { builder = builder + "\n" + $"Submodules: \n{string.Join("", module.Submodules.Select(m => m.ToString()))}"; }
            if (module.Commands.Count >= 1)
            {
                builder = builder + "\n" + $"{string.Join("", module.Commands.Select(x => $"{x.Summary}:\n`{p}{string.Join("||", x.Aliases.Select(z => $"{z} "))}" + $" Parameters: {string.Join(", ",x.Parameters.Select(z => $"{z.Type.Name} {z.Name}").DefaultIfEmpty("None"))} `" +  $" =Requires=> \n{string.Join("\n", x.Preconditions.Select(z => $"{z}").DefaultIfEmpty("None"))} \n" + "\n"))}\n";
            }
            return builder;
        }

        private void AddSubHelp(char p, ModuleInfo module, ref string builder)
        {
            foreach (var sub in module.Submodules) AddSubHelp(p, sub, ref builder);
            builder = builder + "\n" + $"**{module.Name}**";
            if (module.Submodules.Count >= 1) { builder = builder + "\n" + $"Submodules: \n{string.Join("", module.Submodules.Select(m => m.ToString()))}"; }
            if (module.Commands.Count >= 1) { builder = builder + "\n" + $"{string.Join("", module.Commands.Select(x => $"{x.Summary}:\n`{p}{string.Join("||", x.Aliases.Select(z => $"{z} "))}" + $" Parameters: {string.Join(", ", x.Parameters.Select(z => $"{z.Type.Name} {z.Name}").DefaultIfEmpty("None"))} `" + $" =Requires=> \n{string.Join("\n", x.Preconditions.Select(z => $"{z}").DefaultIfEmpty("None"))}" + "\n"))}\n"; }
        }

        private void GetContents(int i, ModuleInfo module, ref string txt)
        {
            txt = txt + "\n" + $"Page {i}.  {module.Name}";
        }
    }
}