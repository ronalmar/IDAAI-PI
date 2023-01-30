using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace IDAAI_APP.Models.Operations.Request
{
    public class PrestamoModuloRegistrarRequest
    {
        [Required]
        [StringLength(maximumLength: 100)]
        public string Rfid { get; set; }

        [Required]
        [StringLength(maximumLength: 50)]
        public string Modulo { get; set; }

        [Required]
        [StringLength(maximumLength: 25)]
        public string Usuario { get; set; }
    }

    public class PrestamoGrupoModuloRegistrarRequest
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

    public class PrestamoEstudianteRegistrarRequest
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

    public class PrestamoEditarRequest
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(maximumLength: 25)]
        public string Usuario { get; set; }
    }
}
