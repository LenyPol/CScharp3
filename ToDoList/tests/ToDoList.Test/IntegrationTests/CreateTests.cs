using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.WebApi;
using System.Reflection;

namespace ToDoList.Test.IntegrationTests;

[Collection("Sequential")]
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

        var value = Assert.IsType<ToDoItemGetResponseDto>(result.Value);

        Assert.Equal("Jmeno1", value!.Name);
        Assert.Equal("Popis1", value.Description);
        Assert.False(value.IsCompleted);
        Assert.True(value.Id > 0);

        var entity = DbContext.ToDoItems.SingleOrDefault(i => i.ToDoItemId == value.Id);
        Assert.NotNull(entity);
        Assert.Equal(value.Name, entity!.Name);
        Assert.Equal(value.Description, entity.Description);
        Assert.False(entity.IsCompleted);

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

        var allItems = DbContext.ToDoItems.OrderBy(i => i.ToDoItemId).ToList();
        Assert.Equal(2, allItems.Count);
        Assert.True(allItems[1].ToDoItemId > allItems[0].ToDoItemId);
    }

    [Fact]
    public void Create_WhenExceptionOccurs_ReturnsProblem()
    {
        // Arrange
        var createRequest = new ToDoItemCreateRequestDto(null!, "Popis bez jména", false);

        // Act
        var result = Controller.Create(createRequest);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
        Assert.Equal("Name cannot be null or empty.", badRequestResult.Value);
        Assert.Empty(DbContext.ToDoItems);
    }
}
