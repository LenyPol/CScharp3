using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.WebApi;
using System.Reflection;

namespace ToDoList.Test.IntegrationTests;

public class CreateTests : ToDoListControllerTestBase
{

    [Fact]
    public void Create_ValidItem_ReturnCreatedItem()
    {
        //Arrange
        var createRequest = new ToDoItemCreateRequestDto("Jmeno1", "Popis1", false);

        // Act
        var result = Controller.Create(createRequest) as CreatedAtActionResult;
        // Assert
        Assert.NotNull(result);
        Assert.Equal(nameof(ToDoItemsController.ReadById), result!.ActionName);

        var value = result.Value as ToDoItemGetResponseDto;
        Assert.NotNull(value);

        Assert.Equal("Jmeno1", value!.Name);
        Assert.Equal("Popis1", value.Description);
        Assert.False(value.IsCompleted);
        Assert.True(value.Id > 0);

    }
    [Fact]
    public void Create_MultipleItems_AssignsIncrementalIds()
    {
        // Arrange
        var item1 = new ToDoItemCreateRequestDto("Jmeno1", "Popis1", false);
        var item2 = new ToDoItemCreateRequestDto("Jmeno2", "Popis2", true);

        // Act
        var result1 = Controller.Create(item1) as CreatedAtActionResult;
        var result2 = Controller.Create(item2) as CreatedAtActionResult;

        // Assert
        var value1 = result1!.Value as ToDoItemGetResponseDto;
        var value2 = result2!.Value as ToDoItemGetResponseDto;

        Assert.NotNull(value1);
        Assert.NotNull(value2);
        Assert.True(value2!.Id > value1!.Id);
    }

    [Fact]
    public void Create_WhenExceptionOccurs_ReturnsProblem()
    {
        // Arrange
        var createRequest = new ToDoItemCreateRequestDto(null!, "Popis bez jména", false);

        // Act
        var result = Controller.Create(createRequest);

        // Assert
        var problemResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, problemResult.StatusCode);
    }
}
