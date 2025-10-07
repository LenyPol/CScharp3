namespace ToDoList.WebApi;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;

[Route("api/[controller]")] //localhost:5000/api/ToDoItems
[ApiController]

public class ToDoItemsController : ControllerBase
{

    private static List<ToDoItem> item = [];

    [HttpPost]
    public IActionResult Create(ToDoItemCreateRequestDto request) //použijeme DTO - Data Transfer Object
    {
        return Ok();
    }

    [HttpGet]
    public IActionResult Read() // api/ToDoItems Get
    {
        return Ok();
    }
    [HttpGet("{toDoItemId:int}")]
    public IActionResult ReadById(int toDoItemId) // api/ToDoItems/<id>
    {
        try
        {
            throw new Exception("Něco se nepovedlo");
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);//500
        }
        return Ok(); //200

    }
    [HttpPut("{toDoItemId:int}")]
    public IActionResult UpdateById(int toDoItemId, [FromBody] ToDoItemUpdateRequestDto request)
    {
        return Ok(); //200
    }
    [HttpDelete("{toDoItemId:int}")]
    public IActionResult DeleteById(int toDoItemId)
    {
        return Ok();
    }
}

