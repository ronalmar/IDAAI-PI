using System.ComponentModel.DataAnnotations;
using IDAAI_API.Entidades.Operations.Consultas;

namespace IDAAI_API.Entidades.Operations.Estudiante
{
    public class EstudianteQuery: PaginacionQuery
    {       
        [StringLength(maximumLength: 100)]
        public string Nombres { get; set; }
        
        [StringLength(maximumLength: 100)]
        public string Apellidos { get; set; }
        [StringLength(maximumLength: 10)]
        public string Matricula { get; set; }

        [EmailAddress]
        [StringLength(maximumLength: 50)]
        public string Email { get; set; }

        [StringLength(maximumLength: 100)]
        public string Carrera { get; set; }

        [StringLength(maximumLength: 50)]
        public string Modulo { get; set; }
    }
}
