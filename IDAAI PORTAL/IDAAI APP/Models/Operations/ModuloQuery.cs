using System.ComponentModel.DataAnnotations;

namespace IDAAI_APP.Models.Operations
{
    public class ModuloQuery : PaginacionQuery
    {
        [StringLength(maximumLength: 50)]
        public string Nombre { get; set; }

        [StringLength(maximumLength: 50)]
        public string PeriodoAcademico { get; set; }
    }
}
