namespace ToDoList.Test.IntegrationTests;

using ToDoList.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.Models;
using ToDoList.WebApi;

public class GetTests : ToDoListControllerTestBase
{

    [Fact]
    public void Get_AllItems_ReturnsAllItems()
    {
        // Arrange
        var todoItem1 = new ToDoItem
        {
            ToDoItemId = 1,
            Name = "Jmeno1",
            Description = "Popis1",
            IsCompleted = false
        };
        var todoItem2 = new ToDoItem
        {
            ToDoItemId = 2,
            Name = "Jmeno2",
            Description = "Popis2",
            IsCompleted = true
        };

        DbContext.ToDoItems.AddRange(todoItem1, todoItem2);
        DbContext.SaveChanges();

        // Act
        var actionResult = Controller.Read();
        var result = actionResult.Result as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        var value = result.Value as List<ToDoItemGetResponseDto>;
        Assert.NotNull(value);
        Assert.Equal(2, value.Count);

        var firstToDo = value.First();
        Assert.Equal("Jmeno1", firstToDo.Name);
        Assert.Equal("Popis1", firstToDo.Description);
        Assert.False(firstToDo.IsCompleted);
    }
    [Fact]
    public void Get_ById_ReturnsCorrectItem()
    {
        // Arrange
        var todoItem = new ToDoItem
        {
            Name = "Jmeno1",
            Description = "Popis1",
            IsCompleted = false
        };

        DbContext.ToDoItems.Add(todoItem);
        DbContext.SaveChanges();

        var id = todoItem.ToDoItemId;
        // Act
        var result = Controller.ReadById(todoItem.ToDoItemId) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        var value = result!.Value as ToDoItemGetResponseDto;
        Assert.NotNull(value);

        Assert.Equal(todoItem.ToDoItemId, value!.Id);
        Assert.Equal(todoItem.Name, value.Name);
        Assert.Equal(todoItem.Description, value.Description);
        Assert.Equal(todoItem.IsCompleted, value.IsCompleted);
    }
    [Fact]
    public void Get_ById_NonExisting_ReturnsNotFound()
    {
        // Act
        var result = Controller.ReadById(9999);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
