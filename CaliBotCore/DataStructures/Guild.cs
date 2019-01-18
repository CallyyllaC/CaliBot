using CaliBotCore.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaliBotCore.DataStructures
{
    [Serializable]
    class Guild
    {
        public ulong Id;
        public char Prefix = '!';
        public bool Blacklisted = false;
        public ulong[] DefaultRole = new ulong[0];
        public bool Music = false;
        public ulong LevelupChannel = 0;
        public bool Activity = true;
        public int Nep = 0;
        public ulong NationsChannel = 0;
        public Nation[] Nations = new Nation[0];
    }
}
