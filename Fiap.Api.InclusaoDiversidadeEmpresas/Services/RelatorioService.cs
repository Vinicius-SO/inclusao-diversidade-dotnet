// 📁 Services/RelatorioService.cs (Corrigindo os Usings)

using Fiap.Api.InclusaoDiversidadeEmpresas.Models;
using Fiap.Api.InclusaoDiversidadeEmpresas.Services;

// USE APENAS UM DESTES. MANTENHA O QUE CONTÉM QueryParameters e PagedResultViewModel
using Fiap.Api.InclusaoDiversidadeEmpresas.ViewModels;
// Se este using existe, REMOVA-O: using InclusaoDiversidadeEmpresas.ViewModels; 

using InclusaoDiversidadeEmpresas.Models;
using InclusaoDiversidadeEmpresas.Services;

using System;
using System.Linq;
using System.Threading.Tasks;

// ... (Resto do código do RelatorioService.cs)

// Se o seu namespace principal for Fiap.Api.InclusaoDiversidadeEmpresas.Services, mantenha este.
namespace Fiap.Api.InclusaoDiversidadeEmpresas.Services
{
    public class RelatorioService : IRelatorioService
    {
        private readonly IColaboradorService _colaboradorService;

        public RelatorioService(IColaboradorService colaboradorService)
        {
            _colaboradorService = colaboradorService;
        }

        public async Task<RelatorioDeDiversidadeModel> GerarRelatorioAsync()
        {
            // 1. Cria o objeto QueryParameters para buscar TODOS os colaboradores
            var allRecordsQuery = new QueryParameters
            {
                PageNumber = 1,
                PageSize = int.MaxValue // Força a busca de todos os registros
            };

            // 2. Obtém o objeto de resultado paginado
            var colaboradoresVM = await _colaboradorService.GetAllColaboradores(allRecordsQuery);

            // 3. CORREÇÃO: Acessa a lista real pela propriedade 'Items' (Isto resolve o erro do .ToList())
            // O tipo de 'listaColaboradores' agora é List<ColaboradorListaViewModel>, que tem as propriedades de diversidade.
            var listaColaboradores = colaboradoresVM.Items.ToList();

            // 4. Contagens e Lógica do Relatório
            var totalColaborador = listaColaboradores.Count;

            var contagemDeMulheres = listaColaboradores
                .Count(c => c.GeneroColaborador.Equals("Feminino", StringComparison.OrdinalIgnoreCase));

            var contagemDePessoasNegras = listaColaboradores
                .Count(c => c.EtniaColaborador.Equals("Preta", StringComparison.OrdinalIgnoreCase) ||
                            c.EtniaColaborador.Equals("Parda", StringComparison.OrdinalIgnoreCase));

            var contagemDePessoasComDesabilidade = listaColaboradores
                .Count(c => c.TemDisabilidade);

            var contagemDePessoasLgbt = listaColaboradores
                .Count(c => !c.GeneroColaborador.Equals("Masculino", StringComparison.OrdinalIgnoreCase) &&
                            !c.GeneroColaborador.Equals("Feminino", StringComparison.OrdinalIgnoreCase));

            // 5. Criação e Preenchimento do Model de Relatório
            var relatorio = new RelatorioDeDiversidadeModel
            {
                Id = 1,
                DataGerada = DateTime.Now,
                TotalColaborador = totalColaborador,
                ContagemDeMulheres = contagemDeMulheres,
                ContagemDePessoasNegras = contagemDePessoasNegras,
                ContagemDePessoasLgbt = contagemDePessoasLgbt,
                ContagemDePessoasComDesabilidade = contagemDePessoasComDesabilidade,
            };

            return relatorio;
        }
    }
}