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
    public void Get_ReadWhenSomeItemAvailable_ReturnsOk()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);
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
        Assert.Equal("Category1", firstToDo.Category);
        Assert.False(firstToDo.IsCompleted);

        repositoryMock.Received(1).ReadAll();
    }
    [Fact]
    public void Get_ReadByIdWhenSomeItemAvailable_ReturnsOk()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);

        var todoItem = new ToDoItem
        {
            ToDoItemId = 1,
            Name = "Jmeno1",
            Description = "Popis1",
            IsCompleted = false,
            Category = "Category1"
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
        Assert.Equal("Category1", value.Category);
        Assert.False(value.IsCompleted);

        repositoryMock.Received(1).ReadById(1);
    }

    [Fact]
    public void Get_ReadByIdWhenItemIsNull_ReturnsNotFound()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);

        repositoryMock.ReadById(Arg.Any<int>()).Returns((ToDoItem?)null);

        // Act
        var result = controller.ReadById(9999);

        // Assert
        Assert.IsType<NotFoundResult>(result);

        repositoryMock.Received(1).ReadById(Arg.Any<int>());
    }
}
