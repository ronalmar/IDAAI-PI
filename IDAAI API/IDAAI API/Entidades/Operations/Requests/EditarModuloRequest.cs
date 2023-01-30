using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace IDAAI_API.Entidades.Operations.Requests
{
    public class EditarModuloRequest
    {
        [Required]
        [JsonProperty("Id")]       
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
