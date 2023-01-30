using System.ComponentModel.DataAnnotations;

namespace IDAAI_API.Entidades.Operations.Consultas
{
    public class ModuloPeriodoQuery : PaginacionQuery
    {
        [Required]
        [StringLength(maximumLength: 50)]
        public string PeriodoAcademico { get; set; }

        [Required]
        [StringLength(maximumLength: 25)]
        public string Usuario { get; set; }
    }

    public class ModuloUsuarioQuery : PaginacionQuery
    {
        [Required]
        [StringLength(maximumLength: 25)]
        public string Usuario { get; set; }
    }
}
