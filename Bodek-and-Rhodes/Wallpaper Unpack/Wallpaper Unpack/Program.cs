using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Wallpaper_Unpack
{
    class Program
    {
        static void Main(string[] args)
        {
            string Basepath = @"C:\Users\tturner\Dropbox\IFTTT";



            Console.WriteLine("Please enter folder to unpack :");
            Console.WriteLine(" press 1 for Home Wallpapers press 2 for Work Wallpapers");
            string answer = "";
            string folder = "";
            
                answer=Console.ReadLine();
                switch (answer)
                {
                    case "1":
                         folder = "HomeWallpapers";
                        break;
                    case "2":
                        folder = "WorkWallpapers";
                        break;
                    default:
                        Console.WriteLine("Please select a 1 or a 2 next time");
                        break;

                }
             
           // string folder= Console.ReadLine();
           // string folder = "HomeWallpapers";

            string path = Path.Combine(Basepath, folder);
            DirectoryInfo dirInfo = new DirectoryInfo(path);

            Console.WriteLine(path);

            if( dirInfo.Exists==true)    
            {
                Console.WriteLine(dirInfo.Name);
               
                    List<string> myWallpapers = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories).ToList();

                    foreach (string file in myWallpapers)
                    {

                        FileInfo WFile = new FileInfo(file);

                        if (new FileInfo(path + "\\" + WFile.Name).Exists == false)
                        {
                            WFile.MoveTo(path + "\\" + WFile.Name);
                        }

                        Console.WriteLine(WFile.Name);
                    }
                    
               
            }
            else
                Console.WriteLine("Can not use base folder");
            Console.ReadLine();

            

        }
    }
}
