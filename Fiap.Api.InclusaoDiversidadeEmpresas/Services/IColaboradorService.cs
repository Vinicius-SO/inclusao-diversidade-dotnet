using InclusaoDiversidadeEmpresas.Models;

namespace InclusaoDiversidadeEmpresas.Services
{
    // Interface que define os métodos que o Controller pode chamar
    public interface IColaboradorService
    {
        // CREATE
        Task<Colaborador> AddColaborador(Colaborador colaborador);

        // READ (Listar Todos)
        Task<IEnumerable<Colaborador>> GetAllColaboradores();

        // READ (Por ID)
        Task<Colaborador?> GetColaboradorById(long id);

        // UPDATE
        Task<Colaborador?> UpdateColaborador(Colaborador colaborador);

        // DELETE
        Task<bool> DeleteColaborador(long id);
    }
}
