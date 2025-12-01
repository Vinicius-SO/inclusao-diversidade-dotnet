using System;

namespace InclusaoDiversidadeEmpresas.Models
{
    public class TreinamentoModel
    {
        public long Id { get; set; }

        public string Titulo { get; set; } = string.Empty;

        public string Descricao { get; set; } = string.Empty;

        public DateTime Data { get; set; }

        // Relacionamentos
        public ICollection<ParticipacaoEmTreinamentoModel> Participacao { get; set; } = new List<ParticipacaoEmTreinamentoModel>();
    }
}

