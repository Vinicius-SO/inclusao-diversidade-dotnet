using System;

namespace InclusaoDiversidadeEmpresas.Models
{
    public class RelatorioDeDiversidadeModel
    {
        public long Id { get; set; }

        public DateTime DataGerada { get; set; }

        public int TotalColaborador { get; set; }

        public int ContagemDeMulheres { get; set; }

        public int ContagemDePessoasNegras { get; set; }

        public int ContagemDePessoasLgbt { get; set; }

        public int ContagemDePessoasComDesabilidade { get; set; }
    }
}
