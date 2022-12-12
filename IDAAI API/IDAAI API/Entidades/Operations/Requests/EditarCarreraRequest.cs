using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace IDAAI_API.Entidades.Operations.Requests
{
    public class EditarCarreraRequest
    {
        [JsonProperty("Id")]
        [Required]
        public int Id { get; set; }

        [JsonProperty("Nombre")]
        [StringLength(maximumLength: 100)]
        public string Nombre { get; set; }

        [JsonProperty("Modulo")]
        [StringLength(maximumLength: 50)]
        public string Modulo { get; set; }

    }
}
