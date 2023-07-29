using DDOMonitor.DTOs;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace DDOMonitor
{
    /// <summary>
    /// Class <c>DDOAuditHttpClient</c> will handle all HTTP requests to the DDOAudit Public API.
    /// </summary>
    static class DDOAuditHttpClient
    {
        private static readonly string BASE_URL = "https://api.ddoaudit.com/";
        private static readonly string SERVER_STATUS_URL = BASE_URL + "gamestatus/serverstatus";
        private static readonly string GROUP_DATA_URL = BASE_URL + "groups/";
        private static readonly string PLAYER_DATA_URL = BASE_URL + "players/";

        private static readonly HttpClient client = new HttpClient();

        /// <summary>
        /// Method <c>GetServerStatus</c> will fetch the <see cref="ServerStatus"/> from the DDOAudit Public API.
        /// </summary>
        /// <returns>
        /// A <see cref="ServerStatus"/> object representing the server status.
        /// </returns>
        public static ServerStatus GetServerStatus()
        {
            return JsonConvert.DeserializeObject<ServerStatus>(GetResponse(SERVER_STATUS_URL));
        }

        /// <summary>
        /// Method <c>GetGroupData</c> will fetch the <see cref="GroupData"/> from the DDOAudit Public API.
        /// </summary>
        /// <param name="server">The chosen server</param>
        /// <returns>
        /// A <see cref="ServerStatus"/> object representing all available groups/LFMs for the server.
        /// </returns>
        public static GroupData GetGroupData(string server)
        {
            return JsonConvert.DeserializeObject<GroupData>(GetResponse(GROUP_DATA_URL + server));
        }

        /// <summary>
        /// Method <c>GetPlayerData</c> will fetch the <see cref="PlayerData"/> from the DDOAudit Public API.
        /// </summary>
        /// <param name="server">The chosen server</param>
        /// <returns>
        /// A <see cref="PlayerData"/> object representing all online players in the server.
        /// </returns>
        public static PlayerData GetPlayerData(string server)
        {
            return JsonConvert.DeserializeObject<PlayerData>(GetResponse(PLAYER_DATA_URL + server));
        }

        /// <summary>
        /// Method <c>GetPlayer</c> will call the <see cref="GetPlayerData"/> and filter the specified Player Name.
        /// </summary>
        /// <param name="server">The chosen server</param>
        /// <param name="playerName">The chosen characterName</param>
        /// <returns>
        /// A <see cref="Player"/> object representing the specified player in the server. This will return <c>null</c>
        /// if no player was found.
        /// </returns>
        public static Player GetPlayer(string server, string playerName)
        {
            List<Player> players = GetPlayerData(server).Players.ToList();
            return players.Where(p => p.Name.Equals(playerName)).FirstOrDefault();
        }

        /// <summary>
        /// Method <c>GetResponse</c> will call the <see cref="HttpClient"/> instance to send an HTTP GET request to the DDO Audit Public API .
        /// </summary>
        /// <param name="url">The url to send the Request</param>
        /// <returns>
        /// A <see cref="string"/> in JSON format with the response or <c>null</c>.
        /// </returns>
        private static string GetResponse(string url)
        {
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.GetAsync(url).Result;
            response.EnsureSuccessStatusCode();
            return response.Content.ReadAsStringAsync().Result ?? "";
        }
    }
}
