using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ModuloDeIndicadores.Models;

namespace ModuloDeIndicadores.Pages.IndicadoresConfig
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
        public IndicadorConfig IndicadorConfig { get; set; }
        public async Task<IActionResult> OnGetAsync(int id = 1)
        {

            IndicadorConfig = await _context.IndicadorConfig.FirstOrDefaultAsync(m => m.Id == id);

            if (IndicadorConfig == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            _context.Attach(IndicadorConfig).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IndicadorExists(IndicadorConfig.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Page();
        }

        private bool IndicadorExists(int id)
        {
            return _context.IndicadorConfig.Any(e => e.Id == id);
        }
    }
}
