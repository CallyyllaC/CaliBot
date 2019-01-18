using Kitsu.Anime;
using Kitsu.Character;
using Kitsu.Manga;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CaliBotCore.Functions
{
    class Weeb
    {
        public static async Task<List<AnimeDataModel>> GetAnime(string input, int loop = 10)
        {
            try
            {
                var reply = await Anime.GetAnimeAsync(input);
                var list = new List<AnimeDataModel>();
                if (reply.Data.Count > loop)
                {
                    for (int i = 0; i < loop; i++)
                    {
                        list.Add(reply.Data[i]);
                    }
                }
                else
                {
                    for (int i = 0; i < reply.Data.Count; i++)
                    {
                        list.Add(reply.Data[i]);
                    }
                }
                return list;
            }
            catch
            {
                return null;
            }
        }

        public static async Task<List<AnimeDataModel>> GetAnimeTrending()
        {
            try
            {
                var reply = await Anime.GetTrendingAsync();
                var list = new List<AnimeDataModel>();
                if (reply.Data.Count > 10)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        list.Add(reply.Data[i]);
                    }
                }
                else
                {
                    for (int i = 0; i < reply.Data.Count; i++)
                    {
                        list.Add(reply.Data[i]);
                    }
                }
                return list;
            }
            catch
            {
                return null;
            }
        }

        public static async Task<List<MangaDataModel>> GetManga(string input, int loop = 10)
        {
            try
            {
                var reply = await Manga.GetMangaAsync(input);
                var list = new List<MangaDataModel>();
                if (reply.Data.Count > loop)
                {
                    for (int i = 0; i < loop; i++)
                    {
                        list.Add(reply.Data[i]);
                    }
                }
                else
                {
                    for (int i = 0; i < reply.Data.Count; i++)
                    {
                        list.Add(reply.Data[i]);
                    }
                }
                return list;
            }
            catch
            {
                return null;
            }
        }

        public static async Task<List<MangaDataModel>> GetMangaTrending()
        {
            try
            {
                var reply = await Manga.GetTrendingAsync();
                var list = new List<MangaDataModel>();
                if (reply.Data.Count > 10)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        list.Add(reply.Data[i]);
                    }
                }
                else
                {
                    for (int i = 0; i < reply.Data.Count; i++)
                    {
                        list.Add(reply.Data[i]);
                    }
                }
                return list;
            }
            catch
            {
                return null;
            }
        }

        public static async Task<List<CharacterDataModel>> GetCharacter(string input, int loop = 10)
        {
            try
            {
                var reply = await Character.GetCharacterAsync(input);
                var list = new List<CharacterDataModel>();
                if (reply.Data.Count > loop)
                {
                    for (int i = 0; i < loop; i++)
                    {
                        list.Add(reply.Data[i]);
                    }
                }
                else
                {
                    for (int i = 0; i < reply.Data.Count; i++)
                    {
                        list.Add(reply.Data[i]);
                    }
                }
                return list;
            }
            catch
            {
                return null;
            }
        }

        public static async Task<CharacterByIdModel> GetCharacter(int input)
        {
            try
            {
                return await Character.GetCharacterAsync(input);;
            }
            catch
            {
                return null;
            }
        }
    }
}
