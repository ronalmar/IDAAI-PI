using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace IDAAI_APP.Models.Operations.Request
{
    public class ModuloRegistrarRequest
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

    public class ModuloEditarRequest
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
