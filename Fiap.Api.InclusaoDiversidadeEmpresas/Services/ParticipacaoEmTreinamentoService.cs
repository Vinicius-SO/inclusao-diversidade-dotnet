using InclusaoDiversidadeEmpresas.Data;
using InclusaoDiversidadeEmpresas.Models;
using Microsoft.EntityFrameworkCore;

namespace Fiap.Api.InclusaoDiversidadeEmpresas.Services
{
    public class ParticipacaoEmTreinamentoService : IParticipacaoEmTreinamentoService
    {
        private readonly DatabaseContext _databaseContext;

        public ParticipacaoEmTreinamentoService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<ParticipacaoEmTreinamentoModel?> AtualizarParticipacaoEmTreinamentoService(long id, ParticipacaoEmTreinamentoModel model)
        {
            var existente = await _databaseContext.ParticipacoesEmTreinamento.FindAsync(id);

            if (existente == null)
                return null;

            existente.ColaboradorId = model.ColaboradorId;
            existente.TreinamentoId = model.TreinamentoId;
            existente.Completo = model.Completo;
            existente.DataDeConclusao = model.DataDeConclusao;

            await _databaseContext.SaveChangesAsync();
            return existente;
        }

        public async Task<ParticipacaoEmTreinamentoModel> CriarParticipacaoEmTreinamentoService(ParticipacaoEmTreinamentoModel participacaoEmTreinamento)
        {
            _databaseContext.ParticipacoesEmTreinamento.Add(participacaoEmTreinamento);
            await _databaseContext.SaveChangesAsync();
            return participacaoEmTreinamento;
        }

        public async Task<bool> DeletarParticipacaoEmTreinamentoService(long id)
        {
            var participacaoEmTreinamento = await _databaseContext.ParticipacoesEmTreinamento.FindAsync(id);
            if (participacaoEmTreinamento == null)
            {
                return false;
            }

            _databaseContext.ParticipacoesEmTreinamento.Remove(participacaoEmTreinamento);
            await _databaseContext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<ParticipacaoEmTreinamentoModel>> ListarParticipacaoPaginado(int pagina, int tamanho)
        {
            return await _databaseContext.ParticipacoesEmTreinamento
                .Include(p => p.Colaborador)
                .Include(p => p.Treinamento)
                .OrderBy(p => p.Id)
                .Skip((pagina - 1) * tamanho)
                .Take(tamanho)
                .ToListAsync();
        }

        public async Task<ParticipacaoEmTreinamentoModel?> ObterParticipacaoEmTreinamentoServicePorId(long id)
        {
            return await _databaseContext.ParticipacoesEmTreinamento
                .Include(p => p.Colaborador)
                .Include(p => p.Treinamento)
                .FirstOrDefaultAsync(t => t.Id == id);
        }
    }
}
