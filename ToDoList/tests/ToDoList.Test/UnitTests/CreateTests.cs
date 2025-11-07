using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.WebApi;
using NSubstitute;
using ToDoList.Persistence.Repositories;

namespace ToDoList.Test.UnitTests;

public class CreateTests
{

    [Fact]
    public void Create_ValidItem_ReturnCreatedItem()
    {
        //Arrange
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);
        var createRequest = new ToDoItemCreateRequestDto("Jmeno1", "Popis1", false);

        // Simulujeme chování EF – po zavolání Create se novému objektu přidělí ID 1
        repositoryMock
            .When(r => r.Create(Arg.Any<ToDoItem>()))
            .Do(callInfo =>
            {
                var item = callInfo.Arg<ToDoItem>();
                item.ToDoItemId = 1;
            });

        // Act
        var result = controller.Create(createRequest) as CreatedAtActionResult;
        // Assert
        Assert.NotNull(result);
        Assert.Equal(nameof(ToDoItemsController.ReadById), result!.ActionName);

        var value = Assert.IsType<ToDoItemGetResponseDto>(result.Value);
        Assert.Equal("Jmeno1", value!.Name);
        Assert.Equal("Popis1", value.Description);
        Assert.False(value.IsCompleted);
        Assert.Equal(1, value.Id);

        repositoryMock.Received(1).Create(Arg.Any<ToDoItem>());

    }

    [Fact]
    public void Create_WhenExceptionOccurs_ReturnsProblem()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);
        var createRequest = new ToDoItemCreateRequestDto(null!, "Popis bez jména", false);

        // Act
        var result = controller.Create(createRequest);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
        Assert.Equal("Name cannot be null or empty.", badRequestResult.Value);

        //ověřuje, že controller nepokračoval k repository
        repositoryMock.DidNotReceive().Create(Arg.Any<ToDoItem>());
    }
}
