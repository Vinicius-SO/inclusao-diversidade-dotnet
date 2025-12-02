

using Fiap.Api.InclusaoDiversidadeEmpresas.Models;
using Fiap.Api.InclusaoDiversidadeEmpresas.ViewModels;
using InclusaoDiversidadeEmpresas.Models;
using InclusaoDiversidadeEmpresas.ViewModels;
using System.Threading.Tasks;


public interface ITreinamentoService
{
    // READ (LISTAR TODOS) 
    Task<PagedResultViewModel<TreinamentoModel>> GetAllTreinamentos(QueryParameters parameters);
    Task<TreinamentoModel?> GetTreinamentoById(long id);
    Task<TreinamentoModel> AddTreinamento(TreinamentoModel treinamento);
    Task<TreinamentoModel?> UpdateTreinamento(long id, TreinamentoModel treinamento);
    Task<bool> DeleteTreinamento(long id);
}
