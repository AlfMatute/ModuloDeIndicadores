using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ModuloDeIndicadores.Data;
using ModuloDeIndicadores.Models;

namespace ModuloDeIndicadores.Pages.Indicadores
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly ModuloDeIndicadores.Data.ApplicationDbContext _context;

        public CreateModel(ModuloDeIndicadores.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            Options = _context.AppUser.Select(a =>
                              new SelectListItem
                              {
                                  Value = a.Id,
                                  Text = a.NombreUsuario
                              }).ToList();
            return Page();
        }

        [BindProperty]
        public Indicador Indicador { get; set; }
        public List<SelectListItem> Options { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Page();
                }
                var usuario = _context.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
                Indicador.Usuario = usuario;
                var responsable = _context.Users.Where(u => u.Id == Indicador.Responsable.Id).FirstOrDefault();
                Indicador.Responsable = responsable;
                _context.Indicador.Add(Indicador);
                Nota nota = new Nota();
                nota.Indicador = Indicador;
                nota.Mes = DateTime.Now.Month;
                nota.Año = DateTime.Now.Year;
                await _context.SaveChangesAsync();
                _context.Nota.Add(nota);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }
            catch(Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error al querer crear la nota: " + ex.Message);
                return Page();
            }
        }
    }
}
