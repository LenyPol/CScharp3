using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.WebApi;
using NSubstitute;
using ToDoList.Persistence.Repositories;

namespace ToDoList.Test.UnitTests;

public class GetTests
{

    [Fact]
    public void Get_AllItems_ReturnsAllItems()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);
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

        repositoryMock.ReadAll().Returns(new List<ToDoItem> { todoItem1, todoItem2 });

        // Act
        var actionResult = controller.Read();
        var result = actionResult.Result as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        var value = result.Value as List<ToDoItemGetResponseDto>;
        Assert.NotNull(value);
        Assert.Equal(2, value!.Count);

        var firstToDo = value.First();
        Assert.Equal("Jmeno1", firstToDo.Name);
        Assert.Equal("Popis1", firstToDo.Description);
        Assert.False(firstToDo.IsCompleted);
    }
    [Fact]
    public void Get_ById_ReturnsCorrectItem()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);

        var todoItem = new ToDoItem
        {
            ToDoItemId = 1,
            Name = "Jmeno1",
            Description = "Popis1",
            IsCompleted = false
        };

        repositoryMock.ReadById(1).Returns(todoItem);

        // Act
        var result = controller.ReadById(todoItem.ToDoItemId) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        var value = result!.Value as ToDoItemGetResponseDto;
        Assert.NotNull(value);

        Assert.Equal(1, value!.Id);
        Assert.Equal("Jmeno1", value.Name);
        Assert.Equal("Popis1", value.Description);
        Assert.False(value.IsCompleted);
    }

    [Fact]
    public void Get_ById_NonExisting_ReturnsNotFound()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);

        repositoryMock.ReadById(Arg.Any<int>()).Returns((ToDoItem?)null);

        // Act
        var result = controller.ReadById(9999);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
