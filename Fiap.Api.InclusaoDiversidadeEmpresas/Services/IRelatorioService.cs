using InclusaoDiversidadeEmpresas.Models;
using System.Threading.Tasks;

namespace InclusaoDiversidadeEmpresas.Services
{
    public interface IRelatorioService
    {
        // Retorna o RelatorioDeDiversidadeModel preenchido
        Task<RelatorioDeDiversidadeModel> GerarRelatorioAsync();
    }
}