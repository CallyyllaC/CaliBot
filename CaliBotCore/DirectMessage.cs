using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaliBotCore
{
    class DirectMessage
    {
        IDMChannel OwnerChannel = null;

        //Assigns local variable with passed param
        public DirectMessage(IDMChannel Channel)
        {
            OwnerChannel = Channel;
        }

        public async void ForwardDMs(SocketMessage message, DiscordSocketClient client)
        {
            try
            {
                Console.WriteLine("Recieved new DM");

                foreach (var item in message.Embeds)
                {
                    await OwnerChannel.SendMessageAsync("", false, item);
                }

                var embed = Embed.GetEmbed($"{message.Author.Username} : {message.Author.Id}", message.Content, Program.CurrentColour, $"UTC: {message.Timestamp.UtcDateTime.ToShortTimeString()} Local: {message.Timestamp.LocalDateTime.ToShortTimeString()}", message.Author.GetAvatarUrl());
                await OwnerChannel.SendMessageAsync("", false, embed);

                if (message.Attachments.Count > 0)
                {
                    foreach (var img in message.Attachments)
                    {
                        await OwnerChannel.SendMessageAsync(img.ProxyUrl);
                    }
                }
            }
            catch (Exception e)
            {
                await Program.Log.WriteToLog(e.Message);
            }
        }

        public async void SendMessageAsync(string message)
        {
            try
            {
                await OwnerChannel.SendMessageAsync("", false, Embed.GetEmbed(Program.CurrentName, message, Program.CurrentColour, Program.CurrentName, Program.CurrentUrl));
            }
            catch (Exception e)
            {
                await Program.Log.WriteToLog(e.Message);
            }
        }

        public async Task<bool> IsDMAsync(SocketMessage message, DiscordSocketClient client)
        {
            try
            {
                var currentchannelid = message.Channel.Id;
                var dmchannels = client.DMChannels;
                foreach (var item in dmchannels)
                {
                    if (item.Id == currentchannelid)
                    {
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                await Program.Log.WriteToLog(e.Message);
            }
            return false;
        }

    }
}
