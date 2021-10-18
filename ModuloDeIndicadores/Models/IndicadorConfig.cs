using System.ComponentModel.DataAnnotations;

namespace ModuloDeIndicadores.Models
{
    public class IndicadorConfig
    {
        [Key]
        public int Id { get; set; }
        public int Mes { get; set; }
        public int Año { get; set; }
    }
}
