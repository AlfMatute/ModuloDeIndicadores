using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModuloDeIndicadores.Data;

namespace ModuloDeIndicadores.Controllers
{
    [Route("api/Notaadjunto")]
    [ApiController]
    public class NotaAdjuntoController : Controller
    {
        private readonly ApplicationDbContext _db;
        public NotaAdjuntoController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult DocumentDownload(int id)
        {
            try
            {
                var document = _db.Nota.Where(o => o.Id_Nota == id).FirstOrDefault();
                return File(document.Adjunto, document.AdjuntoTipo, document.AdjuntoNombre);
            }
            catch
            {
                return null;
            }
        }
    }
}
