using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace IDAAI_API.Entidades.Operations.Requests
{
    public class EditarModuloRequest
    {
        [JsonProperty("Id")]
        [Required]
        public int Id { get; set; }

        [JsonProperty("Nombre")]
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
