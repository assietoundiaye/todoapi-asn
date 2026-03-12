using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoApi_AsN.Models;
using TodoApi_AsN.Services;

namespace TodoApi_AsN.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TodoItemsController : ControllerBase
{
    private readonly TodoService _todoService;

    public TodoItemsController(TodoService todoService)
    {
        _todoService = todoService;
    }

    // GET: api/TodoItems — tous les utilisateurs authentifiés
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetTodoItems()
    {
        var items = await _todoService.GetAllAsync();
        return Ok(items.Select(ItemToDTO).ToList());
    }

    // GET: api/TodoItems/{id} — tous les utilisateurs authentifiés
    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<TodoItemDTO>> GetTodoItem(string id)
    {
        var item = await _todoService.GetByIdAsync(id);

        if (item == null)
            return NotFound();

        return Ok(ItemToDTO(item));
    }

    // POST: api/TodoItems — admin uniquement
    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<TodoItemDTO>> PostTodoItem(TodoItemDTO todoDTO)
    {
        var todoItem = new TodoItem
        {
            Name = todoDTO.Name,
            IsComplete = todoDTO.IsComplete
        };

        await _todoService.CreateAsync(todoItem);

        return CreatedAtAction(
            nameof(GetTodoItem),
            new { id = todoItem.Id },
            ItemToDTO(todoItem));
    }

    // PUT: api/TodoItems/{id} — admin uniquement
    [HttpPut("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> PutTodoItem(string id, TodoItemDTO todoDTO)
    {
        if (id != todoDTO.Id)
            return BadRequest("L'ID dans l'URL ne correspond pas à l'ID de l'objet.");

        var existingItem = await _todoService.GetByIdAsync(id);
        if (existingItem == null)
            return NotFound();

        existingItem.Name = todoDTO.Name;
        existingItem.IsComplete = todoDTO.IsComplete;

        await _todoService.UpdateAsync(id, existingItem);

        return NoContent();
    }

    // DELETE: api/TodoItems/{id} — admin uniquement
    [HttpDelete("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> DeleteTodoItem(string id)
    {
        var todoItem = await _todoService.GetByIdAsync(id);
        if (todoItem == null)
            return NotFound();

        await _todoService.RemoveAsync(id);

        return NoContent();
    }

    private static TodoItemDTO ItemToDTO(TodoItem item) => new TodoItemDTO
    {
        Id = item.Id,
        Name = item.Name,
        IsComplete = item.IsComplete
    };
}
