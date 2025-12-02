using InclusaoDiversidadeEmpresas.Models;

namespace Fiap.Api.InclusaoDiversidadeEmpresas.Services
{
    public interface IParticipacaoEmTreinamentoService
    {
        Task<IEnumerable<ParticipacaoEmTreinamentoModel>> ListarParticipacaoPaginado(int pagina, int tamanho);
        Task<ParticipacaoEmTreinamentoModel?> ObterParticipacaoEmTreinamentoServicePorId(long id);
        Task<ParticipacaoEmTreinamentoModel> CriarParticipacaoEmTreinamentoService(ParticipacaoEmTreinamentoModel participacaoEmTreinamento);
        Task<ParticipacaoEmTreinamentoModel?> AtualizarParticipacaoEmTreinamentoService(long id, ParticipacaoEmTreinamentoModel participacaoEmTreinamento);
        Task<bool> DeletarParticipacaoEmTreinamentoService(long id);
    }
}
