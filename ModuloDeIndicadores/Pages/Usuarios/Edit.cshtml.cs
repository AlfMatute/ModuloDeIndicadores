using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ModuloDeIndicadores.Models;

namespace ModuloDeIndicadores.Pages.Usuarios
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
        public AppUser AppUser { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            AppUser = await _context.AppUser.FirstOrDefaultAsync(m => m.Id == id);

            if (AppUser == null)
            {
                return NotFound();
            }
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

            try
            {
                var usuario = await _context.AppUser.FindAsync(AppUser.Id);
                if(usuario == null)
                {
                    return NotFound();
                }
                bool correcto = true;
                var datoDeBd = await _context.AppUser.FirstOrDefaultAsync(b => b.Email == AppUser.Email);
                if (datoDeBd != null)
                {
                    if(!datoDeBd.Id.Equals(usuario.Id))
                    {
                        correcto = false;
                    }
                }
                datoDeBd = await _context.AppUser.FirstOrDefaultAsync(b => b.Numero == AppUser.Numero);
                if (datoDeBd != null)
                {
                    if (!datoDeBd.Id.Equals(usuario.Id))
                    {
                        correcto = false;
                    }
                }
                datoDeBd = await _context.AppUser.FirstOrDefaultAsync(b => b.Identificador == AppUser.Identificador);
                if (datoDeBd != null)
                {
                    if (!datoDeBd.Id.Equals(usuario.Id))
                    {
                        correcto = false;
                    }
                }

                if(correcto)
                {
                    usuario.Numero = AppUser.Numero;
                    usuario.Identificador = AppUser.Identificador;
                    usuario.PhoneNumber = AppUser.PhoneNumber;
                    usuario.Email = AppUser.Email;
                    usuario.NombreUsuario = AppUser.NombreUsuario;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Existen datos que ya estan en la base de datos, favor validar");
                    return Page();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppUserExists(AppUser.Id))
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

        private bool AppUserExists(string id)
        {
            return _context.AppUser.Any(e => e.Id == id);
        }
    }
}
