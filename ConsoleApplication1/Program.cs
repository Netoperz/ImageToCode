using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace ConsoleApplication1
{
    class Program
    {

        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    string path = args[i];
                    string picName = Path.GetFileNameWithoutExtension(path);
                    string hPath = picName + ".c";

                    // M5Stack LCD resolution
                    Size LCD = new Size();
                    LCD.Width = 320;
                    LCD.Height = 240;

                    Bitmap img = new Bitmap(path);
                    // Resize
                    Size imgSize = img.Size;
                    if (!((img.Size.Width <= LCD.Width) && (img.Size.Height <= LCD.Height)))
                    {
                        double imgWidth = Convert.ToDouble(img.Width);
                        double imgHeight = Convert.ToDouble(img.Height);
                        double p = imgWidth / imgHeight;
                        bool whm = (imgWidth > imgHeight) ? true : false;
                        if (whm)
                        {
                            imgSize.Width = LCD.Width;
                            imgSize.Height = Convert.ToInt32(imgSize.Width / p);
                        }
                        else
                        {
                            imgSize.Height = LCD.Height;
                            imgSize.Width = Convert.ToInt32(imgSize.Height * p);
                        }
                        img = new Bitmap(img, imgSize);
                    }

                    // Save
                    //img.Save(path + "_", ImageFormat.Bmp);

                    // Print array
                    string bmpArray = "const unsigned char " + picName + "[" + (LCD.Height * LCD.Width * 2) + "] = {";
                    for (int h = 0; h < LCD.Height; h++)
                    {
                        Console.Clear();
                        Console.Write("Converting: " + (Convert.ToInt32(h * 100 / LCD.Height)) + " %");
                        for (int w = 0; w < LCD.Width; w++)
                        {
                            UInt16 c;
                            if ((h >= img.Height) || (w >= img.Width))
                            {
                                c = 0x0000;
                            }
                            else
                            {
                                Color px = img.GetPixel(w, h);
                                c = Convert.ToByte(px.R >> 3);
                                c <<= 6;
                                c |= Convert.ToByte(px.G >> 2);
                                c <<= 5;
                                c |= Convert.ToByte(px.B >> 3);
                            }
                            bmpArray += ("0x" + Convert.ToString((c >> 8), 16) + ", 0x" + Convert.ToString((c & 0xFF), 16)) + ", ";
                        }
                    }
                    bmpArray = bmpArray.Remove(bmpArray.Length - 2, 2);
                    bmpArray += "};";
                    img.Dispose();
                    if (File.Exists(hPath))
                    {
                        File.Delete(hPath);
                    }
                    File.WriteAllText(hPath, bmpArray, Encoding.UTF8);
                }
            }
            else
            {
                Console.WriteLine("Please, drop image files on this app icon");
                Console.ReadLine();
            }
        }
    }
}
