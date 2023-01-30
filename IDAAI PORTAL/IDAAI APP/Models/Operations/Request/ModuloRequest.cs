using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace IDAAI_APP.Models.Operations.Request
{
    public class ModuloRegistrarRequest
    {
        [Required]
        [JsonProperty("Nombre")]        
        [StringLength(maximumLength: 50)]
        public string Nombre { get; set; }

        [JsonProperty("Descripcion")]
        [StringLength(maximumLength: 100)]
        public string Descripcion { get; set; }

        [JsonProperty("PeriodoAcademico")]
        [StringLength(maximumLength: 50)]
        public string PeriodoAcademico { get; set; }

        [JsonProperty("DiasClase")]
        [StringLength(maximumLength: 10)]
        public string DiasClase { get; set; }

        [Required]
        [JsonProperty("Usuario")]
        [StringLength(maximumLength: 25)]       
        public string Usuario { get; set; }
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

        [JsonProperty("DiasClase")]
        [StringLength(maximumLength: 10)]
        public string DiasClase { get; set; }

        [Required]
        [JsonProperty("Usuario")]
        [StringLength(maximumLength: 25)]
        public string Usuario { get; set; }
    }
}
