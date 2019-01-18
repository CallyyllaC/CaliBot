using CaliBotCore.Config;
using CaliBotCore.DataStructures;
using CaliBotCore.Images;
using CaliBotCore.Resources;
using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.WebSocket;
using Discord.Audio;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Timers;
using CaliBotCore.Audio;
using System.Net;
using System.Security.Cryptography;
using System.Collections.Concurrent;
using CaliBotCore.Functions;

namespace CaliBotCore
{
    class Program
    {
        //info
        public static string OwnerGithub = null;
        public static string BotRepo = null;
        public static string Ver = null;
        public static Bot Bot_Properties;

        //rootdir
        public static string Rootdir = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\CaliBot";

        //data
        public static string CurrentName = "BaseName";
        public static uint CurrentColour = new Color(190, 0, 228).RawValue;
        public static string CurrentUrl = "";
        public static string ChangeLog = $"ChangeLog.txt";
        public static string nepmote = null;

        //MyStuff
        public static string Token;
        public static string GoogleApi;
        public static ulong Botid;
        public static ulong OwnerID;
        private static bool ready = false;

        public static ConcurrentDictionary<ulong, Guild> Guilds = new ConcurrentDictionary<ulong, Guild>();
        public static ConcurrentDictionary<ulong, User> Users = new ConcurrentDictionary<ulong, User>();
        public static ConcurrentDictionary<int, Waifu> Waifus = new ConcurrentDictionary<int, Waifu>();
        public static List<IEmote> Emotes = new List<IEmote>();
        public static bool Music = false;

        public static IServiceProvider Services;
        public static CommandService Commands;
        public static AudioService Voice;
        public static DiscordSocketClient Client;

        public static IDMChannel OwnerChannel;
        public static Log Log = new Log();
        public static DirectMessage DirectMessages;
        public static Activity activity;
        public static MyFonts Myfonts;
        public static Personas persona;
        public static Rainbow rainbow;
        public static NationStates nationStates;

        public static List<IGuildUser> jail = new List<IGuildUser>();

        private static readonly Timer timerhour = new Timer();
        private static readonly Timer timermin = new Timer();


        static void Main(string[] args) =>new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            await Init();

