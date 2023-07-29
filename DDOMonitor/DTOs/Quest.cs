namespace DDOMonitor.DTOs
{
    class Quest
    {
        public int? AreaId { get; set; }
        public string Name { get; set; }
        public int? HeroicNormalCR { get; set; }
        public int? EpicNormalCR { get; set; }
        public bool IsFreeToVip { get; set; }
        public string RequiredAdventurePack { get; set; }
        public string AdventureArea { get; set; }
        public string QuestJournalGroup { get; set; }
        public string Patron { get; set; }

        public override bool Equals(object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Quest q = (Quest)obj;
                return (Name.Equals(q.Name));
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
