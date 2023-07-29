using DDOMonitor.DTOs;
using DDOMonitor.Monitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace DDOMonitor
{
    /// <summary>
    /// Class <c>GroupMonitor</c> will monitor for all LFMs available, filtering the range of available Quests by Character Level.
    /// </summary>
    class GroupMonitor
    {
        private readonly string server;
        private readonly ServerMonitor serverMonitor;
        private readonly PlayerMonitor playerMonitor;
        private readonly ToastOperator toastOperator;

        private static Timer groupMonitorTask;
        private static readonly List<Group> bufferedGroups = new List<Group>();

        /// <summary>
        /// The <c>GroupMonitor</c>'s constructor method.
        /// </summary>
        /// <param name="server">the selected server's name.</param>
        /// <param name="serverMonitor">the ServerMonitor instance.</param>
        /// <param name="playerMonitor">the PlayerMonitor instance.</param>
        /// <param name="toastOperator">the ToastOperator instance.</param>
        public GroupMonitor(string server, ServerMonitor serverMonitor, PlayerMonitor playerMonitor, ToastOperator toastOperator)
        {
            this.server = server;
            this.serverMonitor = serverMonitor;
            this.playerMonitor = playerMonitor;
            this.toastOperator = toastOperator;

            try
            {
                // Each monitor task is offset by 15 seconds, as requested by DDOAudit, to avoid overloading.
                groupMonitorTask = new Timer(MonitorGroups, null, TimeSpan.FromSeconds(30), TimeSpan.FromMinutes(1));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method <c>Stop</c> will stop the monitor for polling LFMs.
        /// </summary>
        public void Stop()
        {
            groupMonitorTask.Dispose();
        }

        /// <summary>
        /// Method <c>MonitorGroups</c> will be called in a <see cref="Timer"/> callback.
        /// This will call the <see cref="ServerMonitor"/> to check if the server is available, as well as the <see cref="PlayerMonitor"/>
        /// to fetch the specified <see cref="Player"/>.
        /// </summary>
        /// <param name="state">Ignored param</param>
        private void MonitorGroups(object state)
        {
            if (serverMonitor.IsServerOffline())
                return;

            Player player = playerMonitor.GetPlayer();
            if (player == null)
                return;

            GroupData groupData = DDOAuditHttpClient.GetGroupData(server);

            // Filtering Polled Groups by Player Level
            List<Group> availableGroups = groupData.Groups
                .Where(x => x.MaximumLevel.HasValue && x.MinimumLevel.HasValue)
                .Where(g => g.MinimumLevel.Value <= player.TotalLevel.Value && player.TotalLevel.Value <= g.MaximumLevel.Value)
                .ToList();


            if (!Utils.GetProperty("onlyOnNewQuestsCheck"))
                toastOperator.PushQuestInfoToastNotification(Utils.ParseGroupsToText(availableGroups), Utils.ParseGroupsToText(availableGroups));
            else
            {
                // Only notifies on changed list of available LFMs
                List<Group> newGroups = availableGroups.Where(g => !bufferedGroups.Any(p => g.Equals(p))).ToList();
                if (newGroups.Count > 0)
                {
                    toastOperator.PushQuestInfoToastNotification(Utils.ParseGroupsToText(newGroups), Utils.ParseGroupsToText(availableGroups));

                    bufferedGroups.Clear();
                    bufferedGroups.AddRange(availableGroups);
                }
            }
        }
    }
}
