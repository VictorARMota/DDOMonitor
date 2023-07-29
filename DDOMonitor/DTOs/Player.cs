namespace DDOMonitor.DTOs
{
    class Player
    {
        public string Name { get; set; }
        public string Gender { get; set; }
        public string Race { get; set; }
        public int? TotalLevel { get; set; }
        public Class[] Classes { get; set; }
        public Location Location { get; set; }
        public long? GroupId { get; set; }
        public string Guild { get; set; }
        public bool InParty { get; set; }
        public string HomeServer { get; set; }

        public override bool Equals(object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Player p = (Player)obj;
                return (Name.Equals(p.Name));
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
