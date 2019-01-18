using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaliBotCore.DataStructures
{
    [Serializable]
    class User
    {
        public ulong Id;
        public bool Setup = false;

        //perms
        public bool Blacklisted = false;

        //cooldowns
        public int doshCooldown = 0;
        public int xpcooldown = 0;

        //leveling
        public int level = 0;
        public int xp = 0;
        public int currency = 0;

        //image stuff
        public string font = null;
        public string hexcol = "#000000";
        public bool changed = false;

        //marriage
        public bool married = false;
        public ulong partner = 0;
        public ulong pending = 0;
        public string marriedsince = "";

        //waifu counter
        public string lastWaifu = "";
    }
}
