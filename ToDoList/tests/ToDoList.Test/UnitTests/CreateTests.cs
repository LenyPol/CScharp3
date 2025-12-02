using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.WebApi;
using NSubstitute;
using ToDoList.Persistence.Repositories;
using Microsoft.CodeAnalysis.CSharp;

namespace ToDoList.Test.UnitTests;

public class CreateTests
{

    [Fact]
    public async Task Post_CreateValidRequest_ReturnsCreatedAtAction()
    {
        //Arrange
        var repositoryMock = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);
        var createRequest = new ToDoItemCreateRequestDto("Jmeno1", "Popis1", false, "Category1");

        // Simulujeme chování EF – po zavolání Create se novému objektu přidělí ID 1
        repositoryMock
        .Create(Arg.Do<ToDoItem>(item =>
        {
            item.ToDoItemId = 1;   //  nastaví ID do objektu vytvořeného v kontroleru
        }))
        .Returns(Task.CompletedTask);

        // Act
        var result = await controller.Create(createRequest) as CreatedAtActionResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(nameof(ToDoItemsController.ReadById), result!.ActionName);

        var value = Assert.IsType<ToDoItemGetResponseDto>(result.Value);
        Assert.Equal("Jmeno1", value!.Name);
        Assert.Equal("Popis1", value.Description);
        Assert.Equal("Category1", value.Category);
        Assert.False(value.IsCompleted);
        Assert.Equal(1, value.Id);

        await repositoryMock.Received(1).Create(Arg.Any<ToDoItem>());

    }

    [Fact]
    public async Task Post_CreateInvalidRequest_ReturnsBadRequest()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);
        var createRequest = new ToDoItemCreateRequestDto(null!, "Popis bez jména", false, null);

        // Act
        var result = await controller.Create(createRequest);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
        Assert.Equal("Name cannot be null or empty.", badRequestResult.Value);

        //ověřuje, že controller nepokračoval k repository
        await repositoryMock.DidNotReceive().Create(Arg.Any<ToDoItem>());
    }
}
