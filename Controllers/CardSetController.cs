using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace WebApp.Controllers
{
    
    public class CardSetController : Controller
    {
        private readonly AppDbContext _context;
        
        public CardSetController(AppDbContext context)
        {
            _context = context;
        }

        // GET: CardSet
        [Authorize]
        public async Task<IActionResult> Index()
        {
            //var userId = Convert.ToInt32(User.FindFirstValue("UserId"));
            // Получаем текущего пользователя
            var currentUser = await _context.Users
                .Include(u => u.CardSets) // Включаем связанные наборы карточек пользователя
                .FirstOrDefaultAsync(u => u.Id == Convert.ToInt32(User.FindFirstValue("UserId"))); // Здесь предполагается, что у вас есть метод для получения ID текущего пользователя

            // Проверяем, что пользователь существует
            if (currentUser != null)
            {
                // Возвращаем наборы карточек текущего пользователя
                return View(currentUser.CardSets);
            }
            else
            {
                // Если пользователь не найден, возвращаем пустую коллекцию
                return View(new List<CardSet>());
            }
        }

        // GET: CardSet/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: CardSet/Create
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(string title)
        {
            var userId = Convert.ToInt32(User.FindFirstValue("UserId"));
            

            var cardSet = new CardSet
            {
                Title = title,
                UserId = userId,
            };
            if (ModelState.IsValid)
            {
                _context.Add(cardSet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            if (!ModelState.IsValid)
            {
                foreach (var modelState in ViewData.ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        Console.WriteLine(error.ErrorMessage); // или используйте любой другой механизм для вывода сообщений об ошибках
                    }
                }
            }
            return View(cardSet);
        }
        [Authorize]
        public IActionResult Test(int id)
        {
            var cardSet = _context.CardSets
                .Include(cs => cs.Cards)
                .FirstOrDefault(cs => cs.Id == id);

            if (cardSet == null)
            {
                return NotFound();
            }
            var cards = cardSet.Cards.Select(c => new
            {
                Id = c.Id,
                FrontText = c.FrontText,
                BackText = c.BackText,
                CardSetId = c.CardSetId
            }).ToList(); ;

            ViewBag.Cards = cards;

            return View(cardSet);
        }
        [Authorize]
        public IActionResult Statistics(int correct, int incorrect)
        {
            ViewBag.CorrectCount = correct;
            ViewBag.IncorrectCount = incorrect;
            return View();
        }



        // GET: CardSet/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cardSet = await _context.CardSets.FindAsync(id);
            if (cardSet == null)
            {
                return NotFound();
            }
            return View(cardSet);
        }

        // POST: CardSet/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title")] CardSet cardSet)
        {
            if (id != cardSet.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cardSet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CardSetExists(cardSet.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(cardSet);
        }

        // GET: CardSet/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cardSet = await _context.CardSets
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cardSet == null)
            {
                return NotFound();
            }

            return View(cardSet);
        }

        // POST: CardSet/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cardSet = await _context.CardSets.FindAsync(id);
            _context.CardSets.Remove(cardSet);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cardSet = await _context.CardSets
                .Include(cs => cs.Cards)
                .FirstOrDefaultAsync(m => m.Id == id);

            int firstCardId = await _context.Cards
        .Where(c => c.CardSetId == id)
        .Select(c => c.Id)
        .FirstOrDefaultAsync();

            // Сохраняем значения в ViewBag
            ViewBag.CardSetId = id;
            ViewBag.FirstCardId = firstCardId;

            if (cardSet == null)
            {
                return NotFound();
            }

            return View(cardSet);
        }

        private bool CardSetExists(int id)
        {
            return _context.CardSets.Any(e => e.Id == id);
        }
    }
}