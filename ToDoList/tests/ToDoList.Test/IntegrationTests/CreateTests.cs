using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.WebApi;
using System.Reflection;
using System.Threading.Tasks;

namespace ToDoList.Test.IntegrationTests;

[Collection("Sequential")]
public class CreateTests : ToDoListControllerTestBase
{

    [Fact]
    public async Task Create_ValidItem_ReturnCreatedItem()
    {
        //Arrange
        var createRequest = new ToDoItemCreateRequestDto("Jmeno1", "Popis1", false, "Category1");

        // Act
        var result = await Controller.Create(createRequest) as CreatedAtActionResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(nameof(ToDoItemsController.ReadById), result!.ActionName);

        var value = Assert.IsType<ToDoItemGetResponseDto>(result.Value);

        Assert.Equal("Jmeno1", value!.Name);
        Assert.Equal("Popis1", value.Description);
        Assert.Equal("Category1", value.Category);
        Assert.False(value.IsCompleted);
        Assert.True(value.Id > 0);

        var entity = DbContext.ToDoItems.SingleOrDefault(i => i.ToDoItemId == value.Id);
        Assert.NotNull(entity);
        Assert.Equal(value.Name, entity!.Name);
        Assert.Equal(value.Description, entity.Description);
        Assert.Equal(value.Category, entity.Category);
        Assert.False(entity.IsCompleted);

    }
    [Fact]
    public async Task Create_MultipleItems_AssignsIncrementalIds()
    {
        // Arrange
        var item1 = new ToDoItemCreateRequestDto("Jmeno1", "Popis1", false, "Category1");
        var item2 = new ToDoItemCreateRequestDto("Jmeno2", "Popis2", true, "Category2");

        // Act
        var result1 = await Controller.Create(item1) as CreatedAtActionResult;
        var result2 = await Controller.Create(item2) as CreatedAtActionResult;

        // Assert
        Assert.NotNull(result1);
        Assert.NotNull(result2);
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
    public async Task Create_WhenExceptionOccurs_ReturnsProblem()
    {
        // Arrange
        var createRequest = new ToDoItemCreateRequestDto(null!, "Popis bez jména", false, null);

        // Act
        var result = await Controller.Create(createRequest);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
        Assert.Equal("Name cannot be null or empty.", badRequestResult.Value);
        Assert.Empty(DbContext.ToDoItems);
    }
}
