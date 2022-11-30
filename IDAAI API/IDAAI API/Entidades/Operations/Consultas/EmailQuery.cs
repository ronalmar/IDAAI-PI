using System.ComponentModel.DataAnnotations;

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
