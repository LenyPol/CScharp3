using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.WebApi;

namespace ToDoList.Test;

public class UpdateTests
{
    [Fact]
    public void Update_ExistingItem_ReturnsNoContent_UpdateItem()
    {
        //Arrange
        var todoItem = new ToDoItem
        {
            ToDoItemId = 1,
            Name = "Jmeno1",
            Description = "Popis1",
            IsCompleted = false
        };

        var controller = new ToDoItemsController();
        controller.AddItemToStorage(todoItem);

        var updateRequest = new ToDoItemUpdateRequestDto("Jmeno2", "Popis2", true);
        // Act
        var result = controller.UpdateById(1, updateRequest);

        // Assert
        Assert.IsType<NoContentResult>(result);

        var readResult = controller.ReadById(1) as OkObjectResult;
        Assert.NotNull(readResult);

        var value = readResult!.Value as ToDoItemGetResponseDto;
        Assert.NotNull(value);

        Assert.Equal("Jmeno2", value!.Name);
        Assert.Equal("Popis2", value.Description);
        Assert.True(value.IsCompleted);
    }
    [Fact]
    public void Update_NonExistingItem_ReturnsNotFound()
    {
        // Arrange
        var controller = new ToDoItemsController();

        var updateRequest = new ToDoItemUpdateRequestDto("NonExisting", "Attempt to update", false);

        // Act
        var result = controller.UpdateById(999, updateRequest);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
