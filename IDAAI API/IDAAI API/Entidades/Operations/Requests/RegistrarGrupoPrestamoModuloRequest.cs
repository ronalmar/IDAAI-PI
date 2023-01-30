using IDAAI_API.Entidades.Operations.Consultas;
using System.ComponentModel.DataAnnotations;

namespace IDAAI_API.Entidades.Operations.Requests
{
    public class RegistrarGrupoPrestamoModuloRequest: PaginacionQuery
    {
        [Required]
        public List<string> Rfids { get; set; }

        [Required]
        [StringLength(maximumLength: 50)]
        public string Modulo { get; set; }

        [Required]
        [StringLength(maximumLength: 25)]
        public string Usuario { get; set; }
    }
}
