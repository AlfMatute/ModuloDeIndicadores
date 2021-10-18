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

namespace ModuloDeIndicadores.Pages.Notas
{
    [Authorize(Roles = "Admin,Digitador")]
    public class CreateModel : PageModel
    {
        private readonly ModuloDeIndicadores.Data.ApplicationDbContext _context;

        public CreateModel(ModuloDeIndicadores.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            var usuario = _context.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            Options = _context.Indicador.Where(i => i.ResponsableId == usuario.Id).Select(a =>
                              new SelectListItem
                              {
                                  Value = a.Id_Indicador.ToString(),
                                  Text = a.Descripcion_Indicadores
                              }).ToList();
            return Page();
        }

        [BindProperty]
        public Nota Nota { get; set; }
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

                var inidicador = _context.Indicador.Where(u => u.Id_Indicador == Nota.Indicador.Id_Indicador).FirstOrDefault();
                Nota.Indicador = inidicador;

                _context.Nota.Add(Nota);
                await _context.SaveChangesAsync();

                return RedirectToPage("./Index");
            }
            catch(Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error al querer crear el indicador: " + ex.Message);
                return Page();
            }
        }
    }
}
