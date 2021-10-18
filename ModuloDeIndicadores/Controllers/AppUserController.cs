using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModuloDeIndicadores.Data;

namespace ModuloDeIndicadores.Controllers
{
    [Route("api/AppUser")]
    [ApiController]
    public class AppUserController : Controller
    {
        // GET: AppUserController
        private readonly ApplicationDbContext _db;
        public AppUserController(ApplicationDbContext db)
        {
            _db = db;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Json(new { data = await _db.AppUser.ToListAsync() });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var datoDeBd = await _db.AppUser.FirstOrDefaultAsync(b => b.Id == id);
            if (datoDeBd == null)
            {
                return Json(new { success = false, message = "Error al borrar" });
            }
            _db.AppUser.Remove(datoDeBd);
            await _db.SaveChangesAsync();
            return Json(new { success = true, message = "Borrado exitoso" });
        }
    }
}
