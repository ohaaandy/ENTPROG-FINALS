using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunBuddies.App.Models;
using RunBuddies.DataModel;
using System.Linq;
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
            var events = await _context.Events
                .Include(e => e.Club)
                .Include(e => e.User)
                .OrderBy(e => e.DateTime)
                .ToListAsync();

            return View(events);
        }

        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }
        //    var eventViewModel = await _context.Events
        //        .Where(e => e.EventID == id)
        //        .Select(e => new EventViewModel
        //        {
        //            EventID = e.EventID,
        //            ClubID = e.ClubID,
        //            EventName = e.EventName,
        //            EventType = e.EventType,
        //            DateTime = e.DateTime,
        //            Location = e.Location,
        //            Description = e.Description,
        //            OrganizerName = e.User.UserName,
        //            // HasUserJoined can be set here if needed
        //        })
        //        .FirstOrDefaultAsync();

        //    if (eventViewModel == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(eventViewModel);
        //}

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
                .Where(e => e.UserID == currentUser.Id || e.Participants.Any(p => p.Id == currentUser.Id))
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
                    IsParticipant = e.Participants.Any(p => p.Id == currentUser.Id)
                })
                .ToListAsync();

            return View(userEvents);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> JoinEvent(int eventId)
        {
            var evt = await _context.Events.Include(e => e.Participants).FirstOrDefaultAsync(e => e.EventID == eventId);
            if (evt == null)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);

            if (!evt.Participants.Any(p => p.Id == currentUser.Id))
            {
                evt.Participants.Add(currentUser);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "You have successfully joined the event!";
            }
            else
            {
                TempData["InfoMessage"] = "You are already a participant in this event.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}