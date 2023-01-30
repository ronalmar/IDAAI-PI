using System.ComponentModel.DataAnnotations;

namespace IDAAI_API.Entidades.Operations.Consultas
{
    public class ItemQuery : PaginacionQuery
    {
        [StringLength(maximumLength: 100)]
        public string Rfid { get; set; }

        [StringLength(maximumLength: 100)]
        public string Inventario { get; set; }

        [StringLength(maximumLength: 25)]
        public string Usuario { get; set; }
    }
}
