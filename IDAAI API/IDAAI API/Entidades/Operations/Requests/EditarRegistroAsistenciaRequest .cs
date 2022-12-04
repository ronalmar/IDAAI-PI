using System.ComponentModel.DataAnnotations;

namespace IDAAI_API.Entidades.Operations.Requests
{
    public class EditarRegistroAsistenciaRequest
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public bool EsAsistencia { get; set; }
    }
}
