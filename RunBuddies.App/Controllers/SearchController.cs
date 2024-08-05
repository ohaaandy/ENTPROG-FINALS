using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RunBuddies.App.Models;
using RunBuddies.DataModel;
using System.Security.Claims;

namespace RunBuddies.App.Controllers
{

    public class SearchController : Controller
    {
        private readonly AppDBContext _context;

        public SearchController(AppDBContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Filter(string searchType)
        {
            ViewBag.SearchType = searchType ?? "RunningBuddy";
            return View();
        }

        public IActionResult RunningBuddy()
        {
            ViewBag.SearchType = "RunningBuddy";
            return View("Filter");
        }

        public IActionResult RunningClub()
        {
            ViewBag.SearchType = "RunningClub";
            return View("Filter");
        }

        public IActionResult Search(string searchType, string level, string location, string[] days, int? distance)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var daysList = days?.Select(ConvertToDayOfWeek)
                                .Where(d => d.HasValue)
                                .Select(d => d.Value)
                                .ToList() ?? new List<DayOfWeek>();
            var results = new List<SearchResultViewModel>();

            try
            {
                if (searchType == "RunningBuddy")
                {
                    var query = _context.Users.AsQueryable();

                    // Exclude the current user
                    query = query.Where(u => u.Id != currentUserId);

                    if (!string.IsNullOrEmpty(level))
                        query = query.Where(u => u.RunnerLevel == level);

                    if (!string.IsNullOrEmpty(location))
                        query = query.Where(u => u.Location == location);

                    if (daysList.Any())
                        query = query.Where(u => u.Schedule.HasValue && daysList.Contains(u.Schedule.Value.DayOfWeek));

                    if (distance.HasValue)
                        query = query.Where(u => u.Distance == distance.Value);

                    results = query.Select(u => new SearchResultViewModel
                    {
                        Id = u.Id,
                        Name = u.FirstName + " " + u.LastName,
                        Level = u.RunnerLevel ?? "Not specified",
                        Location = u.Location ?? "Not specified",
                        Schedule = u.Schedule.HasValue ? u.Schedule.Value.DayOfWeek.ToString() : "Not specified",
                        Distance = u.Distance ?? 0,
                        Type = "Buddy"
                    }).ToList();

                    // Logging (consider using a proper logging framework in production)
                    foreach (var user in results)
                    {
                        Console.WriteLine($"User: {user.Name}, Distance: {user.Distance}");
                    }
                }
                else if (searchType == "RunningClub")
                {
                    var query = _context.Clubs.AsQueryable();

                    if (!string.IsNullOrEmpty(location))
                        query = query.Where(c => c.Location == location);

                    results = query.Select(c => new SearchResultViewModel
                    {
                        ClubId = c.ClubID,
                        Name = c.ClubName,
                        Level = "N/A",
                        Location = c.Location ?? "Not specified",
                        Schedule = "N/A",
                        Distance = distance ?? 0,
                        Type = "Club"
                    }).ToList();
                }
                else
                {
                    return BadRequest("Invalid search type");
                }

                return Json(results);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"An error occurred during search: {ex.Message}");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet]
        public IActionResult GetDetails(int id, string uid, string type)
        {
            if (type == "Buddy")
            {
                var user = _context.Users.FirstOrDefault(u => u.Id == uid);
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



        [Authorize]
        public IActionResult Results(string searchType, string level, string location, string days)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Console.WriteLine($"Current User ID: {currentUserId}"); // Debug line

            var daysList = days?.Split(',')
                .Select(ConvertToDayOfWeek)
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

                // Fetch the data from the database
                var users = query.ToList();

                // Perform the day filtering in memory
                results = users
                    .Where(u => !daysList.Any() || (u.Schedule.HasValue && daysList.Contains(u.Schedule.Value.DayOfWeek)))
                    .Select(u => new SearchResultViewModel
                    {
                        Id = u.Id,
                        Name = $"{u.FirstName} {u.LastName}",
                        Level = u.RunnerLevel ?? "Not specified",
                        Location = u.Location ?? "Not specified",
                        Schedule = u.Schedule.HasValue ? u.Schedule.Value.DayOfWeek.ToString() : "Not specified",
                        Distance = u.Distance ?? 0,
                        Type = "Buddy"
                    })
                    .Where(r => r.Id != currentUserId)
                    .ToList();

                Console.WriteLine($"Results count: {results.Count}"); // Debug line
                foreach (var result in results)
                {
                    Console.WriteLine($"User ID: {result.Id}, Name: {result.Name}"); // Debug line
                }
            }
            else if (searchType == "RunningClub")
            {
                var query = _context.Clubs.AsQueryable();

                if (!string.IsNullOrEmpty(location))
                    query = query.Where(c => c.Location == location);

                results = query.Select(c => new SearchResultViewModel
                {
                    ClubId = c.ClubID,
                    Name = c.ClubName,
                    Level = "N/A",
                    Location = c.Location ?? "Not specified",
                    Schedule = "N/A",
                    Distance = 0, // You might want to add a Distance property to Club if needed
                    Type = "Club"
                }).ToList();
            }

            return View(results);
        }

        private DayOfWeek? ConvertToDayOfWeek(string day)
        {
            switch (day.ToLower())
            {
                case "mon": return DayOfWeek.Monday;
                case "tue": return DayOfWeek.Tuesday;
                case "wed": return DayOfWeek.Wednesday;
                case "thu": return DayOfWeek.Thursday;
                case "fri": return DayOfWeek.Friday;
                case "sat": return DayOfWeek.Saturday;
                case "sun": return DayOfWeek.Sunday;
                default: return null;
            }
        }

        public IActionResult TestQuery()
        {
            var users = _context.Users.ToList();
            return Json(users);
        }
    }
}