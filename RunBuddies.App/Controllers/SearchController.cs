using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RunBuddies.App.Models;
using RunBuddies.DataModel;

namespace RunBuddies.App.Controllers
{
    public class SearchController : Controller
    {
        private readonly AppDBContext _context;

        public SearchController(AppDBContext context)
        {
            _context = context;
        }

        public IActionResult Filter()
        {
            // Removed authentication check muna
            return View();
        }

        public IActionResult RunningBuddy()
        {
            return View();
        }

        public IActionResult RunningClub()
        {
            return View();
        }

       [HttpPost]
        public IActionResult Search(string searchType, string level, string location, string[] days)
        {
            var daysList = days?.Select(d => ConvertToDayOfWeek(d.Trim()))
                                .Where(d => d.HasValue)
                                .Select(d => d.Value)
                                .ToList() ?? new List<DayOfWeek>();

            var results = new List<SearchResultViewModel>();

            if (searchType == "RunningBuddy")
            {
                var query = _context.Users.AsQueryable();

                if (!string.IsNullOrEmpty(level))
                    query = query.Where(u => u.RunnerLevel == level);

                if (!string.IsNullOrEmpty(location))
                    query = query.Where(u => u.Location == location);

                if (daysList.Any())
                    query = query.Where(u => daysList.Contains(u.Schedule.DayOfWeek)); // Assuming Schedule is DateTime

                results = query.Select(u => new SearchResultViewModel
                {
                    Id = u.UserID,
                    Name = u.FirstName + " " + u.LastName,
                    Level = u.RunnerLevel,
                    Location = u.Location,
                    Schedule = u.Schedule.DayOfWeek.ToString(),
                    Type = "Buddy"
                }).ToList();
            }
            else if (searchType == "RunningClub")
            {
                var query = _context.Clubs.AsQueryable();

                if (!string.IsNullOrEmpty(location))
                    query = query.Where(c => c.Location == location);

                results = query.Select(c => new SearchResultViewModel
                {
                    Id = c.ClubID,
                    Name = c.ClubName,
                    Level = "N/A",
                    Location = c.Location,
                    Schedule = "N/A", // You might want to add a Schedule property to the Club model
                    Type = "Club"
                }).ToList();
            }

            return Json(results);
        }

        [HttpGet]
        public IActionResult GetDetails(int id, string type)
        {
            if (type == "Buddy")
            {
                var user = _context.Users.FirstOrDefault(u => u.UserID == id);
                return PartialView("_UserDetails", user);
            }
            else if (type == "Club")
            {
                var club = _context.Clubs.FirstOrDefault(c => c.ClubID == id);
                return PartialView("_ClubDetails", club);
            }

            return NotFound();
        }

        [HttpPost]
        public IActionResult InviteBuddy(int id)
        {
            // Implement invite buddy logic
            return Json(new { message = "Invitation sent successfully" });
        }

        [HttpPost]
        public IActionResult JoinClub(int id)
        {
            // Implement join club logic
            return Json(new { message = "Club joined successfully" });
        }

        public IActionResult Results(string searchType, string level, string location, string days)
        {
            var daysList = days?.Split(',')
                .Select(d => ConvertToDayOfWeek(d.Trim()))
                .Where(d => d.HasValue)
                .Select(d => d.Value)
                .ToList() ?? new List<DayOfWeek>();

            var results = new List<SearchResultViewModel>();

            if (searchType == "RunningBuddy")
            {
                var query = _context.Users.AsEnumerable();  // Bring data to memory

                if (!string.IsNullOrEmpty(level))
                    query = query.Where(u => u.RunnerLevel == level);

                if (!string.IsNullOrEmpty(location))
                    query = query.Where(u => u.Location == location);

                if (daysList.Any())
                    query = query.Where(u => daysList.Contains(u.Schedule.DayOfWeek));

                results = query.Select(u => new SearchResultViewModel
                {
                    Id = u.UserID,
                    Name = u.FirstName + " " + u.LastName,
                    Level = u.RunnerLevel,
                    Location = u.Location,
                    Schedule = u.Schedule.DayOfWeek.ToString(),
                    Type = "Buddy"
                }).ToList();
            }
            else if (searchType == "RunningClub")
            {
                results = _context.Clubs
                    .Where(c => string.IsNullOrEmpty(location) || c.Location == location)
                    .AsEnumerable()  // Bring data to memory
                    .Select(c => new SearchResultViewModel
                    {
                        Id = c.ClubID,
                        Name = c.ClubName,
                        Level = "N/A",
                        Location = c.Location,
                        Schedule = "N/A", // You might want to add a Schedule property to the Club model
                        Type = "Club"
                    })
                    .ToList();
            }

            return View(results);
        }

        private DayOfWeek? ConvertToDayOfWeek(string day)
        {
            switch (day.ToLower())
            {
                case "mon":
                    return DayOfWeek.Monday;
                case "tue":
                    return DayOfWeek.Tuesday;
                case "wed":
                    return DayOfWeek.Wednesday;
                case "thu":
                    return DayOfWeek.Thursday;
                case "fri":
                    return DayOfWeek.Friday;
                case "sat":
                    return DayOfWeek.Saturday;
                case "sun":
                    return DayOfWeek.Sunday;
                default:
                    return null;
            }
        }
    }
}