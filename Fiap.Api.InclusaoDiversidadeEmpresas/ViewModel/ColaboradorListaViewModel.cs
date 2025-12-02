
namespace Fiap.Api.InclusaoDiversidadeEmpresas.ViewModels // Use seu namespace correto
{
    public class ColaboradorListaViewModel
    {
        public long Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Departamento { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public string GeneroColaborador { get; set; } = string.Empty;
        public string EtniaColaborador { get; set; } = string.Empty;
        public bool TemDisabilidade { get; set; }
    }
}