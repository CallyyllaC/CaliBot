using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SixLabors.Fonts;

namespace CaliBotCore.Resources
{
    public class MyFonts
    {
        public FontCollection Fonts = new FontCollection();
        
        public MyFonts()
        {
            foreach (var item in Directory.GetFiles($"{Program.Rootdir}\\Resources\\Fonts\\", "*.ttf"))
            {
                try
                {
                    Console.WriteLine($"Installed Font: {item.Replace($"{Program.Rootdir}\\Resources\\Fonts\\", "")}");
                    Fonts.Install(item);
                }
                catch (Exception e)
                {
                    Program.Log.WriteToLog(e.Message + item.Replace($"{Program.Rootdir}\\Resources\\Fonts\\", ""));
                }
            }
            foreach (var item in Directory.GetFiles($"{Program.Rootdir}\\Resources\\Fonts\\", "*.otf"))
            {
                try
                {
                    Console.WriteLine($"Installed Font: {item.Replace($"{Program.Rootdir}\\Resources\\Fonts\\", "")}");
                    Fonts.Install(item);
                }
                catch (Exception e)
                {
                    Program.Log.WriteToLog(e.Message + item.Replace($"{Program.Rootdir}\\Resources\\Fonts\\", ""));
                }
            }
        }
    }
}
