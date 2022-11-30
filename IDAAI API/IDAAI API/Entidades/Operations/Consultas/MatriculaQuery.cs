using System.ComponentModel.DataAnnotations;

namespace IDAAI_API.Entidades.Operations.Estudiante
{
    public class MatriculaQuery : ModuloQuery
    {
        [Required]
        [StringLength(maximumLength: 10)]
        public string Matricula { get; set; }
    }
}
