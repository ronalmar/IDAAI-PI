namespace IDAAI_APP.Models.Operations
{
    public class PaginacionQuery
    {
        public int pagina = 1;
        private readonly int cantidadMinimaDePaginas = 1;
        private int recordsPorPagina = 10;
        private readonly int cantidadMaximaPorPagina = 1000000;
        private readonly int cantidadMinimaPorPagina = 1;
        
        public int RecordsPorPagina
        {
            get
            {
                return recordsPorPagina;
            }
            set
            {
                recordsPorPagina = value > cantidadMaximaPorPagina ? cantidadMaximaPorPagina : value < cantidadMinimaPorPagina ? cantidadMinimaPorPagina : value;
            }
        }

        public int Pagina
        {
            get
            {
                return pagina;
            }
            set
            {
                pagina = value < cantidadMinimaDePaginas ? cantidadMinimaDePaginas : value;
            }
        }
    }
}
