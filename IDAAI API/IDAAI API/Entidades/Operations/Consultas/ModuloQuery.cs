using System.ComponentModel.DataAnnotations;

namespace IDAAI_API.Entidades.Operations.Consultas
{
    public class ModuloQuery: PaginacionQuery
    {
        [Required]
        [StringLength(maximumLength: 50)]
        public string Modulo { get; set; }
    }
}
