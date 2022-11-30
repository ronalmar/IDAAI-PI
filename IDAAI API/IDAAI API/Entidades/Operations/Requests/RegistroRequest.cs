using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace IDAAI_API.Entidades.Operations.Requests
{
    public class RegistroRequest
    {
        [JsonProperty("Usuario")]
        [Required]
        [StringLength(maximumLength: 25)]
        public string Usuario { get; set; }
        [JsonProperty("Password")]
        [Required]
        [StringLength(maximumLength: 25)]
        public string Password { get; set; }
        [JsonProperty("Email")]
        [EmailAddress]
        [StringLength(maximumLength: 50)]
        public string Email { get; set; }

    }
}
