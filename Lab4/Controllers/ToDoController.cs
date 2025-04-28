using Lab4.Data;
using Lab4.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lab4.Controllers
{
    public class ToDoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly EmailService _emailService;
        public ToDoController(ApplicationDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        // Displays the list of to-do items
        public IActionResult Index()
        {
            return View(_context.ToDos.ToList());
        }

        #region Create

        // Displays the form to create a new to-do item
        public IActionResult Create()
        {
            return View();
        }

        // Handles the HTTP POST request to create a new to-do item
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Lab4.Models.ToDo todo)
        {
            if (ModelState.IsValid)
            {
                _context.ToDos.Add(todo);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(todo);
        }

        #endregion

        #region Details

        // Displays details of a specific to-do item
        public IActionResult Details(int id)
        {
            if (id == null || _context.ToDos == null)
            {
                return RedirectToAction("Index");
            }

            var toDos = _context.ToDos.FirstOrDefault(x => x.Id == id);
            if (toDos == null)
            {
                return NotFound();
            }

            return View(toDos);
        }

        #endregion

        #region Edit

        // Displays the form to edit a specific to-do item
        public IActionResult Edit(int? id)
        {
            if (id == null || _context.ToDos == null)
            {
                return RedirectToAction("Index");
            }

            var toDos = _context.ToDos.FirstOrDefault(x => x.Id == id);
            if (toDos == null)
            {
                return NotFound();
            }
            return View(toDos);
        }

        // Handles the HTTP POST request to edit a specific to-do item
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int? id, Lab4.Models.ToDo toDo)
        {
            if (id != toDo.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _context.ToDos.Update(toDo);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        #endregion

        #region Delete

        // Displays a confirmation page for deleting a specific to-do item
        public IActionResult Delete(int id)
        {
            if (id == null || _context.ToDos == null)
            {
                return RedirectToAction("Index");
            }

            var toDos = _context.ToDos.FirstOrDefault(x => x.Id == id);
            if (toDos == null)
            {
                return NotFound();
            }

            return View(toDos);
        }

        // Handles the HTTP POST request to delete a specific to-do item
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public IActionResult DeleteConfirm(int id)
        {
            if (id == null || _context.ToDos == null)
            {
                return NotFound();
            }

            var toDos = _context.ToDos.FirstOrDefault(x => x.Id == id);
            if (toDos != null)
            {
                _context.ToDos.Remove(toDos);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        #endregion

        #region Email
        public async Task<IActionResult> SendTaskEmail(int id, string toEmail)
        {
            var item = await _context.ToDos.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            var subject = $"To-Do Task: {item.Title}";
            var body = $"Task: {item.Title}\nDescription: {item.Details}\nDate created: {item.DateCreated}\nCompleted: {item.Status}";
            try
            {
                await _emailService.SendEmailAsync(toEmail, subject, body);
                TempData["Message"] = "Email sent successfully!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Failed to send email: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> CheckEmails(string protocol = "IMAP")
        {
            List<string> messages = new List<string>(); // ← Объявляем заранее пустой список
            try
            {
                if (protocol.ToUpper() == "IMAP")
                {
                    messages = await _emailService.CheckInboxImapAsync();
                }
                else if (protocol.ToUpper() == "POP3")
                {
                    messages = await _emailService.CheckInboxPop3Async();
                }
                else
                {
                    TempData["Error"] = "Unsupported protocol selected.";
                    return RedirectToAction("Index"); // Или куда-то еще
                }

                ViewBag.Messages = messages;
                ViewBag.Protocol = protocol;
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Failed to check emails: {ex.Message}";
                ViewBag.Messages = messages; // даже если ошибка, передаем пустой список
                ViewBag.Protocol = protocol;
            }

            return View();
        }
        #endregion
    }
}
