using InclusaoDiversidadeEmpresas.Models;

namespace Fiap.Api.InclusaoDiversidadeEmpresas.ViewModel
{
    public class TreinamentoPaginacaoViewModel
    {
        public IEnumerable<TreinamentoModel> Treinamentos { get; set; }
        public int PaginaAtual { get; set; }
        public int TamanhoPagina { get; set; }

        // Indicam se existem páginas anterior/próxima
        public bool TemPaginaAnterior => PaginaAtual > 1;
        public bool TemProximaPagina => Treinamentos.Count() == TamanhoPagina;

        // URLs sugeridas (opcional, útil para front-end)
        public string UrlPaginaAnterior =>
            TemPaginaAnterior ? $"/api/treinamento?pagina={PaginaAtual - 1}&tamanho={TamanhoPagina}" : "";

        public string UrlProximaPagina =>
            TemProximaPagina ? $"/api/treinamento?pagina={PaginaAtual + 1}&tamanho={TamanhoPagina}" : "";
    }
}
