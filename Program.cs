using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ArkSpawnCodeGen
{
    class Program
    {
        /// <summary>
        /// Constants for the file names
        /// </summary>
        private const string PREFIX = "ArkSpawnCodeGen_";
        private const string PRIMAL_ITEMS_PREFIX = PREFIX + "Primal_Items_";
        private const string SPAWNCODES_PREFIX = PREFIX + "Spawncodes_";

        private const string SUMMERY_FILE = PREFIX + "Summery";
        private const string BLUEPRINT_FILE = PREFIX + "Blueprints";
        private const string ENGRAM_FILE = PREFIX + "Engrams";
        private const string PRIMAL_ITEMS_FILE = PRIMAL_ITEMS_PREFIX + "Items";
        private const string SPAWNCODE_ITEMS_FILE = SPAWNCODES_PREFIX + "Items";
        private const string SPAWNCODE_CREATURE_FILE = SPAWNCODES_PREFIX + "Creatures";

        private const string OUTPUT_PATH = "Output/";
        private static string PATH_PREFIX = "/Game/Mods/";

        /// <summary>
        /// List of all read files
        /// </summary>
        private static List<Summery> summeryFile = new List<Summery>();
        private static List<string> blueprintFile = new List<string>();
        private static List<string> engramFile = new List<string>();
        private static List<string> primalItemsFile = new List<string>();
        private static List<string> spawncodeItemsFile = new List<string>();
        private static List<string> spawncodeCreatureFile = new List<string>();

        /// <summary>
        /// Save all found files
        /// </summary>
        private static string[] files;

        /// <summary>
        /// Constructor of ArkSpanCodeGen
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            var path = Directory.GetCurrentDirectory();
            var modFolder = path.Split('\\').Last();
            PATH_PREFIX += modFolder;

            WriteColor(@"[ $$$$$$\            $$\       $$\      $$\                 $$\ ]", ConsoleColor.Yellow);
            WriteColor(@"[$$  __$$\           $$ |      $$$\    $$$ |                $$ |]", ConsoleColor.Yellow);
            WriteColor(@"[$$ /  $$ | $$$$$$\  $$ |  $$\ $$$$\  $$$$ | $$$$$$\   $$$$$$$ |]", ConsoleColor.Yellow);
            WriteColor(@"[$$$$$$$$ |$$  __$$\ $$ | $$  |$$\$$\$$ $$ |$$  __$$\ $$  __$$ |]", ConsoleColor.Yellow);
            WriteColor(@"[$$  __$$ |$$ |  \__|$$$$$$  / $$ \$$$  $$ |$$ /  $$ |$$ /  $$ |]", ConsoleColor.Yellow);
            WriteColor(@"[$$ |  $$ |$$ |      $$  _$$<  $$ |\$  /$$ |$$ |  $$ |$$ |  $$ |]", ConsoleColor.Yellow);
            WriteColor(@"[$$ |  $$ |$$ |      $$ | \$$\ $$ | \_/ $$ |\$$$$$$  |\$$$$$$$ |]", ConsoleColor.Yellow);
            WriteColor(@"[\__|  \__|\__|      \__|  \__|\__|     \__| \______/  \_______|]", ConsoleColor.Yellow);

            Console.WriteLine(Environment.NewLine);
            WriteColor(@"[//--Informationen------------------------------------------------]", ConsoleColor.DarkGreen);
            WriteColor($"[// Title:] ARK Spawncode Generator", ConsoleColor.DarkGreen);
            WriteColor($"[// Autor:] derda, L. Gmann", ConsoleColor.DarkGreen);
            WriteColor(@"[//--Settings-----------------------------------------------------]", ConsoleColor.DarkGreen);
            WriteColor($"[// Output folder:] {OUTPUT_PATH}", ConsoleColor.DarkGreen);
            WriteColor($"[// Mod folder name:] {modFolder} (Absolute path: {PATH_PREFIX})", ConsoleColor.DarkGreen);
            WriteColor(@"[//---------------------------------------------------------------]", ConsoleColor.DarkGreen);
            WriteColor($"[// Blueprints:] To read blueprints they must begin with BP_", ConsoleColor.DarkGreen);
            WriteColor($"[// Engrams:] To read engrams they must begin with EngramEntry", ConsoleColor.DarkGreen);
            WriteColor($"[// Primal Item:] To read primal and spawncode items they must begin with PrimalItem", ConsoleColor.DarkGreen);
            WriteColor($"[// Spawncode Dino:] To read creatures and tamed creatures they must begin with Character_BP", ConsoleColor.DarkGreen);
            WriteColor(@"[//---------------------------------------------------------------]", ConsoleColor.DarkGreen);
            Console.WriteLine(Environment.NewLine);
            WriteColor($"[Push a random key to start the process]", ConsoleColor.DarkRed);
            Console.WriteLine(Environment.NewLine);

            Console.ReadKey();

            files = Directory.GetFiles(Directory.GetCurrentDirectory());

            if (IsArkModFolder())
            {
                ParseFiles();
                WriteFiles();

                Console.WriteLine(Environment.NewLine);
                WriteColor(@"[//--Informationen------------------------------------------------]", ConsoleColor.DarkGreen);
                WriteColor($"[// Read files:] {summeryFile.Count} Total, {engramFile.Count} Engrams, {blueprintFile.Count} Blueprints, {primalItemsFile.Count} Primal Items, {spawncodeItemsFile.Count} Spawncode Items, {spawncodeCreatureFile.Count} Spawncode Creatures", ConsoleColor.DarkGreen);
                WriteColor($"[//] Looks like all worked fine ;)", ConsoleColor.DarkGreen);
                WriteColor(@"[//---------------------------------------------------------------]", ConsoleColor.DarkGreen);
                Console.WriteLine(Environment.NewLine);
            }
            else
            {
                WriteColor(@"[//--Error--------------------------------------------------------]", ConsoleColor.DarkRed);
                WriteColor($"[// Error:] This folder dont have a PrimalGameData.", ConsoleColor.DarkRed);
                WriteColor(@"[//---------------------------------------------------------------]", ConsoleColor.DarkRed);
            }
            Console.ReadKey();
        }

        /// <summary>
        /// Parse all files and save them into list of strings
        /// </summary>
        static void ParseFiles()
        {
            Directory.CreateDirectory(OUTPUT_PATH);

            // Delete files if exist
            File.Delete(OUTPUT_PATH + SUMMERY_FILE + ".txt");
            File.Delete(OUTPUT_PATH + BLUEPRINT_FILE + ".txt");
            File.Delete(OUTPUT_PATH + ENGRAM_FILE + ".txt");
            File.Delete(OUTPUT_PATH + PRIMAL_ITEMS_FILE + ".txt");
            File.Delete(OUTPUT_PATH + SPAWNCODE_ITEMS_FILE + ".txt");
            File.Delete(OUTPUT_PATH + SPAWNCODE_CREATURE_FILE + ".txt");
            
            var path = Directory.GetCurrentDirectory();
            var allItems = Directory.GetFiles(Directory.GetCurrentDirectory(), "*", SearchOption.AllDirectories);

            foreach (var item in allItems)
            {
                var filename = Path.GetFileNameWithoutExtension(item);
                if (filename.StartsWith("EngramEntry"))
                {
                    var s = filename + "_C";
                    summeryFile.Add(new Summery
                    {
                        Type = SummeryEnum.ENGRAM,
                        Value = s
                    });
                    engramFile.Add(s);
                }
                if (filename.StartsWith("BP_"))
                {
                    var s = ((char)34) + "Blueprint'" + item.Replace(path, PATH_PREFIX).Replace(".uasset", "." + filename).Replace(@"\", "/") + "'" + ((char)34);
                    summeryFile.Add(new Summery
                    {
                        Type = SummeryEnum.BLUEPRINT,
                        Value = s
                    });
                    blueprintFile.Add(s);
                }
                if (filename.StartsWith("PrimalItem"))
                {
                    var s = "admincheat GiveItem " + ((char)34) + "Blueprint'" + item.Replace(path, PATH_PREFIX).Replace(".uasset", "." + filename).Replace(@"\", "/") + "'" + ((char)34) + " 1 1 0";
                    var ss = ((char)34) + "Blueprint'" + item.Replace(path, PATH_PREFIX).Replace(".uasset", "." + filename).Replace(@"\", "/") + "'" + ((char)34);
                    summeryFile.Add(new Summery
                    {
                        Type = SummeryEnum.SPAWNCODE_ITEM,
                        Value = s
                    });
                    summeryFile.Add(new Summery
                    {
                        Type = SummeryEnum.PRIMAL_ITEM,
                        Value = s
                    });
                    primalItemsFile.Add(ss);
                    spawncodeItemsFile.Add(s);
                }
                if (filename.StartsWith("Character_BP"))
                {
                    var s = "admincheat SpawnDino " + ((char)34) + "Blueprint'" + item.Replace(path, PATH_PREFIX).Replace(".uasset", "." + filename).Replace(@"\", "/") + "'" + ((char)34) + " 500 0 0 120";
                    summeryFile.Add(new Summery
                    {
                        Type = SummeryEnum.SPAWNCODE_CREATURE,
                        Value = s
                    });
                    spawncodeCreatureFile.Add(s);
                }
                if (filename.StartsWith("Character_BP"))
                {
                    var s = "admincheat GMSummon " + ((char)34) + filename + ((char)34) + " 120";
                    summeryFile.Add(new Summery
                    {
                        Type = SummeryEnum.SPAWNCODE_CREATURE,
                        Value = s
                    });
                    spawncodeCreatureFile.Add(s);
                }
            }
        }

        /// <summary>
        /// Write the content to the files 
        /// </summary>
        static void WriteFiles()
        {
            WriteColor(@"[//--Summery------------------------------------------------------]", ConsoleColor.Cyan);
            var type = "";
            foreach (var item in summeryFile.OrderBy(el => el.Type))
            {
                if(type.Equals("") || !type.Equals(item.Type.ToString()))
                {
                    if (!type.Equals(""))
                    {
                        File.AppendAllText(OUTPUT_PATH + SUMMERY_FILE + ".txt", $"//-------------------------------------------------------------------" + Environment.NewLine + Environment.NewLine);
                    }
                    type = item.Type.ToString();
                    File.AppendAllText(OUTPUT_PATH + SUMMERY_FILE + ".txt", $"//--{item.Type}------------------------------------------------------" + Environment.NewLine);
                }
                File.AppendAllText(OUTPUT_PATH + SUMMERY_FILE + ".txt", item.Value + Environment.NewLine);
                WriteColor($"[// {item.Type}] {item.Value}", ConsoleColor.Cyan);
            }
            WriteColor(@"[//---------------------------------------------------------------]", ConsoleColor.Cyan);
            Console.WriteLine(Environment.NewLine);

            WriteColor(@"[//--Blueprints---------------------------------------------------]", ConsoleColor.Cyan);
            foreach (var item in blueprintFile)
            {
                File.AppendAllText(OUTPUT_PATH + BLUEPRINT_FILE + ".txt", item + Environment.NewLine);
                WriteColor($"[//] {item}", ConsoleColor.Cyan);
            }
            WriteColor(@"[//---------------------------------------------------------------]", ConsoleColor.Cyan);
            Console.WriteLine(Environment.NewLine);

            WriteColor(@"[//--Engrams------------------------------------------------------]", ConsoleColor.Cyan);
            foreach (var item in engramFile)
            {
                File.AppendAllText(OUTPUT_PATH + ENGRAM_FILE + ".txt", item + Environment.NewLine);
                WriteColor($"[//] {item}", ConsoleColor.Cyan);
            }
            WriteColor(@"[//---------------------------------------------------------------]", ConsoleColor.Cyan);
            Console.WriteLine(Environment.NewLine);

            WriteColor(@"[//--SpawncodeItems-----------------------------------------------]", ConsoleColor.Cyan);
            foreach (var item in spawncodeItemsFile)
            {
                File.AppendAllText(OUTPUT_PATH + SPAWNCODE_ITEMS_FILE + ".txt", item + Environment.NewLine);
                WriteColor($"[//] {item}", ConsoleColor.Cyan);
            }
            WriteColor(@"[//---------------------------------------------------------------]", ConsoleColor.Cyan);
            Console.WriteLine(Environment.NewLine);

            WriteColor(@"[//--PrimalItems--------------------------------------------------]", ConsoleColor.Cyan);
            foreach (var item in primalItemsFile)
            {
                File.AppendAllText(OUTPUT_PATH + PRIMAL_ITEMS_FILE + ".txt", item + Environment.NewLine);
                WriteColor($"[//] {item}", ConsoleColor.Cyan);
            }
            WriteColor(@"[//---------------------------------------------------------------]", ConsoleColor.Cyan);
            Console.WriteLine(Environment.NewLine);

            WriteColor(@"[//--CreatureSpawncodes-------------------------------------------]", ConsoleColor.Cyan);
            foreach (var item in spawncodeCreatureFile)
            {
                File.AppendAllText(OUTPUT_PATH + SPAWNCODE_CREATURE_FILE + ".txt", item + Environment.NewLine);
                WriteColor($"[//] {item}", ConsoleColor.Cyan);
            }
            WriteColor(@"[//---------------------------------------------------------------]", ConsoleColor.Cyan);
            Console.WriteLine(Environment.NewLine);
        }

        /// <summary>
        /// Write some coloring console messages for the user
        /// https://stackoverflow.com/questions/2743260/is-it-possible-to-write-to-the-console-in-colour-in-net
        /// </summary>
        /// <param name="message">Message to write</param>
        /// <param name="color">ConsoleColor value of the color</param>
        static void WriteColor(string message, ConsoleColor color)
        {
            var pieces = Regex.Split(message, @"(\[[^\]]*\])");

            for (int i = 0; i < pieces.Length; i++)
            {
                string piece = pieces[i];

                if (piece.StartsWith("[") && piece.EndsWith("]"))
                {
                    Console.ForegroundColor = color;
                    piece = piece.Substring(1, piece.Length - 2);
                }

                Console.Write(piece);
                Console.ResetColor();
            }

            Console.WriteLine();
        }

        /// <summary>
        /// Check if the folder has a file called PrimalGameData
        /// </summary>
        /// <returns>true if file exists otherwise false</returns>
        private static bool IsArkModFolder()
        {
            foreach (string s in files)
            {
                string filename = Path.GetFileNameWithoutExtension(s);
                if (filename.StartsWith("PrimalGameData"))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
