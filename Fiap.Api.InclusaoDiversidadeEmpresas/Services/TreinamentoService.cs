using InclusaoDiversidadeEmpresas.Data;
using InclusaoDiversidadeEmpresas.Models;
using Microsoft.EntityFrameworkCore;

namespace Fiap.Api.InclusaoDiversidadeEmpresas.Services
{
    public class TreinamentoService : ITreinamentoService
    {
        private readonly DatabaseContext _databaseContext;
        public TreinamentoService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }
        public async Task<TreinamentoModel?> AtualizarTreinamento(TreinamentoModel treinamento)
        {
            _databaseContext.Entry(treinamento).State = EntityState.Modified;

            try
            {
                await _databaseContext.SaveChangesAsync();
                return treinamento;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _databaseContext.Treinamentos.AnyAsync(e => e.Id == treinamento.Id))
                {
                    return null; 
                }
                throw;
            }
        }

        public async Task<TreinamentoModel> CriarTreinamento(TreinamentoModel treinamento)
        {
            _databaseContext.Treinamentos.Add(treinamento);
            await _databaseContext.SaveChangesAsync();
            return treinamento;
        }


        public async Task<bool> DeletarTreinamento(long id)
        {
            var treinamento = await _databaseContext.Treinamentos.FindAsync(id);


            if (treinamento == null)
                return false;

            // remover participações primeiro
            var participacoes = _databaseContext.ParticipacoesEmTreinamento
                .Where(p => p.TreinamentoId == id);

            _databaseContext.ParticipacoesEmTreinamento.RemoveRange(participacoes);

            _databaseContext.Treinamentos.Remove(treinamento);


           
            try
            {
                await _databaseContext.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
              
                throw;
            }

            await _databaseContext.SaveChangesAsync();
            return true;

        }

        public async Task<IEnumerable<TreinamentoModel>> ListarTreinamentos()
        {
           return await _databaseContext.Treinamentos.Include(t => t.Participacao).ToListAsync();
         
        }

        public async Task<TreinamentoModel?> ObterTreinamentoPorId(long id)
        {
            return await _databaseContext.Treinamentos
                .Include(t => t.Participacao)
                .FirstOrDefaultAsync(t => t.Id == id);
        }
    }
}
