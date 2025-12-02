

using AutoMapper;
using Fiap.Api.InclusaoDiversidadeEmpresas.Models; 
using Fiap.Api.InclusaoDiversidadeEmpresas.Services;
using Fiap.Api.InclusaoDiversidadeEmpresas.ViewModels; 
using InclusaoDiversidadeEmpresas.Models;
using InclusaoDiversidadeEmpresas.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;



namespace Fiap.Api.InclusaoDiversidadeEmpresas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TreinamentoController : ControllerBase
    {
        private readonly ITreinamentoService _treinamentoService;
        private readonly IMapper _mapper;

        public TreinamentoController(
            ITreinamentoService treinamentoService,
            IMapper mapper)
        {
            _treinamentoService = treinamentoService;
            _mapper = mapper;
        }

        // READ (LISTAR TODOS) 
        [HttpGet]
        public async Task<ActionResult<PagedResultViewModel<TreinamentoModel>>> GetTreinamentos(
            [FromQuery] QueryParameters parameters)
        {
           
            var resultado = await _treinamentoService.GetAllTreinamentos(parameters);

            return Ok(resultado);
        }

        // READ (Por ID) - Antigo GetTreinamento
        [HttpGet("{id}")]
        public async Task<ActionResult<TreinamentoViewModel>> GetTreinamento(long id)
        {
            var treinamento = await _treinamentoService.GetTreinamentoById(id);

            if (treinamento == null)
                return NotFound();

            return Ok(_mapper.Map<TreinamentoViewModel>(treinamento));
        }

        // POST (CREATE) -
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<TreinamentoViewModel>> PostTreinamento(TreinamentoViewModel vm)
        {
            var model = _mapper.Map<TreinamentoModel>(vm);

            // Chamada do novo método
            var criado = await _treinamentoService.AddTreinamento(model);

            var retorno = _mapper.Map<TreinamentoViewModel>(criado);

            return CreatedAtAction(nameof(GetTreinamento), new { id = criado.Id }, retorno);
        }

        // PUT (UPDATE) 
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutTreinamento(long id, TreinamentoViewModel vm)
        {
            if (id != vm.Id)
                return BadRequest("O ID informado não corresponde ao objeto enviado.");

            var model = _mapper.Map<TreinamentoModel>(vm);

           
            var atualizado = await _treinamentoService.UpdateTreinamento(id, model);

            if (atualizado == null)
                return NotFound();

            return Ok(_mapper.Map<TreinamentoViewModel>(atualizado));
        }

        // DELETE
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteTreinamento(long id)
        {
           
            var removido = await _treinamentoService.DeleteTreinamento(id);

            if (!removido)
                return NotFound();

            return NoContent();
        }
    }
}