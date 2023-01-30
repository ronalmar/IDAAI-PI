using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace IDAAI_API.Entidades.Operations.Requests
{
    public class RegistrarCarreraRequest
    {
        [JsonProperty("Nombre")]
        [Required]
        [StringLength(maximumLength: 100)]
        public string Nombre { get; set; }

        [JsonProperty("Modulo")]
        [Required]
        [StringLength(maximumLength: 50)]
        public string Modulo { get; set; }

        [JsonProperty("Usuario")]
        [Required]
        [StringLength(maximumLength: 25)]
        public string Usuario { get; set; }

    }
}
