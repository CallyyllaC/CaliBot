using CaliBotCore.Config;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CaliBotCore.Functions
{

    [Serializable]
    class Waifu
    {
        public string Name = "";
        public int Id = 0;
        public ulong Owner = 0;

        public Waifu(int id, string name, ulong owner)
        {
            Id = id;
            Name = name;
            Owner = owner;
        }

        public static async Task CreateWaifuObject(Waifu newUser)
        {
            Json.CreateJson(newUser.Id.ToString(), $"{Program.Rootdir}\\Waifus", newUser);
            Program.Waifus.GetOrAdd(newUser.Id, newUser);
            await Task.CompletedTask;
        }
    }
}
