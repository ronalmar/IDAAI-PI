using System.ComponentModel.DataAnnotations;

namespace IDAAI_API.Entidades.Operations.Estudiante
{
    public class CarreraQuery: ModuloQuery
    {
        [Required]
        [StringLength(maximumLength: 100)]
        public string Carrera { get; set; }
    }
}
