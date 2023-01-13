using System.ComponentModel.DataAnnotations;

namespace IDAAI_APP.Models.Operations
{
    public class AsistenciaQuery : PaginacionQuery
    {       
        [StringLength(maximumLength: 100)]
        public string Nombres { get; set; }
        
        [StringLength(maximumLength: 100)]
        public string Apellidos { get; set; }
        [StringLength(maximumLength: 10)]
        public string Matricula { get; set; }

        [StringLength(maximumLength: 100)]
        public string Carrera { get; set; }

        [StringLength(maximumLength: 50)]
        public string Modulo { get; set; }
    }
}
