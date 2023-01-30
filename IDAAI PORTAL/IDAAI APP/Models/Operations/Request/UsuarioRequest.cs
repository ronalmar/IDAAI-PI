using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace IDAAI_APP.Models.Operations.Request
{
    public class UsuarioRequest
    {
        [JsonProperty("Id")]
        [Required]
        public int Id { get; set; }

        [JsonProperty("Usuario")]
        [StringLength(maximumLength: 25)]
        public string Usuario { get; set; }

        [JsonProperty("PasswordAnterior")]
        [StringLength(maximumLength: 25)]
        public string PasswordAnterior { get; set; }

        [JsonProperty("Password")]
        [StringLength(maximumLength: 25)]
        public string Password { get; set; }

        [JsonProperty("Email")]
        [EmailAddress]
        [StringLength(maximumLength: 50)]
        public string Email { get; set; }
    }

    public class EditarUsuarioModuloActualRequest
    {
        [JsonProperty("Usuario")]
        [StringLength(maximumLength: 25)]
        public string Usuario { get; set; }

        [JsonProperty("Modulo")]
        [StringLength(maximumLength: 50)]
        public string Modulo { get; set; }
    }

    public class EditarFechaUsuarioRequest
    {
        [JsonProperty("Usuario")]
        [StringLength(maximumLength: 25)]
        public string Usuario { get; set; }
    }
}
