using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ModuloDeIndicadores.Models;

namespace ModuloDeIndicadores.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Indicador> Indicador { get; set; }
        public DbSet<Nota> Nota { get; set; }
        public DbSet<AppUser> AppUser { get; set; }
        public DbSet<IndicadorConfig> IndicadorConfig { get; set; }
    }
}
