using System.ComponentModel.DataAnnotations;

namespace IDAAI_API.Entidades.Operations.Consultas
{
    public class InventarioQuery : PaginacionQuery
    {
        [StringLength(maximumLength: 25)]
        public string Nombre { get; set; }
    }
}
