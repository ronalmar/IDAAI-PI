using IDAAI_API.Entidades.Operations.Consultas;
using System.ComponentModel.DataAnnotations;

namespace IDAAI_API.Entidades.Operations.Requests
{
    public class RegistrarItemRequest
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

    public class GrupoItemsDetectadosRequest
    {
        [Required]
        public List<string> Rfids { get; set; }
    }
}
