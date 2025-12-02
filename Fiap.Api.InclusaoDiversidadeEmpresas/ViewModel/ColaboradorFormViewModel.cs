using System.ComponentModel.DataAnnotations;

namespace InclusaoDiversidadeEmpresas.ViewModels
{
    // Usada para preencher o formulário de Cadastro
    public class ColaboradorFormViewModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        [Display(Name = "Nome Completo")]
        public string NomeColaborador { get; set; } = string.Empty;

        [Required(ErrorMessage = "O gênero é obrigatório")]
        [Display(Name = "Gênero")]
        public string GeneroColaborador { get; set; } = string.Empty;

        [Required(ErrorMessage = "A etnia é obrigatória")]
        [Display(Name = "Etnia / Raça")]
        public string EtniaColaborador { get; set; } = string.Empty;

        [Required(ErrorMessage = "O departamento é obrigatório")]
        public string Departamento { get; set; } = string.Empty;

        [Required(ErrorMessage = "O e-mail é obrigatório")]
        [EmailAddress(ErrorMessage = "E-mail inválido")]
        public string Email { get; set; } = string.Empty;

        // A senha é obrigatória apenas na criação, na edição pode ser opcional
        [DataType(DataType.Password)]
        public string? Senha { get; set; }

        [Display(Name = "Possui Deficiência (PCD)?")]
        public bool TemDisabilidade { get; set; }
    }
}