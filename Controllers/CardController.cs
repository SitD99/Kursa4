using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class CardController : Controller
    {
        private readonly AppDbContext _context;

        public CardController(AppDbContext context)
        {
            _context = context;
        }


        [Authorize]
        public async Task<IActionResult> Details(int? cardSetId, int? id)
        {
            
            if (id == 0)
            {
                return RedirectToAction("Create", "Card", new { cardSetId = cardSetId });
            }

            if (id == null)
            {
                return NotFound();
            }

            var card = await _context.Cards
                .Include(c => c.CardSet)
                .FirstOrDefaultAsync(m => m.CardSetId == cardSetId && m.Id == id);
            if (card == null)
            {
                id = 0;
                return RedirectToAction("Create", "Card", new { cardSetId = cardSetId});
            }

            ViewBag.CardId = card.Id;
            ViewBag.CardSetId = card.CardSetId;

            return View(card);

        }
        [HttpGet]
        public async Task<JsonResult> GetNextCardId(int cardSetId, int currentId)
        {
            var nextCardId = await _context.Cards
                .Where(c => c.CardSetId == cardSetId && c.Id > currentId)
                .OrderBy(c => c.Id)
                .Select(c => c.Id)
                .FirstOrDefaultAsync();

            if (nextCardId == 0)
            {
                nextCardId = await _context.Cards
                    .Where(c => c.CardSetId == cardSetId)
                    .OrderBy(c => c.Id)
                    .Select(c => c.Id)
                    .FirstOrDefaultAsync();
            }
            return Json(new { nextCardId });
        }
        [HttpGet]
        public async Task<JsonResult> GetPrevCardId(int cardSetId, int currentId)
        {
            var prevCardId = await _context.Cards
                .Where(c => c.CardSetId == cardSetId && c.Id < currentId)
                .OrderByDescending(c => c.Id)
                .Select(c => c.Id)
                .FirstOrDefaultAsync();
            if (prevCardId == 0)
            {
                prevCardId = await _context.Cards
                    .Where(c => c.CardSetId == cardSetId)
                    .OrderByDescending(c => c.Id)
                    .Select(c => c.Id)
                    .FirstOrDefaultAsync();
            }

            return Json(new { prevCardId });
        }



        // GET: Card/Create
        [Authorize]
        public IActionResult Create(int cardSetId)
        {
            ViewBag.CardSetId = cardSetId;
            return View();
        }

        // POST: Card/Create
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(string frontText, string backText, int cardSetId)
        {
            var card = new Card
            {
                FrontText = frontText,
                BackText = backText,
                CardSetId = cardSetId
            };
            if (ModelState.IsValid)
            {
                _context.Add(card);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "CardSet", new { id = card.CardSetId });
            }
            return View(card);
        }

        // GET: Card/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var card = await _context.Cards.FindAsync(id);
            if (card == null)
            {
                return NotFound();
            }
            return View(card);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(int id, string frontText, string backText, int cardSetId)
        {
            var card = new Card
            {
                Id = id,
                FrontText = frontText,
                BackText = backText,
                CardSetId = cardSetId
            };
            if (id != card.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(card);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CardExists(card.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "CardSet", new { id = card.CardSetId });
            }
            return View(card);
        }

        // GET: Card/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var card = await _context.Cards
                .Include(c => c.CardSet)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (card == null)
            {
                return NotFound();
            }

            return View(card);
        }

        // POST: Card/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var card = await _context.Cards.FindAsync(id);
            _context.Cards.Remove(card);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "CardSet", new { id = card.CardSetId });
        }

        private bool CardExists(int id)
        {
            return _context.Cards.Any(e => e.Id == id);
        }
    }
}