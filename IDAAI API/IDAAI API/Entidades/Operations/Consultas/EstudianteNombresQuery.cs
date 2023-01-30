using System.ComponentModel.DataAnnotations;
using IDAAI_API.Entidades.Operations.Consultas;

namespace IDAAI_API.Entidades.Operations.Estudiante
{
    public class EstudianteNombresQuery: EstudianteModuloQuery
    {       
        [StringLength(maximumLength: 100)]
        public string Nombres { get; set; }
        
        [StringLength(maximumLength: 100)]
        public string Apellidos { get; set; }

        [StringLength(maximumLength: 25)]
        public string Usuario { get; set; }
    }
}
