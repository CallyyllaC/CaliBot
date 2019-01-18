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
    public class Marriage : InteractiveBase
    {
        [Command("Marry")]
        [Summary("Sends marriage request to user, costs 10 credits")]
        public async Task Marry(IUser user)
        {
            IUser usersend = Context.Message.Author;
            IUser userrecieve = user;

            if (usersend.Id == userrecieve.Id)
            {
                await ReplyAndDeleteAsync("", false, Embed.GetEmbed($"**What!?!**", $"You can't marry yourself..."), new TimeSpan(0, 0, 15));
                return;
            }

            //check to see if you are already married
            if (Program.Users.GetValueOrDefault(usersend.Id).married)
            {
                await ReplyAsync("", false, Embed.GetEmbed($"**What!?!**", $":angry: You can't be married to more than one person! :angry:"));
                return;
            }
            //check to see if they are already married
            if (Program.Users.GetValueOrDefault(userrecieve.Id).married)
            {
                await ReplyAsync("", false, Embed.GetEmbed($"**Ohno**", $":sob: ...I'm sorry, but they already seem to be married :sob:"));
                return;
            }

            //take money and confirm proposal
            if (Program.Users.GetValueOrDefault(usersend.Id).currency < 10)
            {
                await ReplyAndDeleteAsync("", false, Embed.GetEmbed($"**Oh dear**", $":moneybag: It looks like you're too poor to marry someone, it requires 10 credits :moneybag:"), new TimeSpan(0, 0, 15));
                return;
            }
            else
            {
                Program.Users.GetValueOrDefault(usersend.Id).currency = Program.Users.GetValueOrDefault(usersend.Id).currency - 10;
                Program.Users.GetValueOrDefault(usersend.Id).pending = Program.Users.GetValueOrDefault(userrecieve.Id).Id;
            }

            //send dms
            if (!userrecieve.IsBot)
            {
                await Context.Client.GetUser(Program.Users.GetValueOrDefault(userrecieve.Id).Id).SendMessageAsync("", false, Embed.GetEmbed($"**You've Got Mail**", $":heart: {usersend.Username} has proposed to you! :heart:"));
            }
            if (!usersend.IsBot)
            {
                await Context.Client.GetUser(Program.Users.GetValueOrDefault(usersend.Id).Id).SendMessageAsync("", false, Embed.GetEmbed($"**You've Got Mail**", $":heart: your proposal has been sent to {userrecieve.Username}! :heart:"));
            }

            //if bot automarry -- if this bot decline

            if (userrecieve.IsBot && userrecieve.Id != Program.Botid)
            {
                await Wedding.Marry(Program.Users.GetValueOrDefault(usersend.Id), Program.Users.GetValueOrDefault(userrecieve.Id));
                await Context.Client.GetUser(Program.Users.GetValueOrDefault(usersend.Id).Id).SendMessageAsync("", false, Embed.GetEmbed($"**You've Got Mail**", $":robot: Congratulations you forced {userrecieve.Username} to marry you! :robot:"));
                return;
            }
            else if (userrecieve.Id == Program.Botid)
            {
                await ReplyAsync("", false, Embed.GetEmbed($"**{Program.CurrentName} UserSetup**", $":disappointed: Sorry but I will forever be single, here, have your money back, although I spent a little of it already :disappointed:"));
                Program.Users.GetValueOrDefault(usersend.Id).currency = Program.Users.GetValueOrDefault(usersend.Id).currency + 8;
                return;
            }

            //check to see if they are already proposed to you
            var one = Program.Users.GetValueOrDefault(usersend.Id).pending;
            var two = Program.Users.GetValueOrDefault(userrecieve.Id).pending;
            if (Program.Users.GetValueOrDefault(usersend.Id).Id == Program.Users.GetValueOrDefault(userrecieve.Id).pending)
            {
                await Wedding.Marry(Program.Users.GetValueOrDefault(usersend.Id), Program.Users.GetValueOrDefault(userrecieve.Id));
                await Context.Client.GetUser(Program.Users.GetValueOrDefault(userrecieve.Id).Id).SendMessageAsync("", false, Embed.GetEmbed($"**You've Got Mail**", $":heart: Congratulations you are now married to {usersend.Username}! :heart:"));
                await Context.Client.GetUser(Program.Users.GetValueOrDefault(usersend.Id).Id).SendMessageAsync("", false, Embed.GetEmbed($"**You've Got Mail**", $":heart: Congratulations you are now married to {userrecieve.Username}! :heart:"));
                await ReplyAsync("", false, Embed.GetEmbed($"**Congratulations!**", $":heart: \n{userrecieve.Username} and {usersend.Username} are now married! :heart:"));
            }
        }

        [Command("Divorce")]
        [Summary("Deivorce from your current partner, costs 150 credits")]
        public async Task Divorce()
        {
            IUser usersend = Context.Message.Author;
            IUser userrecieve = Context.Client.GetUser(Program.Users.GetValueOrDefault(usersend.Id).partner);
            //take money
            if (Program.Users.GetValueOrDefault(usersend.Id).currency < 150)
            {
                await ReplyAndDeleteAsync("", false, Embed.GetEmbed($"**Oh dear**", $":moneybag: It looks like you're too poor to divorce someone, it requires 150 credits :moneybag:"), new TimeSpan(0, 0, 15));
                return;
            }
            else
            {
                Program.Users.GetValueOrDefault(usersend.Id).currency = Program.Users.GetValueOrDefault(usersend.Id).currency - 150;
            }

            //if bot automarry -- if this bot decline
            if (userrecieve.IsBot && userrecieve.Id != Program.Botid)
            {
                await Wedding.Undo(Program.Users.GetValueOrDefault(usersend.Id), Program.Users.GetValueOrDefault(userrecieve.Id));
                await Context.Client.GetUser(Program.Users.GetValueOrDefault(usersend.Id).Id).SendMessageAsync("", false, Embed.GetEmbed($"**You've Got Mail**", $":robot: Congratulations you free'd {userrecieve.Username}! :robot:"));
                return;
            }

            //check to see if they are already proposed to you
            await Wedding.Undo(Program.Users.GetValueOrDefault(usersend.Id), Program.Users.GetValueOrDefault(userrecieve.Id));
            await Context.Client.GetUser(Program.Users.GetValueOrDefault(userrecieve.Id).Id).SendMessageAsync("", false, Embed.GetEmbed($"**You've Got Mail**", $":broken_heart: You are no longer married to {usersend.Username}! :broken_heart:"));
            await Context.Client.GetUser(Program.Users.GetValueOrDefault(usersend.Id).Id).SendMessageAsync("", false, Embed.GetEmbed($"**You've Got Mail**", $":broken_heart: You are no longer married to {userrecieve.Username}! :broken_heart:"));
        }

        [Command("MarriageStatus")]
        [Alias("MCheck", "MarriageCheck", "MStatus")]
        [Summary("Checks your current marriage status")]
        public async Task MarriageStatus()
        {
            User user = Program.Users.GetValueOrDefault(Context.Message.Author.Id);

            if (user.married)
            {
                IUser partner = Context.Client.GetUser(user.partner);
                await ReplyAsync("", false, Embed.GetEmbed($"**Marriage Status**", $":heart: You are currently married to {partner.Username}, and have been since {user.marriedsince}! :heart:"));
            }
            else if (user.pending != 0)
            {
                IUser partner = Context.Client.GetUser(user.pending);
                await ReplyAsync("", false, Embed.GetEmbed($"**Marriage Status**", $"You are currently awaiting a reply from {partner.Username}, I hope they get back to you soon :sad:"));
            }
            else
            {
                IUser partner = Context.Client.GetUser(user.pending);
                await ReplyAsync("", false, Embed.GetEmbed($"**Marriage Status**", $"There isn't really anything to tell?"));
            }
        }
    }
}
