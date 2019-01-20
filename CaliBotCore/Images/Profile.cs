using CaliBotCore.DataStructures;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Drawing;
using SixLabors.ImageSharp.Processing.Text;
using SixLabors.ImageSharp.Processing.Transforms;
using SixLabors.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaliBotCore.Images
{
    class Profile
    {
        private static FontFamily fam = null;
        private static int currency = 0;
        private static int level = 0;
        private static int xp = 0;
        private static string hexcol = null;
        private static float fontMultiplier = 1;

        public static async Task<byte[]> GetProfile(User user, string username, byte[] avatar)
        {
            //get multiplier
            fontMultiplier = user.fontMultiplier;

            //Get font
            await GetProperties(user);
            
            //Draw, optimise and return
            return await CompressFile.CompressGifAsync(await DrawAsync(user.Id, username, avatar));
        }

        private static async Task GetProperties(User user)
        {
            currency = user.currency;
            hexcol = user.hexcol;
            level = user.level;
            xp = user.xp;

            try
            {
                fam = Program.Myfonts.Fonts.Families.Where(x => x.Name == user.font).First();
            }
            catch
            {
                fam = Program.Myfonts.Fonts.Families.Where(x => x.Name == "Courier Prime Code").First();
            }

            await Task.CompletedTask;
        }

        private static async Task<byte[]> DrawAsync(ulong id, string UserName, byte[] Avatar)
        {
            using (MemoryStream output = new MemoryStream())
            using (var overlay = Image.Load($"{Program.Rootdir}\\Users\\Profile\\{id}.png"))
            {
                PointF Point1 = new PointF(10, 340);
                PointF Point2 = new PointF(10, 380);
                PointF Point3 = new PointF(10, 410);
                PointF Point4 = new PointF(10, 440);
                Font font = new Font(fam, 25*fontMultiplier);
                using (var tmpimg = Image.Load(Avatar))
                {
                    tmpimg.Mutate(y => y.Resize(175, 175));
                    overlay.Mutate(x => x
                        .DrawText(UserName, font, Rgba32.FromHex(hexcol), Point1)
                        .DrawText("Level: " + level, font, Rgba32.FromHex(hexcol), Point2)
                        .DrawText("Total XP: " + xp, font, Rgba32.FromHex(hexcol), Point3)
                        .DrawText("Credits: " + currency, font, Rgba32.FromHex(hexcol), Point4)
                        .DrawImage(tmpimg, PixelBlenderMode.Atop, 1, new Point(525, 250))
                    );
                    overlay.SaveAsGif(output);
                }
                await Task.CompletedTask;
                return output.ToArray();
            }
        }

        public static async Task WriteText(FontFamily fam)
        {
            MemoryStream output = new MemoryStream();
            using (var overlay = Image.Load($"{Program.Rootdir}\\Config\\Sample.png"))
            {
                PointF Point1 = new PointF(10, 10);
                Font font2 = new Font(fam, 72*fontMultiplier);
                overlay.Mutate(x => x
                    .DrawText("Sample Text", font2, Rgba32.FromHex("#000000"), Point1)
                );
                overlay.SaveAsPng(output);
                await File.WriteAllBytesAsync($"{Program.Rootdir}\\Resources\\Fonts\\{fam.Name}.png", output.ToArray());
            }
        }
    }
}
