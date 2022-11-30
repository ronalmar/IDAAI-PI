using System.ComponentModel.DataAnnotations;

namespace IDAAI_API.Entidades.Operations.Estudiante
{
    public class NombresQuery: ModuloQuery
    {
        [Required]
        [StringLength(maximumLength: 100)]
        public string Nombres { get; set; }

        [Required]
        [StringLength(maximumLength: 100)]
        public string Apellidos { get; set; }
    }
}
