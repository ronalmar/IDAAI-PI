using System.ComponentModel.DataAnnotations;

namespace IDAAI_APP.Models.Operations.Request
{
    public class AsistenciaRegistrarRequest
    {
        [Required]
        [StringLength(maximumLength: 10)]
        public string Matricula { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        public bool EsAsistencia { get; set; }

        [Required]
        [StringLength(maximumLength: 50)]
        public string Modulo { get; set; }
    }

    public class AsistenciaEditarRequest
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public bool EsAsistencia { get; set; }
    }
}
