using MoarBooruPraser;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CaliBotCore.Functions
{
    class NotSafeForWork
    {
        static Random r = new Random();
        public static async Task<GelStruct> GetGelImageAsync(string search)
        {
            try
            {
                var results = await Gelbooru.SearchPostsAsync(search, 10000);
                return results[r.Next(results.Count)];
            }
            catch
            {
                return null;
            }
        }

        public static async Task<YanStruct> GetYanImageAsync(string search)
        {
            try
            {
                var results = await Yandere.SearchPostsAsync(search, 10000);
                return results[r.Next(results.Count)];
            }
            catch
            {
                return null;
            }
        }
    }
}
