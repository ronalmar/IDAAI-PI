using System.ComponentModel.DataAnnotations;

namespace IDAAI_API.Entidades.Operations.Requests
{
    public class DevolverPrestamoRequest
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(maximumLength: 25)]
        public string Usuario { get; set; }

    }
}
