using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace CaliBotCore
{
    class Rainbow
    {
        List<SocketRole> rroles = new List<SocketRole>();
        Timer timer = new Timer();
        Random r = new Random();
        List<Color> colors = new List<Color>();
        int i = 0;
        public Rainbow()
        {
            timer.Interval = 60000;
            timer.AutoReset = true;
            timer.Elapsed += Timer_ElapsedAsync;
            timer.Start();

            Getnewrgb();

            foreach (var Guild in Program.Client.Guilds)
            {
                try
                {
                    rroles.Add(Guild.Roles.Single(x => x.Name == "Rainbow"));
                }
                catch
                {
                    if (Guild.Roles.Where(x => x.Name == "Rainbow").Count() > 1)
                    {
                        Guild.DefaultChannel.SendMessageAsync("Error, found more than one role \"Rainbow\"").RunSynchronously();
                    }
                    else
                    {
                        CreateRole(Guild).RunSynchronously();
                    }
                }
            }
        }

        public async Task CreateRole(SocketGuild Guild)
        {
            var tmp = await Guild.CreateRoleAsync("Rainbow");
            await Guild.DefaultChannel.SendMessageAsync("Created new rainbow role, an admin will need to move this in the role heirarchy");
            var tmp2 = Guild.GetRole(tmp.Id);
            rroles.Add(tmp2);
        }

        private void Timer_ElapsedAsync(object sender, ElapsedEventArgs e)
        {
            timer.Stop();
            foreach (var item in rroles)
            {
                try
                {
                    item.ModifyAsync(ChangeColour);
                }
                catch (Exception er)
                {
                    Program.Log.WriteToLog(er.Message);
                }
            }
            timer.Start();
        }

        private void Getnewrgb()
        {
            int x = 30;
            for (int r = 0; r < x; r++) colors.Add(new Color(r * 255 / x, 255, 0));
            for (int g = x; g > 0; g--) colors.Add(new Color(255, g * 255 / x, 0));
            for (int b = 0; b < x; b++) colors.Add(new Color(255, 0, b * 255 / x));
            for (int r = x; r > 0; r--) colors.Add(new Color(r * 255 / x, 0, 255));
            for (int g = 0; g < x; g++) colors.Add(new Color(0, g * 255 / x, 255));
            for (int b = x; b > 0; b--) colors.Add(new Color(0, 255, b * 255 / x));
            colors.Add(new Color(0, 255, 0));
        }

        private void ChangeColour(RoleProperties obj)
        {
            if (i >= colors.Count)
            {
                i = 0;
            }
            obj.Color = colors[i];
            i++;
        }
    }
}
