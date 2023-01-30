using System.ComponentModel.DataAnnotations;

namespace IDAAI_API.Entidades.Operations.Requests
{
    public class EditarItemRequest
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
