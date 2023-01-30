using System.ComponentModel.DataAnnotations;

namespace IDAAI_API.Entidades.Operations.Consultas
{
    public class InventarioNombreQuery : PaginacionQuery
    {
        [StringLength(maximumLength: 25)]
        public string Nombre { get; set; }


        [StringLength(maximumLength: 25)]
        public string Usuario { get; set; }
    }
}
