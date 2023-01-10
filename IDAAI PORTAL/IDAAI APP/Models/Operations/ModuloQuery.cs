using System.ComponentModel.DataAnnotations;
using IDAAI_API.Entidades.Operations.Consultas;

namespace IDAAI_API.Entidades.Operations.Estudiante
{
    public class ModuloQuery : PaginacionQuery
    {
        [StringLength(maximumLength: 50)]
        public string Nombre { get; set; }

        [StringLength(maximumLength: 50)]
        public string PeriodoAcademico { get; set; }
    }
}
