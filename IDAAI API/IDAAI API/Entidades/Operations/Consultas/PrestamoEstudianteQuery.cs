using System.ComponentModel.DataAnnotations;
using IDAAI_API.Entidades.Operations.Consultas;

namespace IDAAI_API.Entidades.Operations.Estudiante
{
    public class PrestamoEstudianteQuery : PaginacionQuery
    {
        [StringLength(maximumLength: 10)]
        public string Matricula { get; set; }

        [StringLength(maximumLength: 25)]
        public string Usuario { get; set; }
    }
}
