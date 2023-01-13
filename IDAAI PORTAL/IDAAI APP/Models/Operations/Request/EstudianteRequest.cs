using System.ComponentModel.DataAnnotations;

namespace IDAAI_APP.Models.Operations.Request
{
    public class EstudianteRegistrarRequest
    {
        [Required]
        [StringLength(maximumLength: 100)]
        public string Nombres { get; set; }

        [Required]
        [StringLength(maximumLength: 100)]
        public string Apellidos { get; set; }

        [Required]
        [StringLength(maximumLength: 10)]
        public string Matricula { get; set; }

        [EmailAddress]
        [StringLength(maximumLength: 50)]
        public string Email { get; set; }

        [StringLength(maximumLength: 100)]
        public string Direccion { get; set; }

        [StringLength(maximumLength: 100)]
        public string Carrera { get; set; }

        [Required]
        [StringLength(maximumLength: 50)]
        public string Modulo { get; set; }
    }

    public class EstudianteEditarRequest
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
    }
}
