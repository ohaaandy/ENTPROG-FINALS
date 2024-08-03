using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunBuddies.DataModel;
using RunBuddies.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace RunBuddies.Controllers
{
    public class EventsController : Controller
    {
        private readonly AppDBContext _context;

        public EventsController(AppDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var events = await _context.Events
                .Select(e => new EventViewModel
                {
                    EventID = e.EventID,
                    EventName = e.EventName,
                    DateTime = e.DateTime,
                    Location = e.Location,
                    Description = e.Description,
                    OrganizerName = e.User.Username // Assuming User has a Username property // Add Event image
                })
                .ToListAsync();

            return View(events);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventViewModel = await _context.Events
                .Where(e => e.EventID == id)
                .Select(e => new EventViewModel
                {
                    EventID = e.EventID,
                    EventName = e.EventName,
                    DateTime = e.DateTime,
                    Location = e.Location,
                    Description = e.Description,
                    OrganizerName = e.User.Username,
                    // You could set HasUserJoined here based on some logic
                })
                .FirstOrDefaultAsync();

            if (eventViewModel == null)
            {
                return NotFound();
            }

            return View(eventViewModel);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventViewModel eventViewModel)
        {
            if (ModelState.IsValid)
            {
                var newEvent = new Event
                {
                    EventName = eventViewModel.EventName,
                    DateTime = eventViewModel.DateTime,
                    Location = eventViewModel.Location,
                    Description = eventViewModel.Description,
                    // Set other properties as needed
                };

                _context.Add(newEvent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(eventViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> JoinEvent(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }

            // Implement join event logic here
            // For example, you might add the current user to the event's participants

            return RedirectToAction(nameof(Details), new { id = @event.EventID });
        }
    }
}