
using Fiap.Api.InclusaoDiversidadeEmpresas.Models;
using Fiap.Api.InclusaoDiversidadeEmpresas.Services;
using Fiap.Api.InclusaoDiversidadeEmpresas.ViewModel;
using Fiap.Api.InclusaoDiversidadeEmpresas.ViewModels;
using InclusaoDiversidadeEmpresas.Models;
using InclusaoDiversidadeEmpresas.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ColaboradoresController : ControllerBase
{
    private readonly IColaboradorService _service;

    public ColaboradoresController(IColaboradorService service)
    {
        _service = service;
    }


    // ... (Método POST)
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Colaborador>> PostColaborador(Colaborador colaborador)
    {
        var novoColaborador = await _service.AddColaborador(colaborador);

        return CreatedAtAction(nameof(GetColaborador), new { id = novoColaborador.Id }, novoColaborador);
    }


    // READ (LISTAR TODOS) 
    // Mapeado para GET /api/Colaboradores?PageNumber=...&PageSize=...
    [HttpGet]
    public async Task<ActionResult<PagedResultViewModel<ColaboradorListaViewModel>>> GetColaboradores(
        [FromQuery] QueryParameters parameters)
    {
        // As validações de tamanho de página (min/max) já estão dentro do QueryParameters.cs!
        var resultado = await _service.GetAllColaboradores(parameters);

        return Ok(resultado);
    }

    // ... (READ por ID, PUT e DELETE permanecem os mesmos)
    [HttpGet("{id}")]
    public async Task<ActionResult<Colaborador>> GetColaborador(long id)
    {
        var colaborador = await _service.GetColaboradorById(id);

        if (colaborador == null)
        {
            return NotFound();
        }

        return Ok(colaborador);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> PutColaborador(long id, Colaborador colaborador)
    {
        if (id != colaborador.Id)
        {
            return BadRequest();
        }

        var colaboradorAtualizado = await _service.UpdateColaborador(colaborador);

        if (colaboradorAtualizado == null)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteColaborador(long id)
    {
        var success = await _service.DeleteColaborador(id);

        if (!success)
        {
            return NotFound();
        }

        return NoContent();
    }
}