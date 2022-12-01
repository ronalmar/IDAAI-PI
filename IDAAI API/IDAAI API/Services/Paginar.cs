using IDAAI_API.Entidades.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace IDAAI_API.Services
{
    public static class Paginacion<T>
    {
        public static List<T> Paginar(List<T> resultParametro, int pagina, int recordsPorPagina)
        {
            var resultPaginado = new List<T>();
            for (var i = (pagina - 1) * recordsPorPagina; i < pagina * recordsPorPagina; i++)
            {
                if (i > resultParametro.Count - 1)
                {
                    break;
                }
                resultPaginado.Add(resultParametro[i]);
            }
            return resultPaginado;
        }
    }
}
