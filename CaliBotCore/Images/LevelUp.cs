using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Text;
using SixLabors.Primitives;

namespace CaliBotCore.Images
{
    class LevelUp
    {
        private static string location;
        private static byte[] outputArray = null;
        private static FontFamily font = null;
        private static string hex = null;

        public static async Task<byte[]> MakeLevelAsync(ulong id)
        {
            //get location of image
            location = $"{Program.Rootdir}\\Users\\LevelUp\\{id}.gif";

            //load font
            await GetFont(id);

            //load hex
            await GetHex(id);

            //load the image
            await LoadImage();

            //complete
            await Draw(id);

            outputArray = await CompressFile.CompressGifAsync(outputArray);

            return outputArray;
        }

        //Load Image
        private static async Task LoadImage()
        {
            //load
            using (var image = Image.Load(location))
            using (var stream = new MemoryStream())
            {
                //save as byte array
                image.SaveAsGif(stream);
                outputArray = stream.ToArray();
            }

            //return
            await Task.CompletedTask;
        }

        //Get Font
        private static async Task GetFont(ulong id)
        {
            var fontname = Program.Users.GetValueOrDefault(id).font;

            try
            {
                font = Program.Myfonts.Fonts.Families.Where(x => x.Name == fontname).First();
            }
            catch
            {
                font = Program.Myfonts.Fonts.Families.Where(x => x.Name == "Courier Prime Code").First();
            }

            await Task.CompletedTask;
        }

        //Get Hex
        private static async Task GetHex(ulong id)
        {
            hex = Program.Users.GetValueOrDefault(id).hexcol;

            if (hex == null)
            {
                hex = "#ffffff";
            }

            await Task.CompletedTask;
        }

        //Draw
        private static async Task Draw(ulong id)
        {
            using (var stream = new MemoryStream())
            using (var image = Image.Load(outputArray))
            {
                PointF Point1 = new PointF(50, 60);
                var thefont = new Font(font, 120);
                image.Mutate(x => x.DrawText("Level Up", thefont, Rgba32.FromHex(hex), Point1));

                image.SaveAsGif(stream);
                outputArray = stream.ToArray();
            }
            await Task.CompletedTask;
        }
    }
}

