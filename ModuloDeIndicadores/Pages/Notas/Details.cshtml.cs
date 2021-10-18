using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ModuloDeIndicadores.Data;
using ModuloDeIndicadores.Models;

namespace ModuloDeIndicadores.Pages.Notas
{
    [Authorize(Roles = "Admin,Digitador")]
    public class DetailsModel : PageModel
    {
        private readonly ModuloDeIndicadores.Data.ApplicationDbContext _context;

        public DetailsModel(ModuloDeIndicadores.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Nota Nota { get; set; }

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
    }
}
