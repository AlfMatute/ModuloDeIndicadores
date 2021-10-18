using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ModuloDeIndicadores.Data;
using ModuloDeIndicadores.Models;

namespace ModuloDeIndicadores.Pages.Notas
{
    [Authorize(Roles = "Admin,Digitador")]
    public class EditModel : PageModel
    {
        private readonly ModuloDeIndicadores.Data.ApplicationDbContext _context;

        public EditModel(ModuloDeIndicadores.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Nota Nota { get; set; }
        [BindProperty]
        public IFormFile Adjunto { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Nota = await _context.Nota.FirstOrDefaultAsync(m => m.Id_Nota == id);

            if (Nota == null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (Adjunto != null)
                {
                    using (var ms = new MemoryStream())
                    {
                        Adjunto.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        Nota.Adjunto = fileBytes;
                        Nota.AdjuntoNombre = Adjunto.FileName;
                        Nota.AdjuntoTipo = Adjunto.ContentType;
                    }
                }
                if (!ModelState.IsValid)
                {
                    return Page();
                }

                _context.Attach(Nota).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NotaExists(Nota.Id_Nota))
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
            catch(Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error al querer actualizar la nota: " + ex.Message);
                return Page();
            }
        }

        private bool NotaExists(int id)
        {
            return _context.Nota.Any(e => e.Id_Nota == id);
        }
    }
}
