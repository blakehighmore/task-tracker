using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskTracker.DTOs.Label;
using TaskTracker.Services.Label;


namespace TaskTracker.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class LabelsController : ControllerBase
{
    private readonly ILabelService _service;

    public LabelsController(ILabelService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LabelReadDto>>> GetAll()
    {
        var list = await _service.GetAllAsync();

        return Ok(list);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<LabelReadDto>> GetById(int id)
    {
        var label = await _service.GetByIdAsync(id);

        if (label is null) return NotFound();

        return Ok(label);
    }

    [HttpPost]
    public async Task<ActionResult<LabelReadDto>> Create(LabelCreateDto dto)
    {
        var label = await _service.CreateAsync(dto);

        return CreatedAtAction(nameof(GetById), new { id = label.Id }, label);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);

        if (!deleted) return NotFound();

        return NoContent();
    }
}