

using Fiap.Api.InclusaoDiversidadeEmpresas.Models;
using InclusaoDiversidadeEmpresas.Models;
using InclusaoDiversidadeEmpresas.ViewModels;
using Fiap.Api.InclusaoDiversidadeEmpresas.ViewModels;
using System.Threading.Tasks; 

namespace Fiap.Api.InclusaoDiversidadeEmpresas.Services
{
    public interface IColaboradorService
    {
        Task<Colaborador> AddColaborador(Colaborador colaborador);

       
        Task<PagedResultViewModel<ColaboradorListaViewModel>> GetAllColaboradores(QueryParameters parameters);

        Task<Colaborador?> GetColaboradorById(long id);

        Task<Colaborador?> UpdateColaborador(Colaborador colaborador);

        Task<bool> DeleteColaborador(long id);
    }
}