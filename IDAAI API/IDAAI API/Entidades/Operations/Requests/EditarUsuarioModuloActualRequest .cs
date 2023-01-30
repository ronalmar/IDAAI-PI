using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace IDAAI_API.Entidades.Operations.Requests
{
    public class EditarUsuarioModuloActualRequest
    {
        [JsonProperty("Usuario")]
        [StringLength(maximumLength: 25)]
        public string Usuario { get; set; }

        [JsonProperty("Modulo")]
        [StringLength(maximumLength: 50)]
        public string Modulo { get; set; }

    }
}
