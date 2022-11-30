using System.ComponentModel.DataAnnotations;
using IDAAI_API.Entidades.Operations.Consultas;

namespace IDAAI_API.Entidades.Operations.Estudiante
{
    public class EmailQuery: ModuloQuery
    {
        [Required]
        [EmailAddress]
        [StringLength(maximumLength: 50)]
        public string Email { get; set; }
    }
}
