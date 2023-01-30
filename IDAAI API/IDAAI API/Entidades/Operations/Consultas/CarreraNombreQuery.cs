using System.ComponentModel.DataAnnotations;

namespace IDAAI_API.Entidades.Operations.Consultas
{
    public class CarreraNombreQuery : PaginacionQuery
    {
        [StringLength(maximumLength: 100)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(maximumLength: 50)]
        public string Modulo { get; set; }

        [StringLength(maximumLength: 25)]
        public string Usuario { get; set; }
    }
}
