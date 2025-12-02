using InclusaoDiversidadeEmpresas.Services;
using InclusaoDiversidadeEmpresas.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace InclusaoDiversidadeEmpresas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RelatoriosController : ControllerBase
    {
        private readonly IRelatorioService _relatorioService;


        public RelatoriosController(IRelatorioService relatorioService)
        {
            _relatorioService = relatorioService;
        }

        // Endpoint: GET /api/Relatorios/diversidade
        [HttpGet("diversidade")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetRelatorioDiversidade()
        {
            var relatorio = await _relatorioService.GerarRelatorioAsync();

            if (relatorio == null)
            {
                return NotFound("Não foi possível gerar o relatório.");
            }

            // Retorna o objeto RelatorioDeDiversidadeModel no formato JSON
            return Ok(new DashboardDiversidadeViewModel(relatorio));
        }
    }
}