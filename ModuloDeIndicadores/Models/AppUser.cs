using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModuloDeIndicadores.Models
{
    public class AppUser : IdentityUser
    {
        public string Numero { get; set; }
        public string Identificador { get; set; }
        public string NombreUsuario { get; set; }
    }
}
