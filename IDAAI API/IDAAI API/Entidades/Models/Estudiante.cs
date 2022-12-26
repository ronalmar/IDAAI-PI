namespace IDAAI_API.Entidades.Models
{
    public class Estudiante
    {
        public int? Id { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Matricula { get; set; }
        public string? Email { get; set; }
        public string? Direccion { get; set; }       
        public string? Carrera { get; set; }
        public string? Modulo { get; set; }
    }
}
