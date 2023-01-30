using System.ComponentModel.DataAnnotations;

namespace IDAAI_API.Entidades.Operations.Consultas
{
    public class EstudianteModuloQuery: PaginacionQuery
    {
        [Required]
        [StringLength(maximumLength: 50)]
        public string Modulo { get; set; }

        [StringLength(maximumLength: 25)]
        public string Usuario { get; set; }
    }

    public class EstudianteTodosQuery : PaginacionQuery
    {

        [StringLength(maximumLength: 25)]
        public string Usuario { get; set; }
    }
}
