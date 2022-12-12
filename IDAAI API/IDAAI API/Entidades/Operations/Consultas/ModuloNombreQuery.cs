using System.ComponentModel.DataAnnotations;

namespace IDAAI_API.Entidades.Operations.Consultas
{
    public class ModuloNombreQuery: PaginacionQuery
    {
        [Required]
        [StringLength(maximumLength: 50)]
        public string Nombre { get; set; }
    }
}
