using System.ComponentModel.DataAnnotations;

namespace IDAAI_API.Entidades.Operations.Requests
{
    public class EditarInventarioRequest
    {
        [Required]
        public int Id { get; set; }

        [StringLength(maximumLength: 100)]
        public string Nombre { get; set; }

        [StringLength(maximumLength: 300)]
        public string Descripcion { get; set; }

        public int CantidadDisponible { get; set; }

        public int CantidadTotal { get; set; }

        [Required]
        [StringLength(maximumLength: 25)]
        public string Usuario { get; set; }
    }
}
