using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace IDAAI_API.Entidades.Operations.Consultas
{
    public class LoginQuery
    {
        [Required]
        [StringLength(maximumLength: 25)]
        public string Usuario { get; set; }
        [Required]
        [StringLength(maximumLength: 25)]
        public string Password { get; set; }
    }
}
