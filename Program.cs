using System;
using System.IO;


namespace ArkSpawnCodeGen
{
    class Program
    {
        static void Main(string[] args)
        {

            string[] files = Directory.GetFiles(Directory.GetCurrentDirectory());
            int amtFiles = files.Length;
            int fileCount = 0;
            bool isArkModFolder()
            {
                foreach (string s in files)
                {
                    string filename = Path.GetFileNameWithoutExtension(s);
                    if (!filename.StartsWith("PrimalGameData"))
                    {
                        if (fileCount == amtFiles - 1)
                        {
                            return false;
                        }
                        else
                        {
                            fileCount++;
                        }
                    }
                    else
                    {
                        return true;
                    }
                }
                return false;
            }

            void MakeSpawnCodes()
            {
                string author = "\nCode generated with ARKMod.net's ARK Code Generator. For latest version, visit https://arkmod.net/.\nHappy ARKing!";
                string engramsHeader = "\n---------------------------------------------------------------------------------Engram Names---------------------------------------------------------------------------------\n";
                string itemsHeader = "\n---------------------------------------------------------------------------------Item Spawncodes--------------------------------------------------------------------------------\n";
                string dinoHeader = "\n---------------------------------------------------------------------------------Creature Spawncodes-----------------------------------------------------------------------------\n";
                string dinoTHeader = "\n---------------------------------------------------------------------------------Tamed Creature Spawncodes-----------------------------------------------------------------------\n";
                var allItems = Directory.GetFiles(Directory.GetCurrentDirectory(), "*", SearchOption.AllDirectories);
                File.Delete("Output.txt"); //this will wipe the text file so a clean set of codes can be re generated
                File.AppendAllText("Output.txt", author + Environment.NewLine);
                File.AppendAllText("Output.txt", engramsHeader + Environment.NewLine);
                foreach (var item in allItems)
                {
                    var filename = Path.GetFileNameWithoutExtension(item);

                    if (filename.StartsWith("EngramEntry"))
                    {

                        var s = filename + "_C";

                        File.AppendAllText("Output.txt", s + Environment.NewLine);

                        Console.WriteLine(s);
                    }
                }
                File.AppendAllText("Output.txt", itemsHeader + Environment.NewLine);// this add the item header to show that everythign below is an item spawn code
                foreach (var item in allItems)
                {
                    var filename = Path.GetFileNameWithoutExtension(item);

                    if (filename.StartsWith("PrimalItem"))
                    {

                        var s = @"admincheat GiveItem " + ((char)34) + "Blueprint'" + item.Substring(item.IndexOf("Content")).Replace(@"Content\", @"\Game\").Replace(".uasset", "." + filename).Replace(@"\", "/") + "'" + ((char)34) + " 1 1 0";

                        File.AppendAllText("Output.txt", s + Environment.NewLine);

                        Console.WriteLine(s);
                    }
                }
                File.AppendAllText("Output.txt", dinoHeader + Environment.NewLine);
                foreach (var item in allItems)
                {
                    var filename = Path.GetFileNameWithoutExtension(item);

                    if (filename.Contains("Character_BP"))
                    {

                        var s = @"admincheat SpawnDino " + ((char)34) + "Blueprint'" + item.Substring(item.IndexOf("Content")).Replace(@"Content\", @"\Game\").Replace(".uasset", "." + filename).Replace(@"\", "/") + "'" + ((char)34) + " 500 0 0 120";

                        File.AppendAllText("Output.txt", s + Environment.NewLine);

                        Console.WriteLine(s);

                    }
                }
                File.AppendAllText("Output.txt", dinoTHeader + Environment.NewLine);
                foreach (var item in allItems)
                {
                    var filename = Path.GetFileNameWithoutExtension(item);

                    if (filename.Contains("Character_BP"))
                    {

                        var s = @"admincheat GMSummon " + ((char)34) + filename + ((char)34) + " 120";

                        File.AppendAllText("Output.txt", s + Environment.NewLine);

                        Console.WriteLine(s);

                    }
                }
                File.AppendAllText("Output.txt", author + Environment.NewLine);
            }
            if (isArkModFolder())
            {
                MakeSpawnCodes();
            }
            else
            {
                Console.WriteLine("This folder dont have a PrimalGameData.");
                Console.ReadKey();
            }
        }
    }
}
