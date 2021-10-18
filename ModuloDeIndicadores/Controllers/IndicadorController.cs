using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModuloDeIndicadores.Data;
using ModuloDeIndicadores.Models;

namespace ModuloDeIndicadores.Controllers
{
    [Route("api/Indicador")]
    [ApiController]
    public class IndicadorController : Controller
    {
        private readonly ApplicationDbContext _db;
        public IndicadorController(ApplicationDbContext db)
        {
            _db = db;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var room_query = await (from indicador in _db.Indicador
                              join usuario in _db.AppUser
                              on indicador.ResponsableId equals usuario.Id
                              select new { indicador.Id_Indicador,
                                  indicador.Nombre_Indicador,
                                  indicador.Descripcion_Indicadores,
                                  indicador.Medida,
                                  indicador.Objetivo,
                                  indicador.Perfil,
                                  indicador.Ponderacion,
                                  indicador.Base,
                                  indicador.Meta,
                                  indicador.Departamento,
                                  indicador.Periodo,
                                  indicador.Fecha_Aproximada,
                                  usuario.NombreUsuario }).ToListAsync();

            return Json(new { data = room_query });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var datoDeBd = await _db.Indicador.FirstOrDefaultAsync(b => b.Id_Indicador == id);
                if (datoDeBd == null)
                {
                    return Json(new { success = false, message = "Error al borrar" });
                }
                _db.Indicador.Remove(datoDeBd);
                await _db.SaveChangesAsync();
                return Json(new { success = true, message = "Borrado exitoso" });
            }
            catch(Exception ex)
            {
                return Json(new { success = false, message = "Error al eliminar: " + ex.Message });
            }
        }
        //[HttpPost]
        //public async Task<IActionResult> UploadFile(IFormFile file)
        //{
        //    if (file == null || file.Length == 0)
        //        return Content("file not selected");

        //    var path = Path.Combine(
        //                Directory.GetCurrentDirectory(), "wwwroot",
        //                file.FileName);

        //    using (var stream = new FileStream(path, FileMode.Create))
        //    {
        //        await file.CopyToAsync(stream);
        //    }

        //    return RedirectToAction("Files");
        //}

        [HttpPost]
        public async Task<IActionResult> Uploadfile(IList<IFormFile> files)
        {
            try
            {
                List<Indicador> _indicadores = new List<Indicador>();
                List<string> _files = new List<string>();

                foreach (var file in files)
                {
                    var path = Path.Combine(
                            Directory.GetCurrentDirectory(), "wwwroot",
                            file.FileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {

                        await file.CopyToAsync(stream);
                        _files.Add(path);
                    }
                }

                foreach (string filename in _files)
                {
                    //string filename = ContentDispositionHeaderValue.Parse(source.ContentDisposition).FileName.Trim('"');

                    //filename = this.EnsureCorrectFilename(filename);

                    System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                    using (var stream = System.IO.File.Open(filename, FileMode.Open, FileAccess.Read))
                    {
                        using (var reader = ExcelReaderFactory.CreateReader(stream))
                        {

                            while (reader.Read()) //Each row of the file
                            {
                                _indicadores.Add(new Indicador
                                {
                                    Nombre_Indicador = reader.GetValue(0).ToString(),
                                    Descripcion_Indicadores = reader.GetValue(1).ToString(),
                                    Medida = reader.GetValue(2).ToString(),
                                    Objetivo = reader.GetValue(3).ToString(),
                                    Perfil = reader.GetValue(4).ToString(),
                                    Ponderacion = Double.Parse(reader.GetValue(5).ToString()),
                                    Base = Double.Parse(reader.GetValue(6).ToString()),
                                    Meta = Double.Parse(reader.GetValue(7).ToString()),
                                });
                            }
                        }
                    }
                    if (System.IO.File.Exists(filename))
                    {
                        System.IO.File.Delete(filename);
                    }
                }
                foreach(Indicador ind in _indicadores)
                {
                    _db.Indicador.Add(ind);
                }
                await _db.SaveChangesAsync();
                return Json(new { success = true, message = "Subida exitosa" });
            }
            catch(Exception ex)
            {
                return Json(new { success = false, message = "Error al subir: " + ex.Message });
            }
        }

        private string EnsureCorrectFilename(string filename)
        {
            if (filename.Contains("\\"))
                filename = filename.Substring(filename.LastIndexOf("\\") + 1);

            return filename;
        }
    }
}
