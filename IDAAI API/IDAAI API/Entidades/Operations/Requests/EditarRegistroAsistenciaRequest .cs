using System.ComponentModel.DataAnnotations;

namespace IDAAI_API.Entidades.Operations.Requests
{
    public class EditarRegistroAsistenciaRequest
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public bool EsAsistencia { get; set; }

        [Required]
        [StringLength(maximumLength: 25)]
        public string Usuario { get; set; }
    }

    public class EstablecerAsistenciaRequest
    {

        [Required]
        [StringLength(maximumLength: 10)]
        public string Matricula { get; set; }
    }
}
