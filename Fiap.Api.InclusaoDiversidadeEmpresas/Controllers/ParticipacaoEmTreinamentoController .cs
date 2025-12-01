using Fiap.Api.InclusaoDiversidadeEmpresas.Services;
using InclusaoDiversidadeEmpresas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization; 

namespace Fiap.Api.InclusaoDiversidadeEmpresas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ParticipacaoEmTreinamentoController : ControllerBase
    {
        private readonly IParticipacaoEmTreinamentoService _participacaoService;

        public ParticipacaoEmTreinamentoController(IParticipacaoEmTreinamentoService participacaoService)
        {
            _participacaoService = participacaoService;
        }


        // GET: api/participacaoemtreinamento
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ParticipacaoEmTreinamentoModel>>> GetParticipacoes()
        {
            return Ok(await _participacaoService.ListarParticipacaoEmTreinamentoService());
        }

        // GET: api/participacaoemtreinamento/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ParticipacaoEmTreinamentoModel>> GetParticipacao(long id)
        {
            var participacao = await _participacaoService.ObterParticipacaoEmTreinamentoServicePorId(id);

            if (participacao == null)
                return NotFound();

            return Ok(participacao);
        }

    

        // POST: api/participacaoemtreinamento
        [HttpPost]
        public async Task<ActionResult<ParticipacaoEmTreinamentoModel>> PostParticipacao(
            ParticipacaoEmTreinamentoModel participacao)
        {
            var criado = await _participacaoService.CriarParticipacaoEmTreinamentoService(participacao);

            return CreatedAtAction(nameof(GetParticipacao), new { id = criado.Id }, criado);
        }

     

        // PUT: api/participacaoemtreinamento/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<ParticipacaoEmTreinamentoModel>> PutParticipacao(
            long id,
            ParticipacaoEmTreinamentoModel participacao)
        {
            if (id != participacao.Id)
                return BadRequest("O ID informado não corresponde ao objeto enviado.");

            var atualizado = await _participacaoService.AtualizarParticipacaoEmTreinamentoService(participacao);

            if (atualizado == null)
                return NotFound();

            return Ok(atualizado);
        }


        // DELETE: api/participacaoemtreinamento/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] 
        public async Task<IActionResult> DeleteParticipacao(long id)
        {
            var removido = await _participacaoService.DeletarParticipacaoEmTreinamentoService(id);

            if (!removido)
                return NotFound();

            return NoContent();
        }

    }


}
