namespace RunBuddies.App.Models
{
    public class SearchResultViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Level { get; set; }
        public string Location { get; set; }
        public string Schedule { get; set; }
        public int Distance { get; set; }
        public string Type { get; set; } // "Buddy" or "Club"
    }
}
