using System.ComponentModel.DataAnnotations;

namespace IDAAI_APP.Models.Operations
{
    public class InventarioQuery : PaginacionQuery
    {
        [StringLength(maximumLength: 25)]
        public string Nombre { get; set; }

        [StringLength(maximumLength: 25)]
        public string Usuario { get; set; }
    }
}
