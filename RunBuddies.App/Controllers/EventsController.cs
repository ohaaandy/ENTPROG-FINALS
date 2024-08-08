using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunBuddies.App.Models;
using RunBuddies.DataModel;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RunBuddies.Controllers
{
    public class EventsController : Controller
    {
        private readonly AppDBContext _context;
        private readonly UserManager<User> _userManager;

        public EventsController(AppDBContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var events = await _context.Events.ToListAsync();
            return View(events);
        }

        public async Task<IActionResult> Details(int id)
        {
            var @event = await _context.Events
                .Include(e => e.Club)
                .Include(e => e.EventParticipants)
                .FirstOrDefaultAsync(e => e.EventID == id);

            if (@event == null)
            {
                return NotFound();
            }

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var viewModel = new EventDetailsViewModel
            {
                Event = @event,
                CurrentUserId = currentUserId
            };

            return View(viewModel);
        }

        public async Task<IActionResult> EventDetails(int id)
        {
            var @event = await _context.Events
                .Include(e => e.User)
                .Include(e => e.EventParticipants)
                    .ThenInclude(ep => ep.User)
                .Include(e => e.Leaderboard)
                    .ThenInclude(l => l.Entries)
                        .ThenInclude(e => e.User)
                .FirstOrDefaultAsync(e => e.EventID == id);

            if (@event == null)
            {
                return NotFound();
            }

            ViewBag.CurrentUserId = _userManager.GetUserId(User);

            return View(@event);
        }

        //public IActionResult Create()
        //{
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(EventViewModel eventViewModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var newEvent = new Event
        //        {
        //            EventName = eventViewModel.EventName,
        //            DateTime = eventViewModel.DateTime,
        //            Location = eventViewModel.Location,
        //            Description = eventViewModel.Description,
        //            // Set other properties as needed
        //        };

        //        _context.Add(newEvent);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(eventViewModel);
        //}

        [Authorize]
        public async Task<IActionResult> MyEvents()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var userEvents = await _context.Events
                .Where(e => e.UserID == currentUser.Id || e.EventParticipants.Any(ep => ep.UserID == currentUser.Id))
                .Include(e => e.Club)
                .Select(e => new EventViewModel
                {
                    EventID = e.EventID,
                    EventName = e.EventName,
                    DateTime = e.DateTime,
                    Location = e.Location,
                    Description = e.Description,
                    ClubName = e.Club.ClubName,
                    IsOrganizer = e.UserID == currentUser.Id,
                    IsParticipant = e.EventParticipants.Any(ep => ep.UserID == currentUser.Id)
                })
                .ToListAsync();

            return View(userEvents);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> JoinEvent(int eventId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var eventToJoin = await _context.Events.FindAsync(eventId);

            if (eventToJoin == null)
            {
                return NotFound();
            }

            var alreadyJoined = await _context.EventParticipants
                .AnyAsync(ep => ep.EventID == eventId && ep.UserID == currentUser.Id);

            if (alreadyJoined)
            {
                TempData["Message"] = "You have already joined this event.";
                return RedirectToAction(nameof(Index));
            }

            var eventParticipant = new EventParticipant
            {
                EventID = eventId,
                UserID = currentUser.Id
            };

            _context.EventParticipants.Add(eventParticipant);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"You have successfully joined the event: {eventToJoin.EventName}!";
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public async Task<IActionResult> ManageEvent(int id)
        {
            var @event = await _context.Events
                .Include(e => e.Club)
                .Include(e => e.EventParticipants)
                    .ThenInclude(ep => ep.User)
                .FirstOrDefaultAsync(e => e.EventID == id);

            if (@event == null)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (@event.UserID != currentUser.Id)
            {
                return Forbid();
            }

            return View(@event);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateEvent(Event model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "There was an error updating the event. Please check the form and try again.";
                return RedirectToAction(nameof(ManageEvent), new { id = model.EventID });
            }

            var eventToUpdate = await _context.Events
                .Include(e => e.EventParticipants)
                .Include(e => e.Leaderboard)
                .FirstOrDefaultAsync(e => e.EventID == model.EventID);

            if (eventToUpdate == null)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (eventToUpdate.UserID != currentUser.Id)
            {
                return Forbid();
            }

            // Update the properties
            eventToUpdate.EventName = model.EventName;
            eventToUpdate.EventType = model.EventType;
            eventToUpdate.DateTime = model.DateTime;
            eventToUpdate.Location = model.Location;
            eventToUpdate.Description = model.Description;

            try
            {
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Event updated successfully!";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(model.EventID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(ManageEvent), new { id = model.EventID });
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.EventID == id);
        }

        [Authorize]
        public async Task<IActionResult> ManageLeaderboard(int eventId)
        {
            var @event = await _context.Events
                .Include(e => e.Leaderboard)
                    .ThenInclude(l => l.Entries)
                        .ThenInclude(e => e.User)
                .FirstOrDefaultAsync(e => e.EventID == eventId);

            if (@event == null)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (@event.UserID != currentUser.Id)
            {
                return Forbid();
            }

            return View(@event);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddLeaderboardEntry(int eventId, string userId, int rank, TimeSpan time)
        {
            var @event = await _context.Events
                .Include(e => e.Leaderboard)
                .FirstOrDefaultAsync(e => e.EventID == eventId);

            if (@event == null)
            {
                return NotFound();
            }

            if (@event.Leaderboard == null)
            {
                @event.Leaderboard = new Leaderboard { EventID = eventId, Entries = new List<LeaderboardEntry>() };
            }
            else if (@event.Leaderboard.Entries == null)
            {
                @event.Leaderboard.Entries = new List<LeaderboardEntry>();
            }

            var entry = new LeaderboardEntry
            {
                LeaderboardID = @event.Leaderboard.LeaderboardID,
                UserID = userId,
                Rank = rank,
                Time = time
            };

            @event.Leaderboard.Entries.Add(entry);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ManageEvent), new { id = eventId });
        }
    }
}