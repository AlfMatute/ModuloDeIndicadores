using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ModuloDeIndicadores.Data;
using ModuloDeIndicadores.Models;

namespace ModuloDeIndicadores.Pages.Indicadores
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly ModuloDeIndicadores.Data.ApplicationDbContext _context;

        public EditModel(ModuloDeIndicadores.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Indicador Indicador { get; set; }
        public List<SelectListItem> Options { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Indicador = await _context.Indicador.FirstOrDefaultAsync(m => m.Id_Indicador == id);
            
            if (Indicador == null)
            {
                return NotFound();
            }

            Options = _context.AppUser.Select(a =>
                              new SelectListItem
                              {
                                  Value = a.Id,
                                  Text = a.NombreUsuario
                              }).ToList();
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var usuario = _context.Users.Where(u => u.Id == Indicador.UsuarioId).FirstOrDefault();
            Indicador.Usuario = usuario;
            var responsable = _context.Users.Where(u => u.Id == Indicador.Responsable.Id).FirstOrDefault();
            Indicador.Responsable = responsable;
            _context.Attach(Indicador).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IndicadorExists(Indicador.Id_Indicador))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool IndicadorExists(int id)
        {
            return _context.Indicador.Any(e => e.Id_Indicador == id);
        }
    }
}
