using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaliBotCore.Config;
using Discord;

namespace CaliBotCore
{
    class Embed
    {
        public static Discord.Embed GetEmbed(string title, string desc, uint colour, string footer, string thumb)
        {
            desc = desc.Truncate(2048);
            var embed = new EmbedBuilder()
            {
                Title = title,
                Description = desc,
                Color = new Color(colour),
                Footer = new EmbedFooterBuilder() { Text = footer },
                ThumbnailUrl = thumb
            };
            var output = embed.Build();
            return output;
        }

        public static Discord.Embed GetEmbed(string title, string desc)
        {
            desc = desc.Truncate(2000);
            if (desc.Length == 2000)
            {
                desc = desc + " -----> Truncated!";
            }
            var embed = new EmbedBuilder()
            {
                Title = title,
                Description = desc,
                Color = new Color(Program.CurrentColour),
                Footer = new EmbedFooterBuilder() { Text = Program.CurrentName },
                ThumbnailUrl = Program.CurrentUrl
            };
            var output = embed.Build();
            return output;
        }

        public static Discord.Embed GetEmbed(string title, string desc, uint colour, string footer, string thumb, string image)
        {
            desc = desc.Truncate(2000);
            if (desc.Length == 2000)
            {
                desc = desc + " -----> Truncated!";
            }
            var embed = new EmbedBuilder()
            {
                Title = title,
                Description = desc,
                Color = new Color(colour),
                Footer = new EmbedFooterBuilder() { Text = footer },
                ThumbnailUrl = thumb,
                ImageUrl = image
            };
            var output = embed.Build();
            return output;
        }

        public static Discord.Embed GetEmbed(string title, string desc, string image)
        {
            desc = desc.Truncate(2000);
            if (desc.Length == 2000)
            {
                desc = desc + " -----> Truncated!";
            }
            var embed = new EmbedBuilder()
            {
                Title = title,
                Description = desc,
                Color = new Color(Program.CurrentColour),
                Footer = new EmbedFooterBuilder() { Text = Program.CurrentName },
                ThumbnailUrl = Program.CurrentUrl,
                ImageUrl = image
            };
            var output = embed.Build();
            return output;
        }

        public static Discord.Embed[] GetEmbeds(string title, string desc, uint colour, string footer, string thumb)
        {
            var returnable = new List<Discord.Embed>();
            var descarr = desc.SplitByLength(2048);
            int cnt = 0;
            foreach (var item in descarr)
            {
                EmbedBuilder embed;
                if (cnt != 0)
                {
                    embed = new EmbedBuilder()
                    {
                        Title = null,
                        Description = item,
                        Color = new Color(colour),
                        Footer = null,
                        ThumbnailUrl = null
                    };
                }
                else
                {
                    embed = new EmbedBuilder()
                    {
                        Title = title,
                        Description = item,
                        Color = new Color(colour),
                        Footer = new EmbedFooterBuilder() { Text = footer },
                        ThumbnailUrl = thumb
                    };
                }
                cnt++;
                returnable.Add(embed.Build());
            }
            return returnable.ToArray();
        }
        public static Discord.Embed[] GetEmbeds(string title, string desc)
        {
            var returnable = new List<Discord.Embed>();
            var descarr = desc.SplitByLength(2048);
            int cnt = 0;
            foreach (var item in descarr)
            {
                EmbedBuilder embed;
                if (cnt != 0)
                {
                    embed = new EmbedBuilder()
                    {
                        Title = null,
                        Description = item,
                        Color = new Color(Program.CurrentColour),
                        Footer = null,
                        ThumbnailUrl = null
                    };
                }
                else
                {
                    embed = new EmbedBuilder()
                    {
                        Title = title,
                        Description = item,
                        Color = new Color(Program.CurrentColour),
                        Footer = new EmbedFooterBuilder() { Text = Program.CurrentName },
                        ThumbnailUrl = Program.CurrentUrl
                    };
                }
                cnt++;
                returnable.Add(embed.Build());
            }
            return returnable.ToArray();
        }
        public static Discord.Embed[] GetEmbeds(string title, string desc, uint colour, string footer, string thumb, string image)
        {
            var returnable = new List<Discord.Embed>();
            var descarr = desc.SplitByLength(2048);
            int cnt = 0;
            foreach (var item in descarr)
            {
                EmbedBuilder embed;
                if (cnt == descarr.Count()-1)
                {
                    embed = new EmbedBuilder()
                    {
                        Title = null,
                        Description = item,
                        Color = new Color(colour),
                        Footer = null,
                        ThumbnailUrl = null,
                        ImageUrl = image
                    };
                }
                else if (cnt != 0)
                {
                    embed = new EmbedBuilder()
                    {
                        Title = null,
                        Description = item,
                        Color = new Color(colour),
                        Footer = null,
                        ThumbnailUrl = null,
                        ImageUrl = null
                    };
                }
                else
                {
                    embed = new EmbedBuilder()
                    {
                        Title = title,
                        Description = item,
                        Color = new Color(colour),
                        Footer = new EmbedFooterBuilder() { Text = footer },
                        ThumbnailUrl = thumb,
                        ImageUrl = null
                    };
                }
                cnt++;
                returnable.Add(embed.Build());
            }
            return returnable.ToArray();
        }
        public static Discord.Embed[] GetEmbeds(string title, string desc, string image)
        {
            var returnable = new List<Discord.Embed>();
            var descarr = desc.SplitByLength(2048);
            int cnt = 0;
            if (descarr.Count() == 0)
            {
                EmbedBuilder embed;

                embed = new EmbedBuilder()
                {
                    Title = title,
                    Description = "",
                    Color = new Color(Program.CurrentColour),
                    Footer = new EmbedFooterBuilder() { Text = Program.CurrentName },
                    ThumbnailUrl = null,
                    ImageUrl = image
                };
                returnable.Add(embed.Build());
            }
            else if (descarr.Count() == 1)
            {
                EmbedBuilder embed;

                embed = new EmbedBuilder()
                {
                    Title = title,
                    Description = descarr.First() ?? "",
                    Color = new Color(Program.CurrentColour),
                    Footer = new EmbedFooterBuilder() { Text = Program.CurrentName },
                    ThumbnailUrl = null,
                    ImageUrl = image
                };
                returnable.Add(embed.Build());
            }
            else
            {
                foreach (var item in descarr)
                {
                    EmbedBuilder embed;

                    if (cnt == descarr.Count() - 1)
                    {
                        embed = new EmbedBuilder()
                        {
                            Title = null,
                            Description = item,
                            Color = new Color(Program.CurrentColour),
                            Footer = new EmbedFooterBuilder() { Text = Program.CurrentName },
                            ThumbnailUrl = null,
                            ImageUrl = image
                        };
                    }
                    else if (cnt != 0)
                    {
                        embed = new EmbedBuilder()
                        {
                            Title = null,
                            Description = item,
                            Color = new Color(Program.CurrentColour),
                            Footer = null,
                            ThumbnailUrl = null,
                            ImageUrl = null
                        };
                    }
                    else
                    {
                        embed = new EmbedBuilder()
                        {
                            Title = title,
                            Description = item,
                            Color = new Color(Program.CurrentColour),
                            Footer = null,
                            ThumbnailUrl = Program.CurrentUrl,
                            ImageUrl = null
                        };
                    }
                    cnt++;
                    returnable.Add(embed.Build());
                }
            }
            return returnable.ToArray();
        }

    }
}
