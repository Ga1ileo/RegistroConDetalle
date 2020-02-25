using Microsoft.EntityFrameworkCore;
using RegistroConDetalle.Entidades;

namespace RegistroConDetalle.DAL
{
    public class Contexto : DbContext
    {
        public DbSet<Personas> Personas { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data Source = DAL\DATA\RegistroDetalle.db");
        }
    }
}
