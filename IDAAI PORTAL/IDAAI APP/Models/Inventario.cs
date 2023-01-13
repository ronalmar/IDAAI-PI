namespace IDAAI_APP.Models
{
    public class Inventario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int CantidadDisponible { get; set; }
        public int CantidadTotal { get; set; }
    }
}
