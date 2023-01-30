using System.ComponentModel.DataAnnotations;

namespace IDAAI_API.Entidades.Operations.Requests
{
    public class RegistrarPrestamoEstudianteRequest
    {
        [Required]
        [StringLength(maximumLength: 100)]
        public string Rfid { get; set; }

        [Required]
        [StringLength(maximumLength: 10)]
        public string Matricula { get; set; }

        [Required]
        [StringLength(maximumLength: 25)]
        public string Usuario { get; set; }
    }
}
