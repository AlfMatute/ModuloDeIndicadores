using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ModuloDeIndicadores.Models
{
    public class Nota
    {
        [Key]
        public int Id_Nota { get; set; }
        public Indicador Indicador { get; set; }
        [Display(Name = "Nota")]
        public string Descripcion { get; set; }
        public double Logro { get; set; }
        public double Puntos { get; set; }
        [Range(1, 12,
        ErrorMessage = "El valor del mes {0} debe ser entre {1} y {2}.")]
        public int Mes { get; set; }
        public int Año { get; set; }
        public byte[] Adjunto { get; set; }
        public string AdjuntoTipo { get; set; }
        public string AdjuntoNombre { get; set; }
    }
}
