using CaliBotCore.DataStructures;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CaliBotCore.Functions
{
    class Wedding
    {
        public static async Task Marry(User one, User two) //cost 10
        {
            one.married = true;
            one.marriedsince = DateTime.Now.ToShortDateString();
            one.partner = two.Id;
            one.pending = 0;

            two.married = true;
            two.marriedsince = DateTime.Now.ToShortDateString();
            two.partner = one.Id;
            two.pending = 0;

            await Program.EditUser(one);
            await Program.EditUser(two);
            await Program.SaveUser(one.Id);
            await Program.SaveUser(two.Id);
        }

        public static async Task Undo(User one, User two) //cost 100
        {
            one.married = false;
            one.marriedsince = null;
            one.partner = 0;
            one.pending = 0;

            two.married = false;
            two.marriedsince = null;
            two.partner = 0;
            two.pending = 0;

            await Program.EditUser(one);
            await Program.EditUser(two);
            await Program.SaveUser(one.Id);
            await Program.SaveUser(two.Id);

        }

        public static bool Pending(ulong user, ulong checkfor)
        {
            if (checkfor == 0)
                return false;

            if (user == checkfor)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
