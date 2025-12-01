using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace InclusaoDiversidadeEmpresas.Models
{
    public class ParticipacaoEmTreinamentoModel
    {
        public long Id { get; set; }

        public long ColaboradorId { get; set; }
        [JsonIgnore]
        public Colaborador? Colaborador { get; set; }

        public long TreinamentoId { get; set; }

        [JsonIgnore]
        public TreinamentoModel? Treinamento { get; set; }

        public bool Completo { get; set; }

        public DateTime? DataDeConclusao { get; set; }
    }
}

