namespace IDAAI_API.Entidades.Models
{
    public class PrestamoModulo
    {
        public int Id { get; set; }
        public DateTime FechaPrestado { get; set; }
        public DateTime? FechaDevuelto { get; set; }
        public string Inventario { get; set; }
        public string Item { get; set; }
        public string Modulo { get; set; }
        public string EstadoDevolucion { get; set; }
    }
}
