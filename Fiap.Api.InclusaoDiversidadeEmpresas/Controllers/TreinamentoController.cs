using Fiap.Api.InclusaoDiversidadeEmpresas.Services;
using InclusaoDiversidadeEmpresas.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization; // 👈 Adicione este using

namespace Fiap.Api.InclusaoDiversidadeEmpresas.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize] 
    public class TreinamentoController : ControllerBase
    {
        private readonly ITreinamentoService _treinamentoService;

        public TreinamentoController(ITreinamentoService treinamentoService)
        {
            _treinamentoService = treinamentoService;
        }


        // GET: api/treinamento
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TreinamentoModel>>> GetTreinamentos()
        {
            return Ok(await _treinamentoService.ListarTreinamentos());
        }

        // GET: api/treinamento/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<TreinamentoModel>> GetTreinamento(long id)
        {
            var treinamento = await _treinamentoService.ObterTreinamentoPorId(id);

            if (treinamento == null)
                return NotFound();

            return Ok(treinamento);
        }

        // POST: api/treinamento
        [HttpPost]
        [Authorize(Roles = "Admin")] // 👈 2. RESTRIÇÃO: Apenas Admin pode criar
        public async Task<ActionResult<TreinamentoModel>> PostTreinamento(TreinamentoModel treinamento)
        {
            var criado = await _treinamentoService.CriarTreinamento(treinamento);

            return CreatedAtAction(nameof(GetTreinamento), new { id = criado.Id }, criado);
        }

  

        // PUT: api/treinamento/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")] // 👈 2. RESTRIÇÃO: Apenas Admin pode atualizar
        public async Task<ActionResult<TreinamentoModel>> PutTreinamento(long id, TreinamentoModel treinamento)
        {
            if (id != treinamento.Id)
                return BadRequest("O ID informado não corresponde ao objeto enviado.");

            var atualizado = await _treinamentoService.AtualizarTreinamento(treinamento);

            if (atualizado == null)
                return NotFound();

            return Ok(atualizado);
        }

     

        // DELETE: api/treinamento/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] // 👈 2. RESTRIÇÃO: Apenas Admin pode deletar
        public async Task<IActionResult> DeleteTreinamento(long id)
        {
            var removido = await _treinamentoService.DeletarTreinamento(id);

            if (!removido)
                return NotFound();

            return NoContent();
        }
    }
}