using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ModuloDeIndicadores.Models
{
    public class Indicador
    {
        [Key]
        public int Id_Indicador { get; set; }
        public IdentityUser Usuario { get; set; }
        [Display(Name = "Nombre")]
        public string Nombre_Indicador { get; set; }
        [Display(Name = "Descripción")]
        public string Descripcion_Indicadores { get; set; }
        public string Medida { get; set; }
        [Display(Name = "Objetivo Estratégico")]
        public string Objetivo { get; set; }
        [Display(Name = "Perfil del puesto")]
        public string Perfil { get; set; }
        [Display(Name = "Ponderación")]
        public double Ponderacion { get; set; }
        public double Base { get; set; }
        public double Meta { get; set; }
        public string Periodo { get; set; }
        [Display(Name = "Fecha aproximada de recepción")]
        public string Fecha_Aproximada { get; set; }
        public IdentityUser Responsable { get; set; }
        public string UsuarioId { get; set; }
        public string ResponsableId { get; set; }
        public string Departamento { get; set; }
    }
}
