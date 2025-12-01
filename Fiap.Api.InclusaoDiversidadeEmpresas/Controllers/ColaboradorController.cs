using Microsoft.AspNetCore.Mvc;
using InclusaoDiversidadeEmpresas.Models;
using InclusaoDiversidadeEmpresas.Services;
using Microsoft.AspNetCore.Authorization; // 👈 NECESSÁRIO para usar [Authorize]


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

   
    // Mapeado para POST /api/Colaboradores
    [HttpPost]
    [Authorize(Roles = "Admin")] //Apenas Admin pode criar um novo Colaborador
    public async Task<ActionResult<Colaborador>> PostColaborador(Colaborador colaborador)
    {
        var novoColaborador = await _service.AddColaborador(colaborador);

        return CreatedAtAction(nameof(GetColaborador), new { id = novoColaborador.Id }, novoColaborador);
    }


    // READ (LISTAR TODOS)
    // Mapeado para GET /api/Colaboradores
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Colaborador>>> GetColaboradores()
    {
        var colaboradores = await _service.GetAllColaboradores();

        return Ok(colaboradores);
    }

    // READ (LISTAR POR ID)
    // Mapeado para GET /api/Colaboradores/{id}
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

    // Mapeado para PUT /api/Colaboradores/{id}
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")] //Apenas Admin pode atualizar
    public async Task<IActionResult> PutColaborador(long id, Colaborador colaborador)
    {
        if (id != colaborador.Id)
        {
            return BadRequest();
        }

        // Chama o método no Service
        var colaboradorAtualizado = await _service.UpdateColaborador(colaborador);

        if (colaboradorAtualizado == null)
        {
            return NotFound();
        }

        return NoContent();
    }


    // Mapeado para DELETE /api/Colaboradores/{id}
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