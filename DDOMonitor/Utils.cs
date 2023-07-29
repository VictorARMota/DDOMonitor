using DDOMonitor.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DDOMonitor
{
    static class Utils
    {
        /// <summary>
        /// Method <c>SavePlayer</c> will save a <see cref="Player"/> to a text file for future usage.
        /// </summary>
        /// <param name="player">The Player instance to be saved</param>
        /// <param name="server">The player's server</param>
        public static void SavePlayer(Player player, string server)
        {
            Dictionary<string, List<Player>> loadedPlayersByServer = LoadSavedPlayers();
            List<Player> loadedPlayers = new List<Player>();
            // Fetches all saved players by server
            if (loadedPlayersByServer.TryGetValue(server, out loadedPlayers))
            {
                // Replaces the player info
                List<Player> updatedList = loadedPlayers.FindAll(p => !p.Equals(player));
                updatedList.Add(player);
                loadedPlayersByServer[server] = updatedList;

                File.WriteAllText(@"player.txt", string.Empty);
                File.WriteAllText(@"player.txt", JsonConvert.SerializeObject(loadedPlayersByServer));
            }
        }

        /// <summary>
        /// Method <c>LoadSavedPlayer</c> will load a <see cref="Player"/> from the text file.
        /// </summary>
        /// <param name="playerName">The player's name/param>
        /// <param name="server">The player's server</param>
        /// <returns>
        /// The <see cref="Player"/> instance found or null.
        /// </returns>
        public static Player LoadSavedPlayer(string playerName, string server)
        {
            List<Player> loadedPlayers = new List<Player>();
            // Fetches all saved players by server
            if (LoadSavedPlayers().TryGetValue(server, out loadedPlayers))
                return loadedPlayers.Find(p => p.Name.Equals(playerName));

            return null;
        }

        /// <summary>
        /// Method <c>LoadSavedPlayers</c> will load a Dictionaty with the saved Players by Server from the text file.
        /// </summary>
        /// <returns>
        /// A <see cref="Dictionary{string, List{Player}}"/> with a list of Players by server.
        /// </returns>
        public static Dictionary<string, List<Player>> LoadSavedPlayers()
        {
            if (!File.Exists("player.txt"))
                return null;

            return JsonConvert.DeserializeObject<Dictionary<string, List<Player>>>(File.ReadAllText("player.txt")) ??
                new Dictionary<string, List<Player>>();
        }

        /// <summary>
        /// Method <c>SaveConfig</c> will save a configuration Dictionary to a text file.
        /// </summary>
        /// <param name="configs">The configurations set by the user</param>
        public static void SaveConfig(Dictionary<string, bool> configs)
        {
            File.WriteAllText(@"config.txt", string.Empty);
            using (StreamWriter writetext = File.AppendText("config.txt"))
            {
                foreach (var entry in configs)
                    writetext.WriteLine("{0}={1}", entry.Key, entry.Value);
            }
        }

        /// <summary>
        /// Method <c>LoadConfig</c> will fetch the configuration Dictionary from the text file.
        /// </summary>
        /// <returns>
        /// A <see cref="Dictionary{string, bool}"/> object representing all saved configurations.
        /// </returns>
        public static Dictionary<string, bool> LoadConfig()
        {
            Dictionary<string, bool> loadedConfig = new Dictionary<string, bool>();
            if (!File.Exists("config.txt"))
                return loadedConfig;

            using (StreamReader readtext = new StreamReader("config.txt"))
            {

                string line;
                while ((line = readtext.ReadLine()) != null)
                {
                    if (!String.IsNullOrEmpty(line) && line.Contains("="))
                    {
                        string[] split = line.Split('=');
                        loadedConfig.Add(split[0], split.Length > 1 && Boolean.Parse(split[1]));
                    }
                }

                return loadedConfig;
            }
        }

        /// <summary>
        /// Method <c>GetProperty</c> will fetch a property from the Configuration's text file.
        /// </summary>
        /// <param name="propertyName">The property to fetch</param>
        /// <returns>
        /// The property value.
        /// </returns>
        public static bool GetProperty(string propertyName)
        {
            Dictionary<string, bool> loadedConfig = LoadConfig();
            if (loadedConfig.TryGetValue(propertyName, out bool value))
                return value;
            else
                return false;
        }

        /// <summary>
        /// Method <c>ParseGroupsToText</c> will format all given groups to a readable format.
        /// </summary>
        /// <param name="groups">The group list to parse</param>
        /// <returns>
        /// A <see cref="string"/> with the formated groups.
        /// </returns>
        public static string ParseGroupsToText(List<Group> groups)
        {
            StringBuilder builder = new StringBuilder();
            foreach (Group group in groups)
            {
                builder.AppendFormat("- {0} | {1} : Lvl {2} to {3}.", group.Comment, (group.Quest == null ? "" : group.Quest.Name), group.MinimumLevel, group.MaximumLevel);
                builder.AppendLine();
            }

            return builder.ToString();
        }
    }
}
