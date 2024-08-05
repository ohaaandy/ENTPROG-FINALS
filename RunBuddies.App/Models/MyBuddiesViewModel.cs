namespace RunBuddies.App.Models
{
    public class MyBuddiesViewModel
    {
        public List<BuddyDetailViewModel> Buddies { get; set; }
    }

    public class BuddyDetailViewModel
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string RunnerLevel { get; set; }
        public string Location { get; set; }
        public string PreferredSchedule { get; set; }
        public int PreferredDistance { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public List<BuddySessionViewModel> RecentSessions { get; set; }
    }
}