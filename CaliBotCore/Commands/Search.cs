using Discord.Commands;
using DuckDuckGo.Net;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CaliBotCore.Commands
{
    class SearchCommands : ModuleBase
    {
        readonly string googleApiKey = Program.GoogleApi;


        [Command("Search"), Summary("Returns DuckDuckGo search for query")]
        [Alias("t")]
        public async Task TSearch(string input)
        {
            await ReplyAsync(Searchresults(input));
        }

        [Command("image"), Summary("Returns Image search for query")]
        [Alias("i")]
        public async Task ISearch(string input)
        {
            try
            {
                var reqString = $"https://www.googleapis.com/customsearch/v1?q={Uri.EscapeDataString(input)}&cx=018084019232060951019%3Ahs5piey28-e&num=1&searchType=image&fields=items%2Flink&key={googleApiKey}";
                var obj = JObject.Parse(await GetResponseStringAsync(reqString).ConfigureAwait(false));
                await Context.Channel.SendMessageAsync(obj["items"][0]["link"].ToString()).ConfigureAwait(false);
            }
            catch (HttpRequestException exception)
            {
                if (exception.Message.Contains("403 (Forbidden)")) { await Context.Channel.SendMessageAsync("Daily limit reached!"); }
                else { await Context.Channel.SendMessageAsync("Something went wrong."); }
            }
        }

        [Command("imagerandom"), Summary("Returns Google search result")]
        [Alias("ir")]
        public async Task IRandSearch(string input)
        {
            Random r = new Random();
            try
            {
                var reqString = $"https://www.googleapis.com/customsearch/v1?q= {Uri.EscapeDataString(input)}&cx=018084019232060951019%3Ahs5piey28-e&num=1&searchType=image&start={r.Next(1, 50)}&fields=items%2Flink&key={googleApiKey}";
                var obj = JObject.Parse(await GetResponseStringAsync(reqString).ConfigureAwait(false));
                var items = obj["items"] as JArray;
                await Context.Channel.SendMessageAsync(items[0]["link"].ToString()).ConfigureAwait(false);
            }
            catch (HttpRequestException exception)
            {
                if (exception.Message.Contains("403 (Forbidden)")) { await Context.Channel.SendMessageAsync("Daily limit reached!"); }
                else { await Context.Channel.SendMessageAsync("Something went wrong."); }
            }
        }

        string Searchresults(string input)
        {
            var search = new Search
            {
                NoHtml = true,
                NoRedirects = true,
                IsSecure = true,
                SkipDisambiguation = true,
                ApiClient = new HttpWebApi()
            };
            var result = search.Query(input, "DiscordBot");
            if (result == null)
            {
                return ("No direct results");
            }
            else if (result != null)
            {
                return result.AbstractSource + "\r\n" + result.Abstract + "\r\n" + result.Image;
            }
            else
            {
                return ("Error");
            }
        }
        string Searchimage(string input)
        {
            var search = new Search
            {
                NoHtml = true,
                NoRedirects = true,
                IsSecure = true,
                SkipDisambiguation = true,
                ApiClient = new HttpWebApi()
            };
            var result = search.Query(input, "DiscordBot");
            if (result == null)
            {
                return ("No direct results");
            }
            else if (result != null)
            {
                return result.Image;
            }
            else
            {
                return ("Error");
            }
        }
        public enum RequestHttpMethod
        {
            Get,
            Post
        }
        public static async Task<string> GetResponseStringAsync(string url, IEnumerable<KeyValuePair<string, string>> headers = null, RequestHttpMethod method = RequestHttpMethod.Get)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentNullException(nameof(url));
            var cl = new HttpClient();
            cl.DefaultRequestHeaders.Clear();
            switch (method)
            {
                case RequestHttpMethod.Get:
                    if (headers != null)
                    {
                        foreach (var header in headers)
                        {
                            cl.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
                        }
                    }
                    return await cl.GetStringAsync(url).ConfigureAwait(false);
                case RequestHttpMethod.Post:
                    FormUrlEncodedContent formContent = null;
                    if (headers != null)
                    {
                        formContent = new FormUrlEncodedContent(headers);
                    }
                    var message = await cl.PostAsync(url, formContent).ConfigureAwait(false);
                    return await message.Content.ReadAsStringAsync().ConfigureAwait(false);
                default:
                    throw new NotImplementedException("That type of request is unsupported.");
            }
        }
    }
}
