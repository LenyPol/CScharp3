namespace ToDoList.Test.IntegrationTests;

using ToDoList.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.Models;

public class GetTests : ToDoListControllerTestBase
{

    [Fact]
    public async Task Get_AllItems_ReturnsAllItems()
    {
        // Arrange
        var todoItem1 = new ToDoItem
        {
            ToDoItemId = 1,
            Name = "Jmeno1",
            Description = "Popis1",
            IsCompleted = false,
            Category = "Category1"
        };
        var todoItem2 = new ToDoItem
        {
            ToDoItemId = 2,
            Name = "Jmeno2",
            Description = "Popis2",
            IsCompleted = true,
            Category = "Category2"
        };

        DbContext.ToDoItems.AddRange(todoItem1, todoItem2);
        DbContext.SaveChanges();

        // Act
        var actionResult = await Controller.Read();
        var result = actionResult.Result as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        var value = result.Value as List<ToDoItemGetResponseDto>;
        Assert.NotNull(value);
        Assert.Equal(2, value.Count);

        var firstToDo = value.First();
        Assert.Equal("Jmeno1", firstToDo.Name);
        Assert.Equal("Popis1", firstToDo.Description);
        Assert.Equal("Category1", firstToDo.Category);
        Assert.False(firstToDo.IsCompleted);
    }
    [Fact]
    public async Task Get_ById_ReturnsCorrectItem()
    {
        // Arrange
        var todoItem = new ToDoItem
        {
            Name = "Jmeno1",
            Description = "Popis1",
            IsCompleted = false,
            Category = "Category1"
        };

        DbContext.ToDoItems.Add(todoItem);
        DbContext.SaveChanges();

        var id = todoItem.ToDoItemId;
        // Act
        var result = await Controller.ReadById(todoItem.ToDoItemId) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        var value = result!.Value as ToDoItemGetResponseDto;
        Assert.NotNull(value);

        Assert.Equal(todoItem.ToDoItemId, value!.Id);
        Assert.Equal(todoItem.Name, value.Name);
        Assert.Equal(todoItem.Description, value.Description);
        Assert.Equal(todoItem.Category, value.Category);
        Assert.Equal(todoItem.IsCompleted, value.IsCompleted);
    }
    [Fact]
    public async Task Get_ById_NonExisting_ReturnsNotFound()
    {
        // Act
        var result = await Controller.ReadById(9999);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
