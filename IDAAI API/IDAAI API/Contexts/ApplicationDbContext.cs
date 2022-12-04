using IDAAI_API.Entidades.Models;
using Microsoft.EntityFrameworkCore;

namespace IDAAI_API.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Estudiante> Estudiantes { get; set; }
        public DbSet<Asistencia> RegistroAsistencia { get; set; }
        public DbSet<Autenticacion> Autenticacion { get; set; }
        public DbSet<Modulo> Modulos { get; set; }
    }
}
