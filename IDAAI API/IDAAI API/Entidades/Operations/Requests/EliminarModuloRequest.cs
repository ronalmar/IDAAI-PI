using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace IDAAI_API.Entidades.Operations.Requests
{
    public class EliminarModuloRequest
    {
        [JsonProperty("Id")]
        [Required]
        public int Id { get; set; }

    }
}
