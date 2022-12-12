using System.ComponentModel.DataAnnotations;
using IDAAI_API.Entidades.Operations.Consultas;

namespace IDAAI_API.Entidades.Operations.Estudiante
{
    public class EstudianteEmailQuery: EstudianteModuloQuery
    {
        [Required]
        [EmailAddress]
        [StringLength(maximumLength: 50)]
        public string Email { get; set; }
    }
}
