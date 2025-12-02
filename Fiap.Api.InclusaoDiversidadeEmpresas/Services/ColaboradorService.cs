

using InclusaoDiversidadeEmpresas.Data;
using Fiap.Api.InclusaoDiversidadeEmpresas.Models;
using Fiap.Api.InclusaoDiversidadeEmpresas.ViewModels; // Para PagedResultViewModel
using InclusaoDiversidadeEmpresas.Models;
using InclusaoDiversidadeEmpresas.ViewModels; // Para ColaboradorListaViewModel
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fiap.Api.InclusaoDiversidadeEmpresas.Services
{
    public class ColaboradorService : IColaboradorService
    {
        private readonly DatabaseContext _context;

        // Injeção de Dependência do DatabaseContext
        public ColaboradorService(DatabaseContext context)
        {
            _context = context;
        }

        // CREATE
        public async Task<Colaborador> AddColaborador(Colaborador colaborador)
        {
            _context.Colaboradores.Add(colaborador);
            await _context.SaveChangesAsync();
            return colaborador;
        }

        // READ (Listar Todos) - Implementação da Paginação (QueryParameters)
        public async Task<PagedResultViewModel<ColaboradorListaViewModel>> GetAllColaboradores(QueryParameters parameters)
        {
            // 1. Conta o total de itens para calcular as páginas
            var totalItems = await _context.Colaboradores.CountAsync();

            // 2. Define PageNumber e PageSize (Defensivo, baseado em QueryParameters)
            int page = parameters.PageNumber < 1 ? 1 : parameters.PageNumber;
            int pageSize = parameters.PageSize;

            // 3. Aplica Paginação (Skip e Take) e mapeamento (Select)
            var items = await _context.Colaboradores
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new ColaboradorListaViewModel
                {
                    Id = c.Id,
                    Nome = c.NomeColaborador,
                    Departamento = c.Departamento,
                    Email = c.Email,
                    // CAMPOS DE DIVERSIDADE MANTIDOS E NECESSÁRIOS PARA O RELATÓRIO:
                    GeneroColaborador = c.GeneroColaborador,
                    EtniaColaborador = c.EtniaColaborador,
                    TemDisabilidade = c.TemDisabilidade
                })
                .ToListAsync();

            // 4. Monta o pacote de resposta PagedResultViewModel
            return new PagedResultViewModel<ColaboradorListaViewModel>
            {
                Items = items,
                TotalItems = totalItems,
                Page = page,
                PageSize = pageSize
            };
        }

        // READ (Por ID)
        public async Task<Colaborador?> GetColaboradorById(long id)
        {
            return await _context.Colaboradores.FindAsync(id);
        }

        // UPDATE
        public async Task<Colaborador?> UpdateColaborador(Colaborador colaborador)
        {
            _context.Entry(colaborador).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return colaborador;
            }
            catch (DbUpdateConcurrencyException)
            {
                // Se o item não existir, retorna null
                if (!await _context.Colaboradores.AnyAsync(e => e.Id == colaborador.Id))
                {
                    return null;
                }
                throw;
            }
        }

        // DELETE
        public async Task<bool> DeleteColaborador(long id)
        {
            var colaborador = await _context.Colaboradores.FindAsync(id);
            if (colaborador == null)
            {
                return false;
            }

            _context.Colaboradores.Remove(colaborador);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}