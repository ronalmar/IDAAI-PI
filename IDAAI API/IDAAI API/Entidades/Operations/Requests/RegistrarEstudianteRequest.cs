using System.ComponentModel.DataAnnotations;

namespace IDAAI_API.Entidades.Operations.Requests
{
    public class RegistrarEstudianteRequest
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

        [Required]
        [StringLength(maximumLength: 25)]
        public string Usuario { get; set; }
    }

    public class RegistrarGrupoEstudianteRequest
    {
        [Required]
        [StringLength(maximumLength: 50)]
        public string Modulo { get; set; }

        [Required]
        public List<EstudianteRegistrarRequest> Estudiantes { get; set; }

        [Required]
        [StringLength(maximumLength: 25)]
        public string Usuario { get; set; }
    }

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

        [Required]
        [StringLength(maximumLength: 25)]
        public string Usuario { get; set; }
    }

}
