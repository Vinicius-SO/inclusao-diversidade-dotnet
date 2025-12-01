using InclusaoDiversidadeEmpresas.Data;
using InclusaoDiversidadeEmpresas.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InclusaoDiversidadeEmpresas.Services
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

        // READ (Listar Todos)
        public async Task<IEnumerable<Colaborador>> GetAllColaboradores()
        {
            return await _context.Colaboradores.ToListAsync();
        }

        // READ (Por ID)
        public async Task<Colaborador?> GetColaboradorById(long id)
        {
            return await _context.Colaboradores.FindAsync(id);
        }

        // UPDATE (Lógica simplificada)
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
                // Se a entidade não existir (concorrência), retorna null
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
