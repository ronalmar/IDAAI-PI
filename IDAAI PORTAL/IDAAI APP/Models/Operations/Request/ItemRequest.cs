using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace IDAAI_APP.Models.Operations.Request
{
    public class ItemRegistrarRequest
    {
        [Required]
        [StringLength(maximumLength: 100)]
        public string Rfid { get; set; }

        [Required]
        [StringLength(maximumLength: 100)]
        public string Inventario { get; set; }

        [Required]
        [StringLength(maximumLength: 25)]
        public string Usuario { get; set; }
    }

    public class ItemEditarRequest
    {
        [Required]
        public int Id { get; set; }

        [StringLength(maximumLength: 100)]
        public string Rfid { get; set; }

        public bool? EstaDisponible { get; set; }

        [StringLength(maximumLength: 100)]
        public string Inventario { get; set; }

        [Required]
        [StringLength(maximumLength: 25)]
        public string Usuario { get; set; }
    }
}
