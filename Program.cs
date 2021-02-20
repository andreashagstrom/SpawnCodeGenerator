using System;
using System.IO;


namespace ArkSpawnCodeGen
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (var arg in args)
            {
                if (arg == "-cheese")
                {
                    Console.WriteLine("Fuck you cheese you little shitheel!");
                    Console.ReadKey();
                }
                if (arg == "-Kane")
                {
                    Console.WriteLine("You know you love me kane! Dont try to hide it!!!!!");
                    Console.ReadKey();
                }
            }
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
                string itemsHeader = "---------------------------------------------------------------------------------Item Spawn Codes---------------------------------------------------------------------------------";
                string dinoHeader = "---------------------------------------------------------------------------------Dino Spawn Codes---------------------------------------------------------------------------------";
                var allItems = Directory.GetFiles(Directory.GetCurrentDirectory(), "*", SearchOption.AllDirectories);
                File.Delete("SpawnCodes.txt"); //this will wipe the text file so a clean set of codes can be re generated
                File.AppendAllText("SpawnCodes.txt", itemsHeader + Environment.NewLine);// this add the item header to show that everythign below is an item spawn code
                foreach (var item in allItems)
                {
                    var filename = Path.GetFileNameWithoutExtension(item);

                    if (filename.StartsWith("PrimalItem"))
                    {

                        var s = @"admincheat GiveItem " + ((char)34) + "Blueprint'" + item.Substring(item.IndexOf("Content")).Replace(@"Content\", @"\Game\").Replace(".uasset", "." + filename).Replace(@"\", "/") + "'" + ((char)34) + " 1 1 0";

                        File.AppendAllText("SpawnCodes.txt", s + Environment.NewLine);

                        Console.WriteLine(s);
                    }
                }
                File.AppendAllText("SpawnCodes.txt", dinoHeader + Environment.NewLine);
                foreach (var item in allItems)
                {
                    var filename = Path.GetFileNameWithoutExtension(item);

                    if (filename.Contains("Character_BP"))
                    {

                        var s = @"admincheat SpawnDino " + ((char)34) + "Blueprint'" + item.Substring(item.IndexOf("Content")).Replace(@"Content\", @"\Game\").Replace(".uasset", "." + filename).Replace(@"\", "/") + "'" + ((char)34) + " 500 0 0 120";

                        File.AppendAllText("SpawnCodes.txt", s + Environment.NewLine);

                    }
                }
            }
            if (isArkModFolder())
            {
                MakeSpawnCodes();
            }
            else
            {
                Console.WriteLine("Come on dumb ass this isnt a valid mod folder. Next time try to put me in a folder with a valid game data bp smh.");
                Console.ReadKey();
            }
        }
    }
}
