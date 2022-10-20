using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Huellero.Models;

namespace Huellero.Controllers
{
    public class TblUsuariosController : Controller
    {
        private readonly PruebaContext _context;

        public TblUsuariosController(PruebaContext context)
        {
            _context = context;
        }

        // GET: TblUsuarios
        public async Task<IActionResult> Index()
        {
              return View(await _context.TblUsuarios.ToListAsync());
        }

        // GET: TblUsuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TblUsuarios == null)
            {
                return NotFound();
            }

            var tblUsuario = await _context.TblUsuarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tblUsuario == null)
            {
                return NotFound();
            }

            return View(tblUsuario);
        }

        // GET: TblUsuarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TblUsuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,VNombres,VApellidos,VHuella")] TblUsuario tblUsuario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tblUsuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tblUsuario);
        }

        // GET: TblUsuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TblUsuarios == null)
            {
                return NotFound();
            }

            var tblUsuario = await _context.TblUsuarios.FindAsync(id);
            if (tblUsuario == null)
            {
                return NotFound();
            }
            return View(tblUsuario);
        }

        // POST: TblUsuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,VNombres,VApellidos,VHuella")] TblUsuario tblUsuario)
        {
            if (id != tblUsuario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblUsuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblUsuarioExists(tblUsuario.Id))
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
            return View(tblUsuario);
        }

        // GET: TblUsuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TblUsuarios == null)
            {
                return NotFound();
            }

            var tblUsuario = await _context.TblUsuarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tblUsuario == null)
            {
                return NotFound();
            }

            return View(tblUsuario);
        }

        // POST: TblUsuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TblUsuarios == null)
            {
                return Problem("Entity set 'PruebaContext.TblUsuarios'  is null.");
            }
            var tblUsuario = await _context.TblUsuarios.FindAsync(id);
            if (tblUsuario != null)
            {
                _context.TblUsuarios.Remove(tblUsuario);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TblUsuarioExists(int id)
        {
          return _context.TblUsuarios.Any(e => e.Id == id);
        }
    }
}
