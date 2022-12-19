using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace IDAAI_API.Entidades.Operations.Requests
{
    public class RegistrarModuloRequest
    {
        [JsonProperty("Nombre")]
        [Required]
        [StringLength(maximumLength: 50)]
        public string Nombre { get; set; }

        [JsonProperty("Descripcion")]        
        [StringLength(maximumLength: 100)]
        public string Descripcion { get; set; }

        [JsonProperty("PeriodoAcademico")]
        [StringLength(maximumLength: 50)]
        public string PeriodoAcademico { get; set; }

    }
}
