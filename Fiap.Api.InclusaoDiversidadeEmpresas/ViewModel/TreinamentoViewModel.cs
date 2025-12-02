using System;
using System.ComponentModel.DataAnnotations;

namespace InclusaoDiversidadeEmpresas.ViewModels
{
    public class TreinamentoViewModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "O título é obrigatório")]
        public string Titulo { get; set; } = string.Empty;

        [DataType(DataType.MultilineText)]
        public string Descricao { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Data de Realização")]
        [DataType(DataType.DateTime)]
        public DateTime Data { get; set; }
    }
}