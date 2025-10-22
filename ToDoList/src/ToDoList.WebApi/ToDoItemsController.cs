namespace ToDoList.WebApi;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using System.Linq;

/// <summary>
/// Controller pro správu ToDo položek.
/// localhost:5000/api/ToDoItems
/// </summary>
[Route("api/[controller]")]
[ApiController]

public class ToDoItemsController : ControllerBase
{
    private readonly List<ToDoItem> items = [];


    /// <summary>
    /// Vytvoří nový ToDoItem na základě dat z požadavku.
    /// </summary>
    /// <param name="request">DTO objekt s údaji o úkolu k vytvoření.</param>
    /// <returns>
    /// Vrací <see cref="CreatedAtActionResult"/> s vytvořeným objektem (HTTP 201),
    /// nebo chybu 500 při selhání.
    /// </returns>
    [HttpPost]
    public IActionResult Create([FromBody] ToDoItemCreateRequestDto request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                throw new ArgumentException("Name cannot be null or empty.");

            var item = request.ToDomain();

            item.ToDoItemId = items.Count == 0 ? 1 : items.Max(o => o.ToDoItemId) + 1;
            items.Add(item);

            return CreatedAtAction(
                nameof(ReadById),
                new { toDoItemId = item.ToDoItemId },
                ToDoItemGetResponseDto.FromDomain(item)
            );
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }
    }
    /// <summary>
    /// Vrátí všechny uložené ToDo položky.
    /// </summary>
    /// <returns>
    /// Vrací <see cref="OkObjectResult"/> (HTTP 200) s kolekcí položek,
    /// nebo <see cref="NotFoundResult"/>, pokud seznam neexistuje.
    /// </returns>
    [HttpGet]
    public ActionResult<IEnumerable<ToDoItemGetResponseDto>> Read()
    {
        try
        {
            if (items == null)
                return NotFound();

            var response = items
                .Select(ToDoItemGetResponseDto.FromDomain)
                .ToList();

            return Ok(response);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }
    }
    /// <summary>
    /// Vrátí jednu ToDo položku podle jejího ID.
    /// </summary>
    /// <param name="toDoItemId">ID položky, kterou chceme načíst.</param>
    /// <returns>
    /// Vrací <see cref="OkObjectResult"/> (HTTP 200) s položkou,
    /// nebo <see cref="NotFoundResult"/>, pokud neexistuje.
    /// </returns>
    [HttpGet("{toDoItemId:int}")]
    public IActionResult ReadById(int toDoItemId)
    {
        try
        {
            var item = items.Find(i => i.ToDoItemId == toDoItemId);

            if (item is null)
                return NotFound();

            return Ok(ToDoItemGetResponseDto.FromDomain(item));
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }
    }
    /// <summary>
    /// Aktualizuje ToDo položku podle ID.
    /// </summary>
    /// <param name="toDoItemId">ID položky, kterou chceme aktualizovat.</param>
    /// <param name="request">DTO objekt s novými daty položky.</param>
    /// <returns>
    /// Vrací <see cref="NoContentResult"/> (HTTP 204), pokud je úspěšně aktualizována,
    /// nebo <see cref="NotFoundResult"/>, pokud položka neexistuje.
    /// </returns>
    [HttpPut("{toDoItemId:int}")]
    public IActionResult UpdateById(int toDoItemId, [FromBody] ToDoItemUpdateRequestDto request)
    {
        try
        {
            var item = items.SingleOrDefault(i => i.ToDoItemId == toDoItemId);
            if (item is null)
                return NotFound();

            item.Name = request.Name;
            item.Description = request.Description;
            item.IsCompleted = request.IsCompleted;

            return NoContent();
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }
    }
    /// <summary>
    /// Odstraní ToDo položku podle ID.
    /// </summary>
    /// <param name="toDoItemId">ID položky, kterou chceme odstranit.</param>
    /// <returns>
    /// Vrací <see cref="NoContentResult"/> (HTTP 204), pokud je položka úspěšně smazána,
    /// nebo <see cref="NotFoundResult"/>, 404 pokud neexistuje.
    /// </returns>
    [HttpDelete("{toDoItemId:int}")]
    public IActionResult DeleteById(int toDoItemId)
    {
        try
        {
            var item = items.Find(i => i.ToDoItemId == toDoItemId);
            if (item is null)
                return NotFound();

            items.Remove(item);
            return NoContent();
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }
    }
}

