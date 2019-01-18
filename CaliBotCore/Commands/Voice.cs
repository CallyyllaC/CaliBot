using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CaliBotCore.Functions;
using Discord;
using Discord.Commands;
using CaliBotCore.Audio;
using System.Diagnostics;
using System.IO;
using CaliBotCore.Images;
using System.Linq;

namespace CaliBotCore.Commands
{
    public class Voice : ModuleBase<ICommandContext>
    {
        private readonly AudioService service;
        
        public Voice(AudioService newservice)
        {
            service = newservice;
        }

        [Command("join", RunMode = RunMode.Async)]
        public async Task Join()
        {
            if (!Program.Guilds.GetValueOrDefault(Context.Guild.Id).Music || Context.Message.Author.Id != Program.OwnerID){ await Context.Channel.SendMessageAsync("",false,Embed.GetEmbed("Music","This guild is not authorised to use music")); return; }            
            await service.JoinAudio(Context.Guild, (Context.User as IVoiceState).VoiceChannel);
        }

        [Command("leave", RunMode = RunMode.Async)]
        public async Task Leave()
        {
            if (!Program.Guilds.GetValueOrDefault(Context.Guild.Id).Music || Context.Message.Author.Id != Program.OwnerID) { await Context.Channel.SendMessageAsync("", false, Embed.GetEmbed("Music", "This guild is not authorised to use music")); return; }
            await service.LeaveAudio(Context.Guild);
        }

        [Command("Local", RunMode = RunMode.Async)]
        public async Task Local([Remainder] string song)
        {
            if (!Program.Guilds.GetValueOrDefault(Context.Guild.Id).Music || Context.Message.Author.Id != Program.OwnerID) { await Context.Channel.SendMessageAsync("", false, Embed.GetEmbed("Music", "This guild is not authorised to use music")); return; }
            if (File.Exists(song))
            {
                var fa = File.GetAttributes(song);
                string name;
                var tmp2 = GetSong(song);
                if (tmp2 != null)
                {
                    name = tmp2;
                }
                else
                {
                    name = Path.GetFileNameWithoutExtension(song);
                }
                await service.AddToQueue(Context.Guild, Context.Channel, song, name);
            }
            else
            {
                await ReplyAsync("", false, Embed.GetEmbed("Music", "Couldn't find local file"));
            }
        }

        [Command("Youtube", RunMode = RunMode.Async), Alias("yt")]
        public async Task Youtube([Remainder] string song)
        {
            if (!Program.Guilds.GetValueOrDefault(Context.Guild.Id).Music || Context.Message.Author.Id != Program.OwnerID) { await Context.Channel.SendMessageAsync("", false, Embed.GetEmbed("Music", "This guild is not authorised to use music")); return; }
            await ReplyAsync("This feature has not been added yet");
        }

        [Command("Link", RunMode = RunMode.Async)]
        public async Task Link(string song)
        {
            if (!Program.Guilds.GetValueOrDefault(Context.Guild.Id).Music || Context.Message.Author.Id != Program.OwnerID) { await Context.Channel.SendMessageAsync("", false, Embed.GetEmbed("Music", "This guild is not authorised to use music")); return; }
            try
            {
                var tmp = await GetFromWeb.PullImageFromWebAsync(song);
                var dot = song.LastIndexOf('.');
                var type = song.Substring(dot, song.Length - dot);
                var slash = song.LastIndexOf('/');
                var namelen = song.Length - slash - (Math.Abs(dot - song.Length));
                var name = song.Substring(slash+1, namelen-1);
                if (!File.Exists($"{Program.Rootdir}\\Music\\{Program.CalculateMD5(tmp)}{type}"))
                {
                    await File.WriteAllBytesAsync($"{Program.Rootdir}\\Music\\{Program.CalculateMD5(tmp)}{type}", tmp);
                }
                var tmp2 = GetSong($"{Program.Rootdir}\\Music\\{Program.CalculateMD5(tmp)}{type}");
                if (tmp2 != null)
                {
                    name = tmp2;
                }
                await service.AddToQueue(Context.Guild, Context.Channel, $"{Program.Rootdir}\\Music\\{Program.CalculateMD5(tmp)}{type}", name);
            }
            catch (Exception e)
            {
                await ReplyAsync("", false, Embed.GetEmbed("Music", $"There was an error getting the file : {e.Message}"));
            }
        }

