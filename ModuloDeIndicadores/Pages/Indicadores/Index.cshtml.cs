using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ModuloDeIndicadores.Data;
using ModuloDeIndicadores.Models;

namespace ModuloDeIndicadores.Pages.Indicadores
{
    public class IndexModel : PageModel
    {
        private readonly ModuloDeIndicadores.Data.ApplicationDbContext _context;

        public IndexModel(ModuloDeIndicadores.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Indicador> Indicador { get;set; }

        public async Task OnGetAsync()
        {
            Indicador = await _context.Indicador.ToListAsync();
        }
    }
}
