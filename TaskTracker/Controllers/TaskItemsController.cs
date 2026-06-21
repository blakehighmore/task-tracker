using Microsoft.AspNetCore.Mvc;
using TaskTracker.DTOs;
using TaskTracker.DTOs.TaskItem;
using TaskTracker.Services.TaskItem;


namespace TaskTracker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TaskItemsController : ControllerBase
{
    private readonly ITaskItemService _service;

    public TaskItemsController(ITaskItemService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<TaskItemReadDto>>> GetAll([FromQuery] TaskItemQueryParameters query)
    {
        return await _service.GetAllAsync(query);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TaskItemReadDto>> GetById(int id)
    {
        var taskItem = await _service.GetByIdAsync(id);

        if (taskItem is null) return NotFound();

        return Ok(taskItem);
    }

    [HttpPost]
    public async Task<ActionResult<TaskItemReadDto>> Create(TaskItemCreateDto dto)
    {
        var taskItem = await _service.CreateAsync(dto);

        return CreatedAtAction(nameof(GetById), new { id = taskItem.Id }, taskItem);
    }


    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, TaskItemUpdateDto dto)
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