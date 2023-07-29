using DDOMonitor.DTOs;
using System;
using System.Linq;
using System.Threading;

namespace DDOMonitor
{
    /// <summary>
    /// Class <c>ServerMonitor</c> will monitor the specified Server, polling its status to see if it is online or offline.
    /// </summary>
    class ServerMonitor
    {
        private readonly string server;
        private static Timer serverMonitorTask;
        private static volatile bool offlineServerFlag;
        private readonly ToastOperator toastOperator;

        /// <summary>
        /// The <c>ServerMonitor</c>'s constructor method.
        /// </summary>
        /// <param name="server">the selected server's name.</param>
        /// <param name="toastOperator">the ToastOperator instance.</param>
        public ServerMonitor(string server, ToastOperator toastOperator)
        {
            this.server = server;
            this.toastOperator = toastOperator;

            try
            {
                // Each monitor task is offset by 15 seconds, as requested by DDOAudit, to avoid overloading.
                serverMonitorTask = new Timer(MonitorServerStatus, null, TimeSpan.FromSeconds(0), TimeSpan.FromMinutes(1));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method <c>Stop</c> will stop the monitor for polling the Server status.
        /// </summary>
        public void Stop()
        {
            serverMonitorTask.Dispose();
        }

        /// <summary>
        /// Method <c>MonitorServerStatus</c> will be called in a <see cref="Timer"/> callback.
        /// This will poll the <see cref="WorldStatus"/> to check if the server is available.
        /// </summary>
        /// <param name="state">Ignored param</param>
        private void MonitorServerStatus(object state)
        {
            WorldStatus worldStatus = DDOAuditHttpClient.GetServerStatus().Worlds
                .Where(w => w.Name.Equals(server, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault();

            if (worldStatus.Status == 0 && !offlineServerFlag)
            {
                // Notifies when server is Offline
                offlineServerFlag = true;
                toastOperator.PushServerOfflineToastNotification();
            }

            // Only notifies if the previous state of the Server was Offline
            if (worldStatus.Status == 1 && offlineServerFlag)
                toastOperator.PushServerOnlineToastNotification(server);
        }

        /// <summary>
        /// Method <c>IsServerOffline</c> will return the buffered server state. This was created to avoid demanding too much of DDOAudit's server.
        /// </summary>
        /// <returns>
        /// A boolean representing the server status,
        /// being <c>true</c> an offline server.
        /// </returns>
        public bool IsServerOffline()
        {
            return offlineServerFlag;
        }
    }
}
