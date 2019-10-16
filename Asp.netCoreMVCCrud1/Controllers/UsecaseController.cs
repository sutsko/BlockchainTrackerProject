using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Asp.netCoreMVCCrud1.Models;

namespace Asp.netCoreMVCCrud1.Controllers
{
    public class UsecaseController : Controller
    {
        private readonly ProjectContext _context;

        public UsecaseController(ProjectContext context)
        {
            _context = context;
        }

        // GET: Usecase
        public async Task<IActionResult> Index()
        {
            return View(await _context.Usecases.ToListAsync());
        }

        // GET: Usecase/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usecase = await _context.Usecases
                .FirstOrDefaultAsync(m => m.UsecaseId == id);
            if (usecase == null)
            {
                return NotFound();
            }

            return View(usecase);
        }

        // GET: Usecase/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Usecase/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UsecaseId,UsecaseName")] Usecase usecase)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usecase);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(usecase);
        }

        // GET: Usecase/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usecase = await _context.Usecases.FindAsync(id);
            if (usecase == null)
            {
                return NotFound();
            }
            return View(usecase);
        }

        // POST: Usecase/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UsecaseId,UsecaseName")] Usecase usecase)
        {
            if (id != usecase.UsecaseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usecase);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsecaseExists(usecase.UsecaseId))
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
            return View(usecase);
        }

        // GET: Usecase/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usecase = await _context.Usecases
                .FirstOrDefaultAsync(m => m.UsecaseId == id);
            if (usecase == null)
            {
                return NotFound();
            }

            return View(usecase);
        }

        // POST: Usecase/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usecase = await _context.Usecases.FindAsync(id);
            _context.Usecases.Remove(usecase);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsecaseExists(int id)
        {
            return _context.Usecases.Any(e => e.UsecaseId == id);
        }
    }
}
