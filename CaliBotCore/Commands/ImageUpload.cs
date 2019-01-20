using CaliBotCore.Images;
using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaliBotCore.Commands
{
    public class ImageUpload : InteractiveBase
    {
        [Command("SetLevelup", RunMode = RunMode.Async)]
        [Summary("Set your levelup image, Resize modes: \"Stretch(Default), Pad, or Crop\"")]
        public async Task UploadLevelUp(string ResizeMode = "stretch")
        {
            var tmp = await ReplyAsync("", false, Embed.GetEmbed("Set Levelup", "Working..."));
            if (Context.Message.Attachments.Count > 0)
            {
                switch (ResizeMode.ToLower())
                {
                    case "stretch":
                        await File.WriteAllBytesAsync($"{Program.Rootdir}\\Users\\LevelUp\\{Context.Message.Author.Id}.gif", await Resize.ImageResizeStretchAsync(await GetFromWeb.PullImageFromWebAsync(Context.Message.Attachments.First().ProxyUrl), 1200, 480));
                        break;

                    case "pad":
                        await File.WriteAllBytesAsync($"{Program.Rootdir}\\Users\\LevelUp\\{Context.Message.Author.Id}.gif", await Resize.ImageResizePadAsync(await GetFromWeb.PullImageFromWebAsync(Context.Message.Attachments.First().ProxyUrl), 1200, 480));
                        break;

                    case "crop":
                        await File.WriteAllBytesAsync($"{Program.Rootdir}\\Users\\LevelUp\\{Context.Message.Author.Id}.gif", await Resize.ImageResizeCropAsync(await GetFromWeb.PullImageFromWebAsync(Context.Message.Attachments.First().ProxyUrl), 1200, 480));
                        break;

                    default:
                        await File.WriteAllBytesAsync($"{Program.Rootdir}\\Users\\LevelUp\\{Context.Message.Author.Id}.gif", await Resize.ImageResizeStretchAsync(await GetFromWeb.PullImageFromWebAsync(Context.Message.Attachments.First().ProxyUrl), 1200, 480));
                        break;
                }
                Program.Users.GetValueOrDefault(Context.Message.Author.Id).changed = true;
                await Program.SaveUser(Context.Message.Author.Id);
            }
            else
            {
                await tmp.ModifyAsync(x => x.Embed = Embed.GetEmbed("Set Levelup", "Sorry but you did not attach an image or link"));
                GC.Collect();
                return;
            }
            await tmp.ModifyAsync(x => x.Embed = Embed.GetEmbed("Set Levelup", "Done"));
            GC.Collect();
        }

        [Command("SetLevelup", RunMode = RunMode.Async)]
        [Summary("Set your levelup image, Resize modes: \"Stretch(Default), Pad, or Crop\"")]
        public async Task UploadLevelUp(string ImageLink, string ResizeMode = "stretch")
        {
            if (Context.Message.Attachments.Count > 0)
            {
                await UploadLevelUp(ResizeMode);
                return;
            }

            var tmp = await ReplyAsync("", false, Embed.GetEmbed("Set Levelup", "Working..."));
            try
            {
                switch (ResizeMode.ToLower())
                {
                    case "stretch":
                        await File.WriteAllBytesAsync($"{Program.Rootdir}\\Users\\LevelUp\\{Context.Message.Author.Id}.gif", await Resize.ImageResizeStretchAsync(await GetFromWeb.PullImageFromWebAsync(ImageLink), 600, 240));
                        break;

                    case "pad":
                        await File.WriteAllBytesAsync($"{Program.Rootdir}\\Users\\LevelUp\\{Context.Message.Author.Id}.gif", await Resize.ImageResizePadAsync(await GetFromWeb.PullImageFromWebAsync(ImageLink), 600, 240));
                        break;

                    case "crop":
                        await File.WriteAllBytesAsync($"{Program.Rootdir}\\Users\\LevelUp\\{Context.Message.Author.Id}.gif", await Resize.ImageResizeCropAsync(await GetFromWeb.PullImageFromWebAsync(ImageLink), 600, 240));
                        break;

                    default:
                        await File.WriteAllBytesAsync($"{Program.Rootdir}\\Users\\LevelUp\\{Context.Message.Author.Id}.gif", await Resize.ImageResizeStretchAsync(await GetFromWeb.PullImageFromWebAsync(ImageLink), 600, 240));
                        break;
                }
                Program.Users.GetValueOrDefault(Context.Message.Author.Id).changed = true;
                await Program.SaveUser(Context.Message.Author.Id);
            }
            catch
            {
                await tmp.ModifyAsync(x => x.Embed = Embed.GetEmbed("Set Levelup", "Sorry but you did not attach an image or link"));
                GC.Collect();
                return;
            }
            await tmp.ModifyAsync(x => x.Embed = Embed.GetEmbed("Set Levelup", "Done"));
            GC.Collect();
        }

        [Command("SetProfile", RunMode = RunMode.Async)]
        [Summary("Set your profile image, Resize modes: \"Stretch(Default), Pad, or Crop\"")]
        public async Task UploadProfile(string ResizeMode = "stretch")
        {
            var tmp = await ReplyAsync("", false, Embed.GetEmbed("Set Profile", "Working..."));
            if (Context.Message.Attachments.Count > 0)
            {
                switch (ResizeMode.ToLower())
                {
                    case "stretch":
                        await File.WriteAllBytesAsync($"{Program.Rootdir}\\Users\\Profile\\{Context.Message.Author.Id}.png", await Resize.ImageResizeStretchAsync(await GetFromWeb.PullImageFromWebAsync(Context.Message.Attachments.First().ProxyUrl), 1200, 480));
                        break;

                    case "pad":
                        await File.WriteAllBytesAsync($"{Program.Rootdir}\\Users\\Profile\\{Context.Message.Author.Id}.png", await Resize.ImageResizePadAsync(await GetFromWeb.PullImageFromWebAsync(Context.Message.Attachments.First().ProxyUrl), 1200, 480));
                        break;

                    case "crop":
                        await File.WriteAllBytesAsync($"{Program.Rootdir}\\Users\\Profile\\{Context.Message.Author.Id}.png", await Resize.ImageResizeCropAsync(await GetFromWeb.PullImageFromWebAsync(Context.Message.Attachments.First().ProxyUrl), 1200, 480));
                        break;

                    default:
                        await File.WriteAllBytesAsync($"{Program.Rootdir}\\Users\\Profile\\{Context.Message.Author.Id}.png", await Resize.ImageResizeStretchAsync(await GetFromWeb.PullImageFromWebAsync(Context.Message.Attachments.First().ProxyUrl), 1200, 480));
                        break;
                }
            }
            else
            {
                await tmp.ModifyAsync(x => x.Embed = Embed.GetEmbed("Set Profile", "Sorry but you did not attach an image or link"));
                GC.Collect();
                return;
            }
            await tmp.ModifyAsync(x => x.Embed = Embed.GetEmbed("Set Profile", "Done"));
            GC.Collect();
        }

        [Command("SetProfile", RunMode = RunMode.Async)]
        [Summary("Set your profile image, Resize modes: \"Stretch(Default), Pad, or Crop\"")]
        public async Task UploadProfile(string ImageLink, string ResizeMode = "stretch")
        {
            if (Context.Message.Attachments.Count > 0)
            {
                await UploadProfile(ResizeMode);
                return;
            }

            var tmp = await ReplyAsync("", false, Embed.GetEmbed("Set Profile", "Working..."));
            try
            {
                switch (ResizeMode.ToLower())
                {
                    case "stretch":
                        await File.WriteAllBytesAsync($"{Program.Rootdir}\\Users\\Profile\\{Context.Message.Author.Id}.png", await Resize.ImageResizeStretchAsync(await GetFromWeb.PullImageFromWebAsync(ImageLink), 1200, 480));
                        break;

                    case "pad":
                        await File.WriteAllBytesAsync($"{Program.Rootdir}\\Users\\Profile\\{Context.Message.Author.Id}.png", await Resize.ImageResizePadAsync(await GetFromWeb.PullImageFromWebAsync(ImageLink), 1200, 480));
                        break;

                    case "crop":
                        await File.WriteAllBytesAsync($"{Program.Rootdir}\\Users\\Profile\\{Context.Message.Author.Id}.png", await Resize.ImageResizeCropAsync(await GetFromWeb.PullImageFromWebAsync(ImageLink), 1200, 480));
                        break;

                    default:
                        await File.WriteAllBytesAsync($"{Program.Rootdir}\\Users\\Profile\\{Context.Message.Author.Id}.png", await Resize.ImageResizeStretchAsync(await GetFromWeb.PullImageFromWebAsync(ImageLink), 1200, 480));
                        break;
                }
            }
            catch
            {
                await tmp.ModifyAsync(x => x.Embed = Embed.GetEmbed("Set Profile", "Sorry but you did not attach an image or link"));
                GC.Collect();
                return;
            }
            await tmp.ModifyAsync(x => x.Embed = Embed.GetEmbed("Set Profile", "Done"));
            GC.Collect();
        }

        [Command("GetLevelup", RunMode = RunMode.Async)]
        [Summary("Get current Levelup image")]
        public async Task GetLevelUp(IUser UserTag = null)
        {
            if (UserTag == null)
            {
                UserTag = Context.Message.Author as IUser;
            }

            var tmp = await ReplyAsync("", false, Embed.GetEmbed("Get Levelup", "Working..."));

            if (!File.Exists($"{Program.Rootdir}\\Users\\LevelUp\\{UserTag.Id}.gif"))
            {
                await tmp.ModifyAsync(x => x.Embed = Embed.GetEmbed("Get Levelup", "You havn't uploaded a levelup image yet"));
                return;
            }
            else if (File.Exists($"{Program.Rootdir}\\Users\\LevelUp\\{UserTag.Id}Processed.gif") && Program.Users.GetValueOrDefault(UserTag.Id).changed == false)
            {
                try
                {
                    await Context.Message.Channel.SendFileAsync($"{Program.Rootdir}\\Users\\LevelUp\\{UserTag.Id}Processed.gif");
                }
                catch
                {
                    await tmp.ModifyAsync(x => x.Embed = Embed.GetEmbed("Get Levelup", "Sorry there was an error"));
                    GC.Collect();
                    return;
                }
            }
            else
            {
                try
                {
                    var tmpimg = await LevelUp.MakeLevelAsync(UserTag.Id);

                    if (tmpimg.Length > 8000000)
                    {
                        await tmp.ModifyAsync(x => x.Embed = Embed.GetEmbed("Get Levelup", "File too big"));
                        GC.Collect();
                        return;
                    }
                    else
                    {
                        File.WriteAllBytes($"{Program.Rootdir}\\Users\\LevelUp\\{UserTag.Id}Processed.gif", tmpimg);
                        Program.Users.GetValueOrDefault(UserTag.Id).changed = false;
                        await Program.SaveUser(UserTag.Id);
                        await Context.Message.Channel.SendFileAsync($"{Program.Rootdir}\\Users\\LevelUp\\{UserTag.Id}Processed.gif");
                    }
                }
                catch
                {
                    await tmp.ModifyAsync(x => x.Embed = Embed.GetEmbed("Get Levelup", "Sorry there was an error"));
                    GC.Collect();
                    return;
                }
            }

            await tmp.ModifyAsync(x => x.Embed = Embed.GetEmbed("Get Levelup", "Done"));
            System.Threading.Thread.Sleep(5000);
            await tmp.DeleteAsync();
            GC.Collect();
        }

        [Command("GetProfile", RunMode = RunMode.Async)]
        [Summary("Get current Profile image"), Alias("Profile", "MyProfile")]
        public async Task GetProfile(IGuildUser UserTag = null)
        {
            if (UserTag == null)
            {
                UserTag = Context.Message.Author as IGuildUser;
            }

            var tmp = await ReplyAsync("", false, Embed.GetEmbed("Profile", "Working..."));

            if (!File.Exists($"{Program.Rootdir}\\Users\\Profile\\{UserTag.Id}.png"))
            {
                await tmp.ModifyAsync(x => x.Embed = Embed.GetEmbed("Profile", "You havn't uploaded a profile image yet"));
                return;
            }

            try
            {
                var avatar = await GetFromWeb.PullImageFromWebAsync(UserTag.GetAvatarUrl());
                string name = UserTag.Nickname ?? UserTag.Username;
                var tmpimg = await Profile.GetProfile(Program.Users.GetValueOrDefault(UserTag.Id), name, avatar);

                if (tmpimg.Length > 8000000)
                {
                    await tmp.ModifyAsync(x => x.Embed = Embed.GetEmbed("Get Profile", "File too big"));
                    GC.Collect();
                    return;
                }
                else
                {
                    var file = await CompressFile.CheckForDupesAndSave(tmpimg);
                    await Context.Message.Channel.SendFileAsync(file);
                    File.Delete(file);
                }
            }
            catch
            {
                await tmp.ModifyAsync(x => x.Embed = Embed.GetEmbed("Get Profile", "Sorry there was an error"));
                GC.Collect();
                return;
            }

            await tmp.ModifyAsync(x => x.Embed = Embed.GetEmbed("Get Profile", "Done"));
            System.Threading.Thread.Sleep(5000);
            await tmp.DeleteAsync();
            GC.Collect();
        }
    }
}
