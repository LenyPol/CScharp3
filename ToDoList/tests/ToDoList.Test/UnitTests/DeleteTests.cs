using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.Models;
using ToDoList.WebApi;
using NSubstitute;
using ToDoList.Persistence.Repositories;

namespace ToDoList.Test.UnitTests;

public class DeleteTests
{

    [Fact]
    public async Task Delete_DeleteByIdValidItemId_ReturnsNoContent()
    {
        //Arrange
        var repositoryMock = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);

        var todoItem = new ToDoItem
        {
            ToDoItemId = 1,
            Name = "Jmeno1",
            Description = "Popis1",
            IsCompleted = false
        };

        repositoryMock.ReadById(1).Returns(Task.FromResult(todoItem));
        repositoryMock.Delete(todoItem).Returns(Task.CompletedTask);

        // Act
        var result = await controller.DeleteById(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
        await repositoryMock.Received(1).Delete(todoItem);
    }

    [Fact]
    public async Task Delete_DeleteByIdInvalidItemId_ReturnsNotFound()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);

        repositoryMock.ReadById(999).Returns(Task.FromResult<ToDoItem?>(null));

        // Act
        var result = await controller.DeleteById(999);

        // Assert
        Assert.IsType<NotFoundResult>(result);
        await repositoryMock.DidNotReceive().Delete(Arg.Any<ToDoItem>());
    }

    [Fact]
    public async Task Delete_DeleteByIdUnhandledException_ReturnsInternalServerError()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);

        var todoItem = new ToDoItem
        {
            ToDoItemId = 10,
            Name = "Jmeno10",
            Description = "Popis10",
            IsCompleted = false
        };

        repositoryMock.ReadById(10).Returns(Task.FromResult(todoItem));

        repositoryMock
            .Delete(todoItem)
            .Returns<Task>(_ => throw new Exception("Simulated DB error"));

        // Act
        var result = await controller.DeleteById(10);

        // Assert
        var problemResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, problemResult.StatusCode);

        await repositoryMock.Received(1).ReadById(10);
        await repositoryMock.Received(1).Delete(todoItem);
    }
}
