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
    public class IndustryController : Controller
    {
        private readonly ProjectContext _context;

        public IndustryController(ProjectContext context)
        {
            _context = context;
        }

        // GET: Industry
        public async Task<IActionResult> Index()
        {
            var projectContext = _context.Industries.Include(i => i.Sector);
            return View(await projectContext.ToListAsync());
        }

        // GET: Industry/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var industry = await _context.Industries
                .Include(i => i.Sector)
                .FirstOrDefaultAsync(m => m.IndustryId == id);
            if (industry == null)
            {
                return NotFound();
            }

            return View(industry);
        }

        // GET: Industry/Create
        public IActionResult Create()
        {
            ViewData["SectorId"] = new SelectList(_context.Sectors, "SectorId", "SectorId");
            return View();
        }

        // POST: Industry/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IndustryId,IndustryName,IndustryDescription,SectorId")] Industry industry)
        {
            if (ModelState.IsValid)
            {
                _context.Add(industry);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SectorId"] = new SelectList(_context.Sectors, "SectorId", "SectorId", industry.SectorId);
            return View(industry);
        }

        // GET: Industry/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var industry = await _context.Industries.FindAsync(id);
            if (industry == null)
            {
                return NotFound();
            }
            ViewData["SectorId"] = new SelectList(_context.Sectors, "SectorId", "SectorId", industry.SectorId);
            return View(industry);
        }

        // POST: Industry/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IndustryId,IndustryName,IndustryDescription,SectorId")] Industry industry)
        {
            if (id != industry.IndustryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(industry);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IndustryExists(industry.IndustryId))
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
            ViewData["SectorId"] = new SelectList(_context.Sectors, "SectorId", "SectorId", industry.SectorId);
            return View(industry);
        }

        // GET: Industry/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var industry = await _context.Industries
                .Include(i => i.Sector)
                .FirstOrDefaultAsync(m => m.IndustryId == id);
            if (industry == null)
            {
                return NotFound();
            }

            return View(industry);
        }

        // POST: Industry/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var industry = await _context.Industries.FindAsync(id);
            _context.Industries.Remove(industry);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IndustryExists(int id)
        {
            return _context.Industries.Any(e => e.IndustryId == id);
        }

        public List<Industry> GetInduList()
        {
            List<Industry> Indulist = _context.Industries.ToList();

            return Indulist;
        }

    }
}
