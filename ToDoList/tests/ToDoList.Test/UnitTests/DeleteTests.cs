using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.Models;
using ToDoList.WebApi;
using NSubstitute;
using ToDoList.Persistence.Repositories;

namespace ToDoList.Test.UnitTests;

public class DeleteTests
{

    [Fact]
    public void Delete_ExistingItem_ReturnsNoContent()
    {
        //Arrange
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
        var result = controller.DeleteById(1);

        // Assert
        Assert.IsType<NoContentResult>(result);

        repositoryMock.Received(1).Delete(todoItem);
    }

    [Fact]
    public void Delete_NonExistingItem_ReturnsNotFound()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);

        repositoryMock.ReadById(999).Returns((ToDoItem?)null);

        // Act
        var result = controller.DeleteById(999);

        // Assert
        Assert.IsType<NotFoundResult>(result);

        repositoryMock.DidNotReceive().Delete(Arg.Any<ToDoItem>());
    }

    [Fact]
    public void Delete_WhenRepositoryThrowsException_ReturnsInternalServerError()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);
        var todoItem = new ToDoItem
        {
            ToDoItemId = 10,
            Name = "Jmeno10",
            Description = "Popis10",
            IsCompleted = false
        };

        repositoryMock.ReadById(10).Returns(todoItem);
        repositoryMock
            .When(r => r.Delete(todoItem))
            .Do(_ => throw new Exception("Simulated DB error"));

        // Act
        var result = controller.DeleteById(10);

        // Assert
        var problemResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, problemResult.StatusCode);
    }
}
