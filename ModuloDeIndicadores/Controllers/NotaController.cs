using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModuloDeIndicadores.Data;

namespace ModuloDeIndicadores.Controllers
{
    [Route("api/Nota")]
    [ApiController]
    public class NotaController : Controller
    {
        private readonly ApplicationDbContext _db;
        public NotaController(ApplicationDbContext db)
        {
            _db = db;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var _usuario = await _db.AppUser.Where(u => u.UserName == User.Identity.Name).FirstOrDefaultAsync();
            if(!User.IsInRole("Admin"))
            {
                var config = await _db.IndicadorConfig.FirstOrDefaultAsync();
                var room_query = await (from indicador in _db.Indicador
                                        join usuario in _db.AppUser
                                        on indicador.ResponsableId equals usuario.Id
                                        join nota in _db.Nota
                                        on indicador.Id_Indicador equals nota.Indicador.Id_Indicador
                                        where nota.Año == config.Año && nota.Mes == config.Mes && usuario.Id == _usuario.Id
                                        select new
                                        {
                                            nota.Id_Nota,
                                            indicador.Nombre_Indicador,
                                            indicador.Descripcion_Indicadores,
                                            usuario.NombreUsuario,
                                            indicador.Departamento,
                                            nota.Descripcion,
                                            nota.Logro,
                                            nota.Puntos,
                                            nota.Mes,
                                            nota.Año
                                        }).ToListAsync();

                return Json(new { data = room_query });
            }
            else
            {
                var config = await _db.IndicadorConfig.FirstOrDefaultAsync();
                var room_query = await (from indicador in _db.Indicador
                                        join usuario in _db.AppUser
                                        on indicador.ResponsableId equals usuario.Id
                                        join nota in _db.Nota
                                        on indicador.Id_Indicador equals nota.Indicador.Id_Indicador
                                        where nota.Año == config.Año && nota.Mes == config.Mes
                                        select new
                                        {
                                            nota.Id_Nota,
                                            indicador.Nombre_Indicador,
                                            indicador.Descripcion_Indicadores,
                                            usuario.NombreUsuario,
                                            indicador.Departamento,
                                            nota.Descripcion,
                                            nota.Logro,
                                            nota.Puntos,
                                            nota.Mes,
                                            nota.Año
                                        }).ToListAsync();

                return Json(new { data = room_query });
            }
        }
        public ActionResult Get()
        {
            return View(_db.Nota.ToListAsync());
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var datoDeBd = await _db.Nota.FirstOrDefaultAsync(b => b.Id_Nota == id);
                if (datoDeBd == null)
                {
                    return Json(new { success = false, message = "Error al borrar" });
                }
                _db.Nota.Remove(datoDeBd);
                await _db.SaveChangesAsync();
                return Json(new { success = true, message = "Borrado exitoso" });
            }
            catch(Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
