// 📁 Services/TreinamentoService.cs

using Fiap.Api.InclusaoDiversidadeEmpresas.Models;
using Fiap.Api.InclusaoDiversidadeEmpresas.ViewModels; // Necessário para PagedResultViewModel
using InclusaoDiversidadeEmpresas.Data;
using InclusaoDiversidadeEmpresas.Models;
using InclusaoDiversidadeEmpresas.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Fiap.Api.InclusaoDiversidadeEmpresas.Services
{
    public class TreinamentoService : ITreinamentoService
    {
        private readonly DatabaseContext _databaseContext;

        public TreinamentoService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        // CREATE
        public async Task<TreinamentoModel> AddTreinamento(TreinamentoModel treinamento)
        {
            _databaseContext.Treinamentos.Add(treinamento);
            await _databaseContext.SaveChangesAsync();
            return treinamento;
        }

        // READ (LISTAR TODOS) - IMPLEMENTAÇÃO CORRIGIDA PARA PAGINAÇÃO
        public async Task<PagedResultViewModel<TreinamentoModel>> GetAllTreinamentos(QueryParameters parameters)
        {
            // 1. Conta o total de itens
            var totalItems = await _databaseContext.Treinamentos.CountAsync();

            // 2. Define PageNumber e PageSize (Defensivo)
            int page = parameters.PageNumber < 1 ? 1 : parameters.PageNumber;
            int pageSize = parameters.PageSize;

            // 3. Aplica Paginação (Skip e Take)
            var items = await _databaseContext.Treinamentos
                .OrderBy(t => t.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // 4. Monta o pacote de resposta
            return new PagedResultViewModel<TreinamentoModel>
            {
                Items = items,
                TotalItems = totalItems,
                Page = page,
                PageSize = pageSize
            };
        }

        // READ (Por ID) - Antigo ObterTreinamentoPorId
        public async Task<TreinamentoModel?> GetTreinamentoById(long id)
        {
            return await _databaseContext.Treinamentos
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        // UPDATE - Antigo AtualizarTreinamento
        public async Task<TreinamentoModel?> UpdateTreinamento(long id, TreinamentoModel model)
        {
            var existente = await _databaseContext.Treinamentos.FindAsync(id);

            if (existente == null)
                return null;

            // Atualiza os campos permitidos
            existente.Titulo = model.Titulo;
            existente.Descricao = model.Descricao;
            existente.Data = model.Data;

            await _databaseContext.SaveChangesAsync();

            return existente;
        }

        // DELETE - Antigo DeletarTreinamento
        public async Task<bool> DeleteTreinamento(long id)
        {
            var treinamento = await _databaseContext.Treinamentos.FindAsync(id);

            if (treinamento == null)
                return false;

            var participacoes = _databaseContext.ParticipacoesEmTreinamento
                .Where(p => p.TreinamentoId == id);

            _databaseContext.ParticipacoesEmTreinamento.RemoveRange(participacoes);
            _databaseContext.Treinamentos.Remove(treinamento);

            await _databaseContext.SaveChangesAsync();
            return true;
        }
    }
}