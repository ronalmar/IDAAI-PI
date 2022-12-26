using IDAAI_API.Entidades.Operations.Consultas;
using System.ComponentModel.DataAnnotations;

namespace IDAAI_API.Entidades.Operations.Requests
{
    public class RegistrarGrupoPrestamoModuloRequest: PaginacionQuery
    {
        [Required]
        //[StringLength(maximumLength: 100)]
        public List<string> Rfids { get; set; }

        [Required]
        [StringLength(maximumLength: 50)]
        public string Modulo { get; set; }
    }
}
