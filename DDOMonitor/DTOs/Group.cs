namespace DDOMonitor.DTOs
{
    class Group
    {
        public long? Id { get; set; }
        public string Comment { get; set; }
        public Quest Quest { get; set; }
        public int? MinimumLevel { get; set; }
        public int? MaximumLevel { get; set; }
        public int? AdventureActive { get; set; }
        public Player Leader { get; set; }
        public Player[] Members { get; set; }

        public override bool Equals(object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Group g = (Group)obj;
                if (!Leader.Equals(g.Leader))
                    return false;

                if (string.IsNullOrEmpty(Comment) && !string.IsNullOrEmpty(g.Comment))
                    return false;

                if (!Comment.Equals(g.Comment))
                    return false;

                if (Quest == null)
                    return g.Quest == null;

                return Quest.Equals(g.Quest);
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
