using Fiap.Api.InclusaoDiversidadeEmpresas.Models.ViewModels;
using Fiap.Api.InclusaoDiversidadeEmpresas.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Fiap.Api.InclusaoDiversidadeEmpresas.Controllers
{
    [Route("api/[controller]")] // Rota base: /api/Auth
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // Endpoint: POST /api/Auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Retorna 400 se o DTO não for válido
            }

            // Chama o serviço para autenticar e gerar o token
            var token = await _authService.AuthenticateAndGenerateTokenAsync(loginModel);

            if (string.IsNullOrEmpty(token))
            {
                // Se o token for null ou vazio, as credenciais estavam incorretas
                return Unauthorized(new { message = "Credenciais inválidas." });
            }

            // Retorna o token com sucesso
            return Ok(new { token = token });
        }
    }
}
