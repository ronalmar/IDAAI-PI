using System.ComponentModel.DataAnnotations;

namespace IDAAI_API.Entidades.Operations.Requests
{
    public class EditarEstudianteRequest
    {
        [Required]
        public int Id { get; set; }

        [StringLength(maximumLength: 100)]
        public string Nombres { get; set; }

        [StringLength(maximumLength: 100)]
        public string Apellidos { get; set; }

        [StringLength(maximumLength: 10)]
        public string Matricula { get; set; }

        [EmailAddress]
        [StringLength(maximumLength: 50)]
        public string Email { get; set; }

        [StringLength(maximumLength: 100)]
        public string Direccion { get; set; }

        [StringLength(maximumLength: 100)]
        public string Carrera { get; set; }

        [StringLength(maximumLength: 50)]
        public string Modulo { get; set; }

        [Required]
        [StringLength(maximumLength: 25)]
        public string Usuario { get; set; }
    }
}
