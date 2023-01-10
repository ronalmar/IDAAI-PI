using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace IDAAI_API.Entidades.Operations.Requests
{
    public class EliminarRequest
    {
        [JsonProperty("Id")]
        public int Id { get; set; }

    }
}
