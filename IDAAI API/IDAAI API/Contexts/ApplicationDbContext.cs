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
        public DbSet<AutenticacionDTO> Autenticacion { get; set; }
        public DbSet<Modulo> Modulos { get; set; }
        public DbSet<Carrera> Carreras { get; set; }

        public DbSet<Inventario> Inventario { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<PrestamoEstudiante> PrestamosPorEstudiantes { get; set; }
        public DbSet<PrestamoModulo> PrestamosPorModulos { get; set; }
    }
}
