using CaliBotCore.DataStructures;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Timers;

namespace CaliBotCore.Functions
{
    class NationStates
    {
        private ConcurrentDictionary<ulong, Nation[]> Nations = new ConcurrentDictionary<ulong, Nation[]>();
        private Timer timer = new Timer();
        private FeedParser rss = new FeedParser();

        public NationStates()
        {
            foreach (var item in Program.Guilds.Values)
            {
                try
                {
                    Nations.GetOrAdd(item.Id, item.Nations);
                }
                catch { }
            }
            timer.Interval = 60000;
            timer.AutoReset = true;
            timer.Elapsed += Timer_ElapsedAsync;
            timer.Start();
        }

        private void Timer_ElapsedAsync(object sender, ElapsedEventArgs e)
        {
            CheckFeed();
        }

        public void CheckFeed()
        {
            foreach (var item in Nations)
            {
                var guild = Program.Guilds.GetValueOrDefault(item.Key);
                var channel = Program.Client.GetGuild(item.Key).GetTextChannel(Program.Guilds.GetValueOrDefault(item.Key).NationsChannel);
                foreach (var value in item.Value)
                {
                    string nation = value.Name;
                    var feed = rss.ParseRss($"https://www.nationstates.net/cgi-bin/rss.cgi?nation={nation}");
                    List<Item> newfeed = new List<Item>();
                    int foundat = 696969;
                    if (value.LastPosted != null)
                    {
                        for (int i = feed.Count - 1; i >= 0; i--)
                        {
                            if (feed[i].Title == value.LastPosted.Title && feed[i].PublishDate.ToShortDateString() == value.LastPosted.PublishDate.ToShortDateString())
                            {
                                foundat = i;
                            }
                        }
                    }
                    if (foundat != 696969)
                    {
                        for (int i = 0; i < foundat; i++)
                        {
                            newfeed.Add(feed[i]);
                        }
                    }
                    else
                    {
                        newfeed.AddRange(feed);
                    }
                    value.LastPosted = feed[0];
                    Program.Guilds.GetValueOrDefault(guild.Id).Nations = Nations.GetValueOrDefault(guild.Id);
                    newfeed.Reverse();
                    foreach (var update in newfeed)
                    {
                        channel.SendMessageAsync("", false, Embed.GetEmbed($"**{CultureInfo.CurrentCulture.TextInfo.ToTitleCase(nation.Replace('_', ' '))} Update!**", Regex.Replace(Regex.Replace(update.Title ?? "", @"<[^>]+>|&nbsp;", "").Trim(), @"\s{2,}", " "), Program.CurrentColour, Program.CurrentName, null));
                        System.Threading.Thread.Sleep(1000);
                    }
                }
            }
        }



        public void Add(Guild guild, ulong id, string newnation)
        {
            List<Nation> old = new List<Nation>();
            if (Nations.ContainsKey(guild.Id))
            {
                old.AddRange(Nations.GetValueOrDefault(guild.Id));
                old.Add(new Nation() { Name = newnation });
                old = old.GroupBy(p => p.Name).Select(grp => grp.FirstOrDefault()).ToList();
                Nations.Remove(guild.Id, out Nation[] spare);
                Nations.GetOrAdd(guild.Id, old.ToArray());
            }
            else
            {
                Nations.GetOrAdd(guild.Id, new Nation[] { new Nation() { Name = newnation } });
            }
            Program.Guilds.GetValueOrDefault(guild.Id).Nations = Nations.GetValueOrDefault(guild.Id);
            Program.Guilds.GetValueOrDefault(guild.Id).NationsChannel = id;
        }

        public void Remove(Guild guild, string newnation)
        {
            List<Nation> old = new List<Nation>();
            if (Nations.ContainsKey(guild.Id))
            {
                old.AddRange(Nations.GetValueOrDefault(guild.Id));
                Nations.Remove(guild.Id, out Nation[] tmp);
                var item = old.Where(x => x.Name == newnation).First();
                old.Remove(item);
                Nations.GetOrAdd(guild.Id, old.ToArray());
            }
            else
            {
                return;
            }
            Program.Guilds.GetValueOrDefault(guild.Id).Nations = Nations.GetValueOrDefault(guild.Id);
            Program.Guilds.GetValueOrDefault(guild.Id).NationsChannel = 0;
        }
    }
    public class Nation
    {
        public string Name = "";
        public Item LastPosted = null;
    }
}
