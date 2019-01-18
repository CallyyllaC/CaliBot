using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Transforms;
using SixLabors.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CaliBotCore.Images
{
    class Resize
    {
        public static async Task<Byte[]> ImageResizePadAsync(Byte[] newimage, int size1, int size2)
        {
            Byte[] save = null;
            var size = new Size(size1, size2);
            ResizeOptions options = new ResizeOptions() { Mode = ResizeMode.Pad, Size = new Size(size1, size2) };
            using (var image = Image.Load(newimage))
            {
                image.Mutate(x => x.Resize(options));
                var gif = new MemoryStream();
                image.SaveAsGif(gif);
                save = gif.ToArray();
            }
            await Task.CompletedTask;
            return save;
        }
        public static async Task<Byte[]> ImageResizeCropAsync(Byte[] newimage, int size1, int size2)
        {
            Byte[] save = null;
            var size = new Size(size1, size2);
            ResizeOptions options = new ResizeOptions() { Mode = ResizeMode.Crop, Size = new Size(size1, size2) };
            using (var image = Image.Load(newimage))
            {
                image.Mutate(x => x.Resize(options));
                var gif = new MemoryStream();
                image.SaveAsGif(gif);
                save = gif.ToArray();
            }
            await Task.CompletedTask;
            return save;
        }
        public static async Task<Byte[]> ImageResizeStretchAsync(Byte[] newimage, int size1, int size2)
        {
            Byte[] save = null;
            var size = new Size(size1, size2);
            ResizeOptions options = new ResizeOptions() { Mode = ResizeMode.Stretch, Size = new Size(size1, size2) };
            using (var image = Image.Load(newimage))
            {
                image.Mutate(x => x.Resize(options));
                var gif = new MemoryStream();
                image.SaveAsGif(gif);
                save = gif.ToArray();
            }
            await Task.CompletedTask;
            return save;
        }
    }
}
