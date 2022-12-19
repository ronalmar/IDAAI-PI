using System.ComponentModel.DataAnnotations;

namespace IDAAI_API.Entidades.Operations.Consultas
{
    public class ModuloPeriodoQuery : PaginacionQuery
    {
        [Required]
        [StringLength(maximumLength: 50)]
        public string PeriodoAcademico { get; set; }
    }
}
