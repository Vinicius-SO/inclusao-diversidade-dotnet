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

        public async Task<ParticipacaoEmTreinamentoModel?> AtualizarParticipacaoEmTreinamentoService(ParticipacaoEmTreinamentoModel participacaoEmTreinamento)
        {
            _databaseContext.ParticipacoesEmTreinamento.Update(participacaoEmTreinamento);

            try
            {
                await _databaseContext.SaveChangesAsync();
                return participacaoEmTreinamento;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _databaseContext.ParticipacoesEmTreinamento.AnyAsync(e => e.Id == participacaoEmTreinamento.Id))
                {
                    return null;
                }
                throw;
            }
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

        public async  Task<IEnumerable<ParticipacaoEmTreinamentoModel>> ListarParticipacaoEmTreinamentoService()
        {
            return await _databaseContext.ParticipacoesEmTreinamento
                .Include(p => p.Colaborador)
                .Include(p => p.Treinamento)
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
