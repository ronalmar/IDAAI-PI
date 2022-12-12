using System.ComponentModel.DataAnnotations;
using IDAAI_API.Entidades.Operations.Consultas;

namespace IDAAI_API.Entidades.Operations.Estudiante
{
    public class EstudianteMatriculaQuery : EstudianteModuloQuery
    {
        [Required]
        [StringLength(maximumLength: 10)]
        public string Matricula { get; set; }
    }
}
