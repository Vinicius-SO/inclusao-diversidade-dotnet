using InclusaoDiversidadeEmpresas.Models;

namespace Fiap.Api.InclusaoDiversidadeEmpresas.ViewModel
{
    public class ParticipacaoPaginacaoViewModel
    {
        public IEnumerable<ParticipacaoEmTreinamentoModel> Participacoes { get; set; }
        public int PaginaAtual { get; set; }
        public int TamanhoPagina { get; set; }

        public bool TemPaginaAnterior => PaginaAtual > 1;
        public bool TemProximaPagina => Participacoes.Count() == TamanhoPagina;

        public string UrlPaginaAnterior =>
            TemPaginaAnterior ? $"/api/participacao?pagina={PaginaAtual - 1}&tamanho={TamanhoPagina}" : "";

        public string UrlProximaPagina =>
            TemProximaPagina ? $"/api/participacao?pagina={PaginaAtual + 1}&tamanho={TamanhoPagina}" : "";
    }
}
