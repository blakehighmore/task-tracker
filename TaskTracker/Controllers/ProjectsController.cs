using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskTracker.DTOs;
using TaskTracker.DTOs.Project;
using TaskTracker.Services.Project;


namespace TaskTracker.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _service;

    public ProjectsController(IProjectService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<ProjectReadDto>>> GetAll([FromQuery] ProjectQueryParameters query)
    {
        var project = await _service.GetAllAsync(query);

        return Ok(project);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProjectReadDto>> GetById(int id)
    {
        var project = await _service.GetByIdAsync(id);

        if (project is null) return NotFound();

        return Ok(project);
    }

    [HttpPost]
    public async Task<ActionResult<ProjectReadDto>> Create(ProjectCreateDto dto)
    {
        var ownerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var created = await _service.CreateAsync(dto, ownerId);

        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, ProjectUpdateDto dto)
    {
        var updated = await _service.UpdateAsync(id, dto);

        if (!updated) return NotFound();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);

        if (!deleted) return NotFound();

        return NoContent();
    }
}