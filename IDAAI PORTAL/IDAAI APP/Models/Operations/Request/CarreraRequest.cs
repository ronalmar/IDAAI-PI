using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace IDAAI_APP.Models.Operations.Request
{
    public class CarreraRegistrarRequest
    {
        [JsonProperty("Nombre")]
        [Required]
        [StringLength(maximumLength: 100)]
        public string Nombre { get; set; }

        [JsonProperty("Modulo")]
        [Required]
        [StringLength(maximumLength: 50)]
        public string Modulo { get; set; }

        [Required]
        [JsonProperty("Usuario")]
        [StringLength(maximumLength: 25)]
        public string Usuario { get; set; }
    }

    public class CarreraEditarRequest
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

        [Required]
        [JsonProperty("Usuario")]
        [StringLength(maximumLength: 25)]
        public string Usuario { get; set; }
    }
}
