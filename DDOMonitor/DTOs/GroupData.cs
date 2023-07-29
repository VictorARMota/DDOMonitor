using System;

namespace DDOMonitor.DTOs
{
    class GroupData
    {
        public string Name { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public int? GroupCount { get; set; }
        public Group[] Groups { get; set; }
    }
}
