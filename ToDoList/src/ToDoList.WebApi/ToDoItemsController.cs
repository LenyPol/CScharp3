namespace ToDoList.WebApi;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using System.Linq;

[Route("api/[controller]")] //localhost:5000/api/ToDoItems
[ApiController]

public class ToDoItemsController : ControllerBase
{
    private static readonly List<ToDoItem> items = [];

    [HttpPost]
    public IActionResult Create([FromBody] ToDoItemCreateRequestDto request) // DTO - Data Transfer Object
    {
        var item = request.ToDomain();

        try
        {
            item.ToDoItemId = items.Count == 0 ? 1 : items.Max(o => o.ToDoItemId) + 1; // vygenerování nového ID
            items.Add(item);// přidání
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError); //500 s chybovou hláškou
        }
        // odpověď klientovi 201 Created, Location header, objekt
        return CreatedAtAction(
            nameof(ReadById),
            new { toDoItemId = item.ToDoItemId },
            ToDoItemGetResponseDto.FromDomain(item)
        );
    }

    [HttpGet]
    public ActionResult<IEnumerable<ToDoItemGetResponseDto>> Read() // GET: /api/ToDoItems
    {
        try
        {
            if (items == null) // kontrola, zda list je
                return NotFound();// 404 pokud ne

            var response = items // převedení na DTO objekty
                .Select(ToDoItemGetResponseDto.FromDomain)
                .ToList();

            return Ok(response);// 200
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError); // 500
        }
    }

    [HttpGet("{toDoItemId:int}")]
    public IActionResult ReadById(int toDoItemId) // api/ToDoItems/<id>
    {
        try
        {
            var item = items.Find(i => i.ToDoItemId == toDoItemId); // vrací úrvek splňující predikát, nebo null

            if (item is null)
                return NotFound();//404

            return Ok(ToDoItemGetResponseDto.FromDomain(item));// vracíme DTO
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);//500
        }
    }

    [HttpPut("{toDoItemId:int}")]
    public IActionResult UpdateById(int toDoItemId, [FromBody] ToDoItemUpdateRequestDto request)
    {
        try
        {
            var index = items.FindIndex(i => i.ToDoItemId == toDoItemId);
            if (index < 0)
                return NotFound(); //404

            var updatedItem = request.ToDomain();
            updatedItem.ToDoItemId = toDoItemId;

            items[index] = updatedItem;

            return NoContent();//204
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError); // 500
        }
    }

    [HttpDelete("{toDoItemId:int}")]
    public IActionResult DeleteById(int toDoItemId)
    {
        try
        {
            var item = items.Find(i => i.ToDoItemId == toDoItemId); // najde položku podle ID list vrátí prvek/null
            if (item is null)
                return NotFound(); // 404

            items.Remove(item); // smaže a vrátí 204
            return NoContent();
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }
    }
    public void AddItemToStorage(ToDoItem item)
    {
        items.Add(item);
    }
}

