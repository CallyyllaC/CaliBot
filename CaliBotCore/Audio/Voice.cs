using Discord;
using Discord.Audio;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CaliBotCore.Audio
{
    public class AudioService
    {
        public readonly ConcurrentDictionary<ulong, MusicPlayer> ConnectedChannels = new ConcurrentDictionary<ulong, MusicPlayer>();

        public async Task JoinAudio(IGuild guild, IVoiceChannel target)
        {
            if (ConnectedChannels.TryGetValue(guild.Id, out MusicPlayer client))
            {
                await LeaveAudio(guild);
            }
            if (target.Guild.Id != guild.Id)
            {
                return;
            }

            var audioClient = await target.ConnectAsync();

            if (ConnectedChannels.TryAdd(guild.Id, new MusicPlayer(guild, audioClient)))
            {
                await Program.Log.WriteToLog($"Connected to voice on {guild.Name}.");
            }
        }

        public async Task LeaveAudio(IGuild guild)
        {
            if (ConnectedChannels.TryRemove(guild.Id, out MusicPlayer client))
            {
                await client.client.StopAsync();
                await Program.Log.WriteToLog($"Disconnected from voice on {guild.Name}.");
            }
        }

        private async Task SendAudioAsync(IGuild guild, IMessageChannel channel, string path, string name)
        {
            if (ConnectedChannels.TryGetValue(guild.Id, out MusicPlayer client))
            {
                var msg = await channel.SendMessageAsync("", false, Embed.GetEmbed("Starting Playback", $"Starting playback of {name}"));

                client.playing = true;
                Program.Music = true;
                await Program.Client.SetGameAsync(name, null, ActivityType.Playing);
                using (var ffmpeg = CreateStream(path, client.volume))
                using (var stream = client.client.CreatePCMStream(AudioApplication.Music))
                {
                    var info = GetInfo(path);
                    info.Start();
                    string output = info.StandardOutput.ReadToEnd();
                    info.WaitForExit();
                    var oMycustomclassname = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(output);
                    info.Dispose();
                    string text = "";
                    foreach (string item in oMycustomclassname["format"]["tags"])
                    {
                        try
                        {
                            string str = item.Replace("\"", "");

                            if (str.Length > 1)
                                str = char.ToUpper(str[0]) + str.Substring(1);

                            text = text + "\n" + str;
                        }
                        catch
                        {
                            text = text + "\n" + "error";
                        }
                    }
                    await msg.ModifyAsync(x => x.Embed = Embed.GetEmbed("Currently Playing", text));

                    try { await ffmpeg.StandardOutput.BaseStream.CopyToAsync(stream); }
                    finally
                    {
                        await stream.FlushAsync();
                        client.playing = false;
                        await msg.ModifyAsync(x => x.Embed = Embed.GetEmbed("Finished Playing", text));
                    }
                }
                if (await client.queue.Count() > 0)
                {
                    var next = await client.queue.GetNext();
                    await SendAudioAsync(guild, channel, next.location, next.name);
                }
                Program.Music = false;
                string gm = Program.Client.GetUser(Program.OwnerID).Activity.Name ?? "XD";
                await Program.Client.SetGameAsync(gm, null, ActivityType.Watching);
            }
        }

        public async Task AddToQueue(IGuild guild, IMessageChannel channel, string path, string name)
        {
            if (!File.Exists(path))
            {
                await channel.SendMessageAsync("", false, Embed.GetEmbed("Music", "File does not exist."));
                return;
            }
            if (ConnectedChannels.TryGetValue(guild.Id, out MusicPlayer client))
            {
                await client.queue.Add(path, name);
                await channel.SendMessageAsync("", false, Embed.GetEmbed("Music", $"Added {name} to the queue"));
                if (await client.queue.Count() == 1 && !client.playing)
                {
                    var next = await client.queue.GetNext();
                    await SendAudioAsync(guild, channel, next.location, name);
                }
            }
        }

        public async Task SetVolume(IGuild guild, IMessageChannel channel, float vol)
        {
            if (ConnectedChannels.TryGetValue(guild.Id, out MusicPlayer client))
            {
                client.volume = vol;
                await channel.SendMessageAsync("", false, Embed.GetEmbed("Music", "Volume change will take affect after this song"));
            }
            await Task.CompletedTask;
        }

        public async Task<float> GetVolume(IGuild guild, IMessageChannel channel)
        {
            await Task.CompletedTask;
            if (ConnectedChannels.TryGetValue(guild.Id, out MusicPlayer client))
            {
                return client.volume;
            }
            else return 0;
        }

        public async Task<List<Song>> GetQueue(IGuild guild, IMessageChannel channel)
        {
            if (ConnectedChannels.TryGetValue(guild.Id, out MusicPlayer client))
            {
                return await client.queue.Get();
            }
            else return null;
        }


        private Process CreateStream(string path, float volume)
        {
            return Process.Start(new ProcessStartInfo
            {
                FileName = $"{Program.Rootdir}\\Resources\\ffmpeg\\ffmpeg.exe",
                Arguments = $"-hide_banner -loglevel panic -i \"{path}\" -ac 2 -f s16le -filter:a \"volume = {volume}\" -ar 48000 pipe:1",
                UseShellExecute = false,
                RedirectStandardOutput = true
            });
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

    public class MusicPlayer
    {
        public IAudioClient client;
        public IGuild guild;
        public float volume = 0.5f;
        public MusicQueue queue;
        public bool playing = false;

        public MusicPlayer(IGuild Guild, IAudioClient Client)
        {
            queue = new MusicQueue();
            guild = Guild;
            client = Client;
        }
    }

    public class MusicQueue
    {
        List<Song> queue = new List<Song>();
        public async Task Add(string file, string name)
        {
            queue.Add(new Song(file, name));
            await Task.CompletedTask;
        }
        public async Task Remove(int location)
        {
            queue.RemoveAt(location);
            await Task.CompletedTask;
        }
        public async Task<int> Count()
        {
            await Task.CompletedTask;
            return queue.Count;
        }
        public async Task<Song> GetNext()
        {
            await Task.CompletedTask;
            var tmp = queue[0];
            await Remove(0);
            return tmp;
        }
        public async Task<List<Song>> Get()
        {
            await Task.CompletedTask;
            return queue;
        }
    }

    public class Song
    {
        public string name;
        public string location;
        public Song(string Location, string Name)
        {
            name = Name;
            location = Location;
        }
    }
}