            Client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Info,
                MessageCacheSize = 1000
            });

            Client.Log += Log.WriteToLog;

            Commands = new CommandService();
            Voice = new AudioService();

            Services = new ServiceCollection()
                .AddSingleton(Client)
                .AddSingleton<InteractiveService>()
                .AddSingleton(Voice)
                .AddSingleton(Commands)
                .BuildServiceProvider();
            await Client.LoginAsync(TokenType.Bot, Token);
            await Client.StartAsync();

            Client.Ready += Ready;

            Client.GuildAvailable += Client_GuildAvailable;
            Client.LeftGuild += Client_LeftGuild;
            Client.GuildUpdated += Client_GuildUpdated;

            Client.UserLeft += Client_UserLeft;
            Client.UserJoined += Client_UserJoined;

            Client.RecipientAdded += Client_RecipientAdded;

            Client.MessageReceived += Client_MessageReceived;
            //add here

            await Task.Delay(-1);
        }

        private async Task Client_MessageReceived(SocketMessage arg)
        {
            var Iusermsg = arg as IUserMessage;

            //is self?
            if (Iusermsg.Author.Id == Client.CurrentUser.Id) { return; }

            //is dm?
            bool dm = false;
            if (await DirectMessages.IsDMAsync(arg, Client)) { DirectMessages.ForwardDMs(arg, Client); dm = true; }


            //Get user and guild objects
            User user = Users.GetValueOrDefault(Iusermsg.Author.Id);
            Guild guild = null;
            SocketTextChannel tmp;
            if (!dm) { tmp = Iusermsg.Channel as SocketTextChannel; guild = Guilds.GetValueOrDefault(tmp.Guild.Id); }


            //blacklisted?
            if (user.Blacklisted) { return; }
            if (guild.Blacklisted) { return; }

            if (!dm)
            {
                if (jail.Contains(arg.Author as IGuildUser))
                {
                    var jailedmessage = arg as IUserMessage;
                    await jailedmessage.AddReactionAsync(new Emoji("🛂"));
                    return;
                }

                if (guild.Activity)
                {
                    activity.SendTypingAsync(arg);
                    activity.Emoji(arg as IUserMessage, 30);
                }
                if (Iusermsg.Content.ToLower().Contains("nep") && Iusermsg.Content.ToLower() != $"{guild.Prefix}neps")
                {
                    guild.Nep++;
                    try
                    {
                        await Iusermsg.AddReactionAsync(GuildEmote.Parse(nepmote));
                    }
                    catch
                    {
                        await Log.WriteToLog("Could not parse nepmote");
                    }
                    await EditGuild(guild);
                }
                //Profile Stuff
                Currency.AddCurrencyAsync(user.Id);
                XP.AddXPAsync(arg);
                                
                //Commands
                await HandleCommandAsync(arg);
            }

            await Task.CompletedTask;
        }

        private async Task Client_RecipientAdded(SocketGroupUser arg)
        {
            try
            {
                var embed = Embed.GetEmbed(arg.Username, $"Hello! Welcome to {arg.Channel.Name}!", CurrentColour, CurrentName, arg.GetAvatarUrl());
                await arg.SendMessageAsync("", false, embed);

                if (!File.Exists($"{Rootdir}\\Users\\{arg.Id}.json"))
                {
                    await CreateUserObject(arg);
                }
            }
            catch (Exception e)
            {
                await Log.WriteToLog(e.Message);
            }

            await Task.CompletedTask;
        }

        private async Task Client_UserLeft(SocketGuildUser arg)
        {
            try
            {
                var embed = Embed.GetEmbed($"{arg.Username} / {arg.Nickname}", $"Oh No! They seem to have left the server!", CurrentColour, CurrentName, arg.GetAvatarUrl());

                await arg.Guild.DefaultChannel.SendMessageAsync("", false, embed);
            }
            catch (Exception e)
            {
                await Log.WriteToLog(e.Message);
            }

            await Task.CompletedTask;
        }
        private async Task Client_UserJoined(SocketGuildUser arg)
        {
            try
            {
                await arg.Guild.DefaultChannel.SendMessageAsync("", false, Embed.GetEmbed(arg.Username, $"Hello! Welcome to {arg.Guild.Name}!", CurrentColour, CurrentName, arg.GetAvatarUrl()));

                if (!File.Exists($"{Rootdir}\\Users\\{arg.Id}.json"))
                {
                    await CreateUserObject(arg);
                }

                AutoRole(arg.Id, arg.Guild.Id);
            }
            catch (Exception e)
            {
                await Log.WriteToLog(e.Message);
            }

            await Task.CompletedTask;
        }

        private async Task Client_GuildAvailable(SocketGuild arg)
        {
            if (!File.Exists($"{Rootdir}\\Guilds\\{arg.Id}.json"))
            {
                //if it cant find guild data assume new guild
                await Log.WriteToLog("Couldnt find guild, creating...");

                await CreateGuildObject(arg); //guild setup

                DirectMessages.SendMessageAsync($"Joined Guild: {arg.Name} {arg.Id} Owner: {arg.Owner.Username} {arg.OwnerId}");

                await Log.WriteToLog("Created!");
            }
            else if (!Guilds.ContainsKey(arg.Id))
            {
                //load the data of this guild
                await Log.WriteToLog($"Found guild, {arg.Name}");

                await LoadGuildObject(arg);

                await Log.WriteToLog($"{arg.Name} loaded!");
            }
            else
            {
                return;
            }

            Emotes.AddRange(arg.Emotes);

            foreach (var item in arg.Users)
            {
                if (!Users.ContainsKey(item.Id))
                {
                    if (!File.Exists($"{Rootdir}\\Users\\{item.Id}.json"))
                    {
                        await CreateUserObject(item as SocketUser);
                    }
                    else
                    {
                        await LoadUser(item as SocketUser);
                    }
                }
            }

            await Task.CompletedTask;
        }
        private async Task Client_LeftGuild(SocketGuild arg)
        {
            Guilds.Remove(arg.Id, out Guild tmp);
            await Task.CompletedTask;
        }
        private async Task Client_GuildUpdated(SocketGuild arg1, SocketGuild arg2)
        {
            Emotes.Clear();
            foreach (var item in Guilds.Keys)
            {
                Emotes.AddRange(Client.GetGuild(item).Emotes);
            }
            await Task.CompletedTask;
        }

        private async Task CreateGuildObject(SocketGuild arg)
        {
            Guild newGuild = new Guild { Id = arg.Id };
            Json.CreateJson(arg.Id.ToString(), $"{Rootdir}\\Guilds", newGuild);
            Guilds.GetOrAdd(arg.Id, newGuild);
            await rainbow.CreateRole(arg);
            await Task.CompletedTask;
        }
        private async Task LoadGuildObject(SocketGuild arg)
        {
            Guild newGuild = Json.CreateObject<Guild>($"{Rootdir}\\Guilds\\{arg.Id}.json");
            Guilds.GetOrAdd(arg.Id, newGuild);
            await Task.CompletedTask;
        }

        private async Task CreateUserObject(SocketUser arg)
        {
            User newUser = new User { Id = arg.Id };
            Json.CreateJson(arg.Id.ToString(), $"{Rootdir}\\Users", newUser);
            Users.GetOrAdd(arg.Id, newUser);
            await Task.CompletedTask;
        }
        private async Task LoadUser(SocketUser arg)
        {
            User newUser = Json.CreateObject<User>($"{Rootdir}\\Users\\{arg.Id}.json");
            Users.GetOrAdd(arg.Id, newUser);
            await Task.CompletedTask;
        }

        private async Task LoadUser(string arg)
        {
            User newUser = Json.CreateObject<User>(arg);
            Users.GetOrAdd(newUser.Id, newUser);
            await Task.CompletedTask;
        }
        private async Task LoadWaifu(string arg)
        {
            Waifu newUser = Json.CreateObject<Waifu>(arg);
            Waifus.GetOrAdd(newUser.Id, newUser);
            await Task.CompletedTask;
        }

        private async Task Init()
        {
            Configfile configfile = Json.CreateObject<Configfile>($"{Rootdir}\\Config\\config.json");
            Token = configfile.token;
            GoogleApi = configfile.googleApi;
            Botid = configfile.botid;
            OwnerID = configfile.ownerID;
            OwnerGithub = configfile.OwnerGithub;
            BotRepo = configfile.BotRepo;
            Ver = Json.CreateObject<Ver>($"{Rootdir}\\Ver.json").version;
            nepmote = configfile.nepmote;
            Config.Config.Save();

            Bot_Properties = Updater.Load();

            await LoadUsers();
            await LoadWaifus();

            await Task.CompletedTask;
        }

        private async Task LoadUsers()
        {
            var search = Directory.GetFiles($"{ Rootdir}\\Users\\", "*.json");
            foreach (var item in search)
            {
                await LoadUser(item);
            }
            await Task.CompletedTask;
        }

        private async Task LoadWaifus()
        {
            var search = Directory.GetFiles($"{ Rootdir}\\Waifus\\", "*.json");
            foreach (var item in search)
            {
                await LoadWaifu(item);
            }
            await Task.CompletedTask;
        }

        private async Task Ready()
        {
            if (ready)
            {
                await OwnerChannel.SendMessageAsync("Reconnected");
                return;
            }
            ready = true;
            try
            {
                //dm's
                OwnerChannel = await Client.GetUser(OwnerID).GetOrCreateDMChannelAsync();
                DirectMessages = new DirectMessage(OwnerChannel);
                await OwnerChannel.SendMessageAsync("Connected");

                //objects
                Myfonts = new MyFonts();
                activity = new Activity(Client);
                persona = new Personas(Client);
                rainbow = new Rainbow();
                nationStates = new NationStates();

                //addemotes
                await activity.GetDefaultEmojis();

                //timer
                timerhour.Interval = (60 * 60 * 1000);
                timerhour.AutoReset = true;
                timerhour.Elapsed += Timer_ElapsedAsynchourAsync;
                timermin.Interval = (60 * 1000);
                timermin.AutoReset = true;
                timermin.Elapsed += Timer_ElapsedAsyncmin;
                timermin.Start();

                //closed check
                try
                {
                    if (Bot_Properties.updated == true)
                    {
                        var guild = Client.Guilds.Single(x => x.Id == Bot_Properties.guildId);
                        var channel = guild.Channels.Single(x => x.Id == Bot_Properties.channelId);
                        var chan = channel as SocketTextChannel;
                        var msg = await chan.GetMessageAsync(Bot_Properties.messageId);
                        var message = msg as IUserMessage;
                        await message.ModifyAsync(x => x.Embed = Embed.GetEmbed("**Bot Update**", $"Successfully Updated!", Program.CurrentColour, Program.CurrentName, Program.CurrentUrl));
                        Bot_Properties = Updater.Reset(Bot_Properties);
                        foreach (var item in Client.Guilds)
                        {
                            await item.DefaultChannel.SendMessageAsync("", false, Embed.GetEmbed("**Bot Update**", $"Bot has been updated, please check the changelog to find out what's new! If you find bugs please report them to the bots dm's", Program.CurrentColour, Program.CurrentName, Program.CurrentUrl));
                        }
                    }
                    else if (Bot_Properties.restart == true)
                    {
                        
                        var guild = Client.Guilds.Single(x => x.Id == Bot_Properties.guildId);
                        var channel = guild.Channels.Single(x => x.Id == Bot_Properties.channelId);
                        var chan = channel as SocketTextChannel;
                        var msg = await chan.GetMessageAsync(Bot_Properties.messageId);
                        var message = msg as IUserMessage;
                        await message.ModifyAsync(x => x.Embed = Embed.GetEmbed("**Bot Restart**", $"Back!", Program.CurrentColour, Program.CurrentName, Program.CurrentUrl));
                        Bot_Properties = Updater.Reset(Bot_Properties);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    if (Bot_Properties == null)
                    {
                        Bot_Properties = new Bot();
                        Updater.Save(Bot_Properties);
                    }
                }

                //commands
                await Commands.AddModulesAsync(Assembly.GetEntryAssembly(), Services);

                Console.WriteLine("Ready Complete");
            }
            catch (Exception e)
            {
                await Log.WriteToLog(e.Message);
            }
        }

        private async void Timer_ElapsedAsynchourAsync(object sender, ElapsedEventArgs e)
        {
            foreach (var item in Users)
            {
                try
                {
                    await SaveUser(item.Value.Id);
                }
                catch { }
            }
            foreach (var item in Guilds)
            {
                try
                {
                    await SaveGuild(item.Value.Id);
                }
                catch { }
            }
        }

        private void Timer_ElapsedAsyncmin(object sender, ElapsedEventArgs e)
        {
            if (!timerhour.Enabled)
            {
                if (DateTime.Now.Minute == 0)
                {
                    timerhour.Start();
                }
            }

            //Check versions
            var tmp = Json.CreateObject<Ver>($"{Rootdir}\\Ver.json");
            string link = $"https://raw.githubusercontent.com/{OwnerGithub}/{BotRepo}/master/Ver.json";
            using (var tmpclient = new System.Net.Http.HttpClient())
            {
                var content = Json.CreateObjectFromString<Ver>(tmpclient.GetStringAsync(link).Result);
                if (double.Parse(tmp.version) < double.Parse(content.version))
                {
                    foreach (var item in Guilds)
                    {
                        try
                        {
                            var msg = Client.GetGuild(item.Value.Id).DefaultChannel.SendMessageAsync("", false, Embed.GetEmbed("**Bot Update**", $"An update has been detected!, I will be back once the update has completed!"));
                        }
                        catch (Exception exc)
                        {
                            Log.WriteToLog(exc);
                        }
                    }

                    string url = $"https://github.com/{Program.OwnerGithub}/{Program.BotRepo}/releases/download/{content.version}/Release.zip";
                    string path = $"Release.zip";
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                    using (var client = new System.Net.Http.HttpClient())
                    {
                        var contents = client.GetByteArrayAsync(url).Result;
                        File.WriteAllBytes(path, contents);
                    }

                    Program.Ver = content.version;
                    Config.Config.Save();
                    Environment.Exit(60000);
                }
            }

            foreach (var item in Users.Values)
            {
                item.doshCooldown = 0;
                item.xpcooldown = 0;
            }
        }

        private async Task HandleCommandAsync(SocketMessage messageParam)
        {
            //get guilds prefix and message
            var socketchannel = messageParam.Channel as SocketTextChannel;
            char guildprefix = Guilds.GetValueOrDefault(socketchannel.Guild.Id).Prefix;

            //return if
            if (!(messageParam is SocketUserMessage message)) return;
            var x = message.Content.Replace(" ", "").ToArray().Distinct();
            if (x.Count() <= 1) return;

            //prefix and placeholder
            int argPos = 0;
            if (!(message.HasCharPrefix(guildprefix, ref argPos) || message.HasMentionPrefix(Client.CurrentUser, ref argPos))) return;


            var context = new SocketCommandContext(Client, message);
            var result = await Commands.ExecuteAsync(context, argPos, Services);

            //Log an error
            if (!result.IsSuccess)
            {
                await context.Channel.SendMessageAsync(result.ErrorReason);
                await Log.WriteToLog(result.Error.ToString());
                await Log.WriteToLog(result.ErrorReason);
            }
        }

        public static async Task<uint> GetUserColour(ulong id)
        {
            await Task.CompletedTask;
            return Convert.ToUInt32(Users.GetValueOrDefault(id).hexcol.Replace("#", ""), 16);
        }

        public static async Task SaveUser(ulong id)
        {
            User newUser = Users.GetValueOrDefault(id);
            Json.CreateJson(id.ToString(), $"{Rootdir}\\Users", newUser);
            await Task.CompletedTask;
        }
        public static async Task SaveGuild(ulong id)
        {
            Guild newGuild = Guilds.GetValueOrDefault(id);
            Json.CreateJson(id.ToString(), $"{Rootdir}\\Guilds", newGuild);
            await Task.CompletedTask;
        }
        
        //convert time
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
        public void AutoRole(ulong user, ulong guild)
        {
            ulong[] role;
            try
            {
                role = Guilds.GetValueOrDefault(guild).DefaultRole;
            }
            catch { return; }

            foreach (var item in role)
            {
                try
                {
                    Client.GetGuild(guild).GetUser(user).AddRoleAsync(Client.GetGuild(guild).GetRole(item));
                }
                catch { }
            }
        }
        public static async Task EditUser(User user)
        {
            Users.Remove(user.Id, out User tmp);
            Users.GetOrAdd(user.Id, user);
            await SaveUser(user.Id);
        }
        public static async Task EditGuild(Guild guild)
        {
            Guilds.Remove(guild.Id, out Guild tmp);
            Guilds.GetOrAdd(guild.Id, guild);
            await SaveGuild(guild.Id);
        }

        public static string CalculateMD5(byte[] file)
        {
            using (var md5 = MD5.Create())
            {
                var hash = md5.ComputeHash(file);
                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
        }
    }
    public static class StringExt
    {
        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }

        public static IEnumerable<string> SplitByLength(this string str, int maxLength)
        {
            for (int index = 0; index < str.Length; index += maxLength)
            {
                yield return str.Substring(index, Math.Min(maxLength, str.Length - index));
            }
        }
    }
}
