using System;
using InclusaoDiversidadeEmpresas.Models;

namespace InclusaoDiversidadeEmpresas.ViewModels
{
    public class DashboardDiversidadeViewModel
    {
        public DashboardDiversidadeViewModel() { }
        public DashboardDiversidadeViewModel(RelatorioDeDiversidadeModel relatorio)
        {
            DataGerada = relatorio.DataGerada;
            TotalColaborador = relatorio.TotalColaborador;

            // Mapeia os dados brutos
            QtdMulheres = relatorio.ContagemDeMulheres;
            QtdNegros = relatorio.ContagemDePessoasNegras;
            QtdLgbt = relatorio.ContagemDePessoasLgbt;
            QtdPcd = relatorio.ContagemDePessoasComDesabilidade;
        }

        public DateTime DataGerada { get; set; }
        public int TotalColaborador { get; set; }

        // Quantidades Absolutas
        public int QtdMulheres { get; set; }
        public int QtdNegros { get; set; }
        public int QtdLgbt { get; set; }
        public int QtdPcd { get; set; }

        // Porcentagens Calculadas
        public double PorcentagemMulheres => CalcularPorcentagem(QtdMulheres);
        public double PorcentagemNegros => CalcularPorcentagem(QtdNegros);
        public double PorcentagemLgbt => CalcularPorcentagem(QtdLgbt);
        public double PorcentagemPcd => CalcularPorcentagem(QtdPcd);

        private double CalcularPorcentagem(int valor)
        {
            if (TotalColaborador == 0) return 0;
            return Math.Round((double)valor / TotalColaborador * 100, 2);
        }
    }
}