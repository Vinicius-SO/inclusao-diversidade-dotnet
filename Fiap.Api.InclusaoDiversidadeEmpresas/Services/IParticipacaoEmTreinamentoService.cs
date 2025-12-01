using InclusaoDiversidadeEmpresas.Models;

namespace Fiap.Api.InclusaoDiversidadeEmpresas.Services
{
    public interface IParticipacaoEmTreinamentoService
    {
        Task<IEnumerable<ParticipacaoEmTreinamentoModel>> ListarParticipacaoEmTreinamentoService();
        Task<ParticipacaoEmTreinamentoModel?> ObterParticipacaoEmTreinamentoServicePorId(long id);
        Task<ParticipacaoEmTreinamentoModel> CriarParticipacaoEmTreinamentoService(ParticipacaoEmTreinamentoModel participacaoEmTreinamento);
        Task<ParticipacaoEmTreinamentoModel?> AtualizarParticipacaoEmTreinamentoService(ParticipacaoEmTreinamentoModel participacaoEmTreinamento);
        Task<bool> DeletarParticipacaoEmTreinamentoService(long id);
    }
}
