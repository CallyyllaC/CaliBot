using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using CaliBotCore.DataStructures;
using Discord;

namespace CaliBotCore.Functions
{
    class Ranks
    {
        public static async Task<int> GetGlobalRank(ulong id)
        {
            List<User> users = new List<User>();
            users.AddRange(Program.Users.Values);
            users = users.OrderBy(x => x.xp).Reverse().ToList();
            var place = users.IndexOf(users.Find(x => x.Id == id));

            await Task.CompletedTask;
            return place + 1;
        }

        public static async Task<int> GetLocalRank(IGuild guild, ulong id)
        {
            List<User> users = await GetLocalUsers(guild);
            users = users.OrderBy(x => x.xp).Reverse().ToList();
            var place = users.IndexOf(users.Find(x => x.Id == id));

            await Task.CompletedTask;
            return place + 1;
        }

        public static async Task<int> GetGlobalDosh(ulong id)
        {
            List<User> users = new List<User>();
            users.AddRange(Program.Users.Values);
            users = users.OrderBy(x => x.currency).Reverse().ToList();
            var place = users.IndexOf(users.Find(x => x.Id == id));

            await Task.CompletedTask;
            return place + 1;
        }

        public static async Task<int> GetLocalDosh(IGuild guild, ulong id)
        {
            List<User> users = await GetLocalUsers(guild);
            users = users.OrderBy(x => x.currency).Reverse().ToList();
            var place = users.IndexOf(users.Find(x => x.Id == id));

            await Task.CompletedTask;
            return place + 1;
        }

        public static async Task<List<User>> GetGlobal()
        {
            List<User> users = new List<User>();
            users.AddRange(Program.Users.Values);
            users = users.OrderBy(x => x.xp).Reverse().ToList();

            await Task.CompletedTask;
            return users;
        }
        public static async Task<List<User>> GetGlobalDosh()
        {
            List<User> users = new List<User>();
            users.AddRange(Program.Users.Values);
            users = users.OrderBy(x => x.currency).Reverse().ToList();

            await Task.CompletedTask;
            return users;
        }
        public static async Task<List<User>> GetLocal(IGuild guild)
        {
            List<User> users = await GetLocalUsers(guild);
            users = users.OrderBy(x => x.xp).Reverse().ToList();

            await Task.CompletedTask;
            return users;
        }
        public static async Task<List<User>> GetLocalDosh(IGuild guild)
        {
            List<User> users = await GetLocalUsers(guild);
            users = users.OrderBy(x => x.currency).Reverse().ToList();

            await Task.CompletedTask;
            return users;
        }

        private static async Task<List<User>> GetLocalUsers(IGuild guild)
        {
            List<User> users = new List<User>();
            List<ulong> ids = new List<ulong>();

            var guilds = await guild.GetUsersAsync();            
            foreach (var item in guilds)
            {
                ids.Add(item.Id);
            }
            users.AddRange(Program.Users.Values.Where(x => ids.Contains(x.Id)));

            await Task.CompletedTask;
            return users;
        }
    }
}
