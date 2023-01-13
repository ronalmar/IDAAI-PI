using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace IDAAI_APP.Models.Operations.Request
{
    public class EliminarRequest
    {
        [JsonProperty("Id")]
        public int Id { get; set; }

    }
}
