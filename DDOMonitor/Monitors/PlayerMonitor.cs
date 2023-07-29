using DDOMonitor.DTOs;
using System;
using System.Threading;

namespace DDOMonitor.Monitors
{
    /// <summary>
    /// Class <c>PlayerMonitor</c> will monitor the specified Player/Character, polling its info every minute to check Level changes.
    /// </summary>
    class PlayerMonitor
    {
        private readonly string server;
        private readonly string characterName;
        private readonly ServerMonitor serverMonitor;

        private static Timer playerMonitorTask;

        private volatile Player player;

        /// <summary>
        /// The <c>PlayerMonitor</c>'s constructor method.
        /// </summary>
        /// <param name="server">the selected server's name.</param>
        /// <param name="characterName">the selected character's name.</param>
        /// <param name="serverMonitor">the ServerMonitor instance.</param>
        public PlayerMonitor(string server, string characterName, ServerMonitor serverMonitor)
        {
            this.server = server;
            this.characterName = characterName;
            this.serverMonitor = serverMonitor;

            try
            {
                // Each monitor task is offset by 15 seconds, as requested by DDOAudit, to avoid overloading.
                playerMonitorTask = new Timer(MonitorPlayerStatus, null, TimeSpan.FromSeconds(15), TimeSpan.FromMinutes(1));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method <c>Stop</c> will stop the monitor for polling Player data.
        /// </summary>
        public void Stop()
        {
            playerMonitorTask.Dispose();
        }

        /// <summary>
        /// Method <c>MonitorPlayerStatus</c> will be called in a <see cref="Timer"/> callback.
        /// This will call the <see cref="ServerMonitor"/> to check if the server is available
        /// to fetch the specified <see cref="Player"/>.
        /// This method will update a text file with the found players, to keep monitoring LFMs even if the player is offline.
        /// </summary>
        /// <param name="state">Ignored param</param>
        private void MonitorPlayerStatus(object state)
        {
            if (serverMonitor.IsServerOffline())
                return;

            Player player = DDOAuditHttpClient.GetPlayer(server, characterName);
            // If the player is offline, the request will return null
            if (player == null)
            {
                // Tries to find saved Player in player.txt file
                player = Utils.LoadSavedPlayer(characterName, server);
                if (player == null)
                    throw new Exception("Player not found on server " + server);
            }

            if (!player.TotalLevel.HasValue)
                throw new Exception("Cannot find Character's level");

            // Saves the player to a .txt file, to fetch later if the player is offline.
            Utils.SavePlayer(player, server);
            this.player = player;
        }

        /// <summary>
        /// Method <c>GetPlayer</c> will return the buffered player state. This was created to avoid demanding too much of DDOAudit's server.
        /// </summary>
        /// <returns>
        /// The found <c>Player</c> instance or <c>null</c>
        /// </returns>
        public Player GetPlayer()
        {
            return player;
        }
    }
}
