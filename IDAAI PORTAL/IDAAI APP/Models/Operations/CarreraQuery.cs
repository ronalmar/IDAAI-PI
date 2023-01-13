using System.ComponentModel.DataAnnotations;

namespace IDAAI_APP.Models.Operations
{
    public class CarreraQuery : PaginacionQuery
    {
        [StringLength(maximumLength: 100)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(maximumLength: 50)]
        public string Modulo { get; set; }
    }
}
