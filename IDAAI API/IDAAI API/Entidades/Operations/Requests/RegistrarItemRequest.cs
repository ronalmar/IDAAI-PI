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

    public class GrupoItemsDetectadosRfidRequest
    {
        [Required]
        public List<_Rfid> Rfids { get; set; }
    }

    public class _Rfid
    {
        [Required]
        [StringLength(maximumLength: 100)]
        public string Rfid { get; set; }
    }
    public class GrupoItemsDetectadosZebraRequest
    {
        [Required]
        public List<ZebraRequest> ListaDatos { get; set; }
    }

    public class ZebraRequest
    {
        public DataZebra data { get; set; }
        public DateTime timestamp { get; set; }
        public string type { get; set; }
    }

    public class DataZebra {
        public decimal channel { get; set; }
        public int eventNum { get; set; }
        public string format { get; set; }
        public string idHex { get; set; }
    }

}
