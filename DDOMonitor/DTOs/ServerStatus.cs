using System;

namespace DDOMonitor.DTOs
{
    class ServerStatus
    {
        public DateTime LastUpdateTime { get; set; }
        public WorldStatus[] Worlds { get; set; }
    }

    class WorldStatus
    {
        public string Name { get; set; }
        public string StatusServerUrl { get; set; }
        public int? Order { get; set; }
        public int? Status { get; set; }
    }
}
