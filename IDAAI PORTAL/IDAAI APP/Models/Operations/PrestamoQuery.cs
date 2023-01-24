using System.ComponentModel.DataAnnotations;

namespace IDAAI_APP.Models.Operations
{
    public class PrestamoModuloQuery : PaginacionQuery
    {
        [StringLength(maximumLength: 50)]
        public string Modulo { get; set; }
    }

    public class PrestamoEstudianteQuery : PaginacionQuery
    {
        [StringLength(maximumLength: 10)]
        public string Matricula { get; set; }
    }
}
