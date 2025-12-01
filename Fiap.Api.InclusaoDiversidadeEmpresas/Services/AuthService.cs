using Fiap.Api.InclusaoDiversidadeEmpresas.Models.ViewModels;
using InclusaoDiversidadeEmpresas.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Fiap.Api.InclusaoDiversidadeEmpresas.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IColaboradorService _colaboradorService; // Injeção do Service de Colaborador

     
        private const string AdminRole = "Admin";
        private const string UserRole = "User";

        public AuthService(IConfiguration configuration, IColaboradorService colaboradorService)
        {
            _configuration = configuration;
            _colaboradorService = colaboradorService;
        }

        public async Task<string> AuthenticateAndGenerateTokenAsync(LoginModel loginModel)
        {
            // 1. **Validação de Credenciais**

            // 1.1. Tentativa de Login com Admin Fixo (apenas para testes iniciais)
            var adminEmail = _configuration.GetValue<string>("AdminCredentials:Email");
            var adminSenha = _configuration.GetValue<string>("AdminCredentials:Senha");

            if (loginModel.Email.Equals(adminEmail, StringComparison.OrdinalIgnoreCase) &&
                loginModel.Senha == adminSenha)
            {
                // Se for o admin fixo, geramos o token com a Role 'Admin'
                return GenerateToken(adminEmail, AdminRole);
            }
            return null;
        }

        private string GenerateToken(string email, string role)
        {
            var key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("JwtSettings:SecretKey"));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                // Claims: As informações que serão carregadas no Token
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, email),
                    new Claim(ClaimTypes.Role, role)
                }),
                // Tempo de expiração do token (Ex: 7 dias)
                Expires = DateTime.UtcNow.AddDays(7),

                // Credenciais de assinatura
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                ),

                // Configurações de Audiência e Emissor
                Issuer = _configuration.GetValue<string>("JwtSettings:Issuer"),
                Audience = _configuration.GetValue<string>("JwtSettings:Audience")
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
