namespace IDAAI_API.Entidades.Models
{
    public class PrestamoEstudiante
    {
        public int Id { get; set; }
        public DateTime FechaPrestado { get; set; }
        public DateTime? FechaDevuelto { get; set; }
        public string Inventario { get; set; }
        public string Item { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Matricula { get; set; }
        public string? Email { get; set; }
        public string EstadoDevolucion { get; set; }
    }
}
