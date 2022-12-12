using System.ComponentModel.DataAnnotations;
using IDAAI_API.Entidades.Operations.Consultas;

namespace IDAAI_API.Entidades.Operations.Estudiante
{
    public class EstudianteCarreraQuery: EstudianteModuloQuery
    {
        [Required]
        [StringLength(maximumLength: 100)]
        public string Carrera { get; set; }
    }
}
