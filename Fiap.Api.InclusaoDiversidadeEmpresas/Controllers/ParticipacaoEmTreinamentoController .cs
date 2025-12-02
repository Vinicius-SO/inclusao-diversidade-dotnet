using AutoMapper;
using Fiap.Api.InclusaoDiversidadeEmpresas.Services;
using Fiap.Api.InclusaoDiversidadeEmpresas.ViewModel;
using InclusaoDiversidadeEmpresas.Models;
using InclusaoDiversidadeEmpresas.ViewModels;
using Microsoft.AspNetCore.Authorization; 
using Microsoft.AspNetCore.Mvc;

namespace Fiap.Api.InclusaoDiversidadeEmpresas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParticipacaoController : ControllerBase
    {
        private readonly IParticipacaoEmTreinamentoService _service;
        private readonly IMapper _mapper;

        public ParticipacaoController(
            IParticipacaoEmTreinamentoService service,
            IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        // POST
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<ParticipacaoViewModel>> Criar(ParticipacaoViewModel vm)
        {
            var model = _mapper.Map<ParticipacaoEmTreinamentoModel>(vm);

            var criado = await _service.CriarParticipacaoEmTreinamentoService(model);

            return Ok(_mapper.Map<ParticipacaoViewModel>(criado));
        }

        // GET paginado
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ParticipacaoViewModel>>> Listar(
            int page = 1, int pageSize = 10)
        {
            var dados = await _service.ListarParticipacaoPaginado(page, pageSize);

            return Ok(_mapper.Map<IEnumerable<ParticipacaoViewModel>>(dados));
        }

        // PUT
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(long id, ParticipacaoViewModel vm)
        {
            var model = _mapper.Map<ParticipacaoEmTreinamentoModel>(vm);

            var atualizado = await _service.AtualizarParticipacaoEmTreinamentoService(id, model);

            if (atualizado == null)
                return NotFound();

            return Ok(_mapper.Map<ParticipacaoViewModel>(atualizado));
        }

        // DELETE
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(long id)
        {
            var removido = await _service.DeletarParticipacaoEmTreinamentoService(id);

            if (!removido)
                return NotFound();

            return NoContent();
        }
    }


}
