using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CaliBotCore.Images
{
    class GetFromWeb
    {
        public static async Task<byte[]> PullImageFromWebAsync(string location)
        {
            try
            {
                byte[] data;
                using (WebClient client = new WebClient())
                {
                    data = client.DownloadData(location);
                }
                return data;
            }
            catch (Exception e)
            {
                await Program.Log.WriteToLog(e.Message);
                return null;
            }
        }
    }
}
