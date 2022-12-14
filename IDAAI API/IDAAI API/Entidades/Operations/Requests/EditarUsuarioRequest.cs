using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace IDAAI_API.Entidades.Operations.Requests
{
    public class EditarUsuarioRequest
    {
        [JsonProperty("Id")]
        [Required]
        public int Id { get; set; }
        [JsonProperty("Usuario")]
        [StringLength(maximumLength: 25)]
        public string Usuario { get; set; }
        [JsonProperty("Password")]
        [StringLength(maximumLength: 25)]
        public string Password { get; set; }
        [JsonProperty("Email")]
        [EmailAddress]
        [StringLength(maximumLength: 50)]
        public string Email { get; set; }

    }
}