        [Command("Upload", RunMode = RunMode.Async)]
        public async Task Upload()
        {
            if (!Program.Guilds.GetValueOrDefault(Context.Guild.Id).Music || Context.Message.Author.Id != Program.OwnerID) { await Context.Channel.SendMessageAsync("", false, Embed.GetEmbed("Music", "This guild is not authorised to use music")); return; }
            if (Context.Message.Attachments.Count() >= 1)
            {
                int count = 1;
                foreach (var item in Context.Message.Attachments)
                {
                    try
                    {
                        var tmp = await GetFromWeb.PullImageFromWebAsync(item.Url);
                        var dot = item.Filename.LastIndexOf('.');
                        var type = item.Filename.Substring(dot, item.Filename.Length-dot);
                        var name = item.Filename.Substring(0, item.Filename.Length - type.Length);
                        if (!File.Exists($"{Program.Rootdir}\\Music\\{Program.CalculateMD5(tmp)}{type}"))
                        {
                            await File.WriteAllBytesAsync($"{Program.Rootdir}\\Music\\{Program.CalculateMD5(tmp)}{type}", tmp);
                        }
                        var tmp2 = GetSong($"{Program.Rootdir}\\Music\\{Program.CalculateMD5(tmp)}{type}");
                        if (tmp2 != null)
                        {
                            name = tmp2;
                        }
                        await service.AddToQueue(Context.Guild, Context.Channel, $"{Program.Rootdir}\\Music\\{Program.CalculateMD5(tmp)}{type}",name);
                    }
                    catch(Exception e)
                    {
                        await ReplyAsync("", false, Embed.GetEmbed("Music", $"There was an error getting the file : {e.Message}"));
                    }
                    count++;
                }
            }
            else
            {

            }
        }

        [Command("volume", RunMode = RunMode.Async)]
        public async Task Volume(float song)
        {
            if (!Program.Guilds.GetValueOrDefault(Context.Guild.Id).Music || Context.Message.Author.Id != Program.OwnerID) { await Context.Channel.SendMessageAsync("", false, Embed.GetEmbed("Music", "This guild is not authorised to use music")); return; }
            await service.SetVolume(Context.Guild, Context.Channel, song);
        }

        [Command("volume", RunMode = RunMode.Async)]
        public async Task Volume()
        {
            if (!Program.Guilds.GetValueOrDefault(Context.Guild.Id).Music || Context.Message.Author.Id != Program.OwnerID) { await Context.Channel.SendMessageAsync("", false, Embed.GetEmbed("Music", "This guild is not authorised to use music")); return; }
            var tmp = await service.GetVolume(Context.Guild, Context.Channel);
            await ReplyAsync("", false, Embed.GetEmbed("Music", tmp.ToString()));
        }

        [Command("queue", RunMode = RunMode.Async)]
        public async Task Queue()
        {
            if (!Program.Guilds.GetValueOrDefault(Context.Guild.Id).Music || Context.Message.Author.Id != Program.OwnerID) { await Context.Channel.SendMessageAsync("", false, Embed.GetEmbed("Music", "This guild is not authorised to use music")); return; }
            var queue = await service.GetQueue(Context.Guild, Context.Channel);

            var text = "";
            int count = 0;
            foreach (var item in queue)
            {
                count++;
                text = text + "\n" + item.name;
            }

            await ReplyAsync("", false, Embed.GetEmbed("Music Queue",text));
        }

        private string GetSong(string path)
        {
            try
            {
                var info = GetInfo(path);
                info.Start();
                string output = info.StandardOutput.ReadToEnd();
                info.WaitForExit();
                var oMycustomclassname = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(output);
                info.Dispose();
                return oMycustomclassname["format"]["tags"]["title"];
            }
            catch
            {
                return null;
            }
        }

        private Process GetInfo(string path)
        {
            return Process.Start(new ProcessStartInfo
            {
                FileName = $"{Program.Rootdir}\\Resources\\ffmpeg\\ffprobe.exe",
                Arguments = $"-v quiet -print_format json -show_format \"{path}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true
            });
        }
    }
}
