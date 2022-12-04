namespace IDAAI_API.DTOs
{
    public class AsistenciaDTO
    {
        public int Id { get; set; }
        public int IdEstudiante { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Matricula { get; set; }
        public string Email { get; set; }
        public DateTime Fecha { get; set; }
        public string EstadoAsistencia { get; set; }
        public string Carrera { get; set; }
        public string Modulo { get; set; }
    }
}
