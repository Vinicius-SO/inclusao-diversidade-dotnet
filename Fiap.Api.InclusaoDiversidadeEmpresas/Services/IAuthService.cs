using Fiap.Api.InclusaoDiversidadeEmpresas.Models.ViewModels;
using System.Threading.Tasks;

namespace Fiap.Api.InclusaoDiversidadeEmpresas.Services
{
    public interface IAuthService
    {
        // Retorna o token se o login for válido, caso contrário, retorna null ou string vazia.
        Task<string> AuthenticateAndGenerateTokenAsync(LoginModel loginModel);
    }
}
