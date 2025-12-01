// Adicionado o using para o seu serviço de Colaborador (se estiver em outro namespace)
using Fiap.Api.InclusaoDiversidadeEmpresas.Services;
using InclusaoDiversidadeEmpresas.Models;
using InclusaoDiversidadeEmpresas.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
// Se o seu IColaboradorService estiver no mesmo namespace do RelatorioService,
// este using pode não ser necessário, mas o mantemos por segurança.


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
            // 1. Obtém a lista completa de colaboradores (necessário para todas as contagens)
            var colaboradores = await _colaboradorService.GetAllColaboradores();
            var listaColaboradores = colaboradores.ToList();

            // 2. Contagens
            var totalColaborador = listaColaboradores.Count;

            var contagemDeMulheres = listaColaboradores
                // Filtra pelo valor 'Feminino' no campo GeneroColaborador
                .Count(c => c.GeneroColaborador.Equals("Feminino", StringComparison.OrdinalIgnoreCase));

            var contagemDePessoasNegras = listaColaboradores
                // Filtra por 'Preta' ou 'Parda' no campo EtniaColaborador
                .Count(c => c.EtniaColaborador.Equals("Preta", StringComparison.OrdinalIgnoreCase) ||
                            c.EtniaColaborador.Equals("Parda", StringComparison.OrdinalIgnoreCase));

            var contagemDePessoasComDesabilidade = listaColaboradores
                // Filtra pelo campo booleano TemDisabilidade
                .Count(c => c.TemDisabilidade);

            // 3. Contagem de Pessoas LGBTQIA+ (Baseado na exclusão de gêneros binários)
            // ASSUNÇÃO: Se o GenereColaborador for 'Não Binário', 'Outro', etc., será contado aqui.
            var contagemDePessoasLgbt = listaColaboradores
                .Count(c => !c.GeneroColaborador.Equals("Masculino", StringComparison.OrdinalIgnoreCase) &&
                            !c.GeneroColaborador.Equals("Feminino", StringComparison.OrdinalIgnoreCase));

            // 4. Criação e Preenchimento do Model de Relatório
            var relatorio = new RelatorioDeDiversidadeModel
            {
                // Note: O Id aqui é apenas um valor de retorno, pois o relatório não é persistido no banco
                Id = 1,
                DataGerada = DateTime.Now,
                TotalColaborador = totalColaborador,

                ContagemDeMulheres = contagemDeMulheres,
                ContagemDePessoasNegras = contagemDePessoasNegras,

                // Campo adicionado e corrigido na lógica
                ContagemDePessoasLgbt = contagemDePessoasLgbt,

                ContagemDePessoasComDesabilidade = contagemDePessoasComDesabilidade,
            };

            return relatorio;
        }
    }
}