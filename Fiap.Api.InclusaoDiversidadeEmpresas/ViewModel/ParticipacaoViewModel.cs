using System;
using System.ComponentModel.DataAnnotations;

namespace InclusaoDiversidadeEmpresas.ViewModels
{
    public class ParticipacaoViewModel
    {
        [Required]
        public long ColaboradorId { get; set; }

        [Required]
        public long TreinamentoId { get; set; }

        public bool Completo { get; set; } = false;

        public DateTime? DataDeConclusao { get; set; }
    }
}