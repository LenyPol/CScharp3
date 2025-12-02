using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.WebApi;
using NSubstitute;
using ToDoList.Persistence.Repositories;

namespace ToDoList.Test.UnitTests;

public class UpdateTests
{

    [Fact]
    public async Task Put_UpdateByIdWhenItemUpdated_ReturnsNoContent()
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

        var updateRequest = new ToDoItemUpdateRequestDto("Jmeno2", "Popis2", true, "Category2");

        repositoryMock.ReadById(1).Returns(Task.FromResult<ToDoItem?>(todoItem));

        // Act
        var result = await controller.UpdateById(1, updateRequest);

        // Assert
        Assert.IsType<NoContentResult>(result);

        await repositoryMock.Received(1).Update(Arg.Is<ToDoItem>(item =>
            item.ToDoItemId == 1 &&
            item.Name == "Jmeno2" &&
            item.Description == "Popis2" &&
            item.Category == "Category2" &&
            item.IsCompleted == true
        ));
    }

    [Fact]
    public async Task Put_UpdateByIdWhenIdNotFound_ReturnsNotFound()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);

        var updateRequest = new ToDoItemUpdateRequestDto("NonExisting", "Attempt to update", false, null);

        repositoryMock.ReadById(Arg.Any<int>()).Returns((ToDoItem?)null);
        // Act
        var result = await controller.UpdateById(999, updateRequest);

        // Assert
        Assert.IsType<NotFoundResult>(result);

        await repositoryMock.DidNotReceive().Update(Arg.Any<ToDoItem>());
    }
}
