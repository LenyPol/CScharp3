using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.WebApi;
using System.Reflection;

namespace ToDoList.Test.IntegrationTests;

public class UpdateTests : ToDoListControllerTestBase
{

    [Fact]
    public void Update_ExistingItem_ReturnsNoContent_UpdateItem()
    {
        //Arrange
        var todoItem = new ToDoItem
        {
            Name = "Jmeno1",
            Description = "Popis1",
            IsCompleted = false
        };

        DbContext.ToDoItems.Add(todoItem);
        DbContext.SaveChanges();
        var id = todoItem.ToDoItemId;

        var updateRequest = new ToDoItemUpdateRequestDto("Jmeno2", "Popis2", true, "Category2");

        // Act
        var result = Controller.UpdateById(id, updateRequest);

        // Assert
        Assert.IsType<NoContentResult>(result);

        var updateItem = DbContext.ToDoItems.Find(todoItem.ToDoItemId);
        Assert.NotNull(updateItem);
        Assert.Equal("Jmeno2", updateItem!.Name);
        Assert.Equal("Popis2", updateItem.Description);
        Assert.Equal("Category2", updateItem.Category);
        Assert.True(updateItem.IsCompleted);
    }
    [Fact]
    public void Update_NonExistingItem_ReturnsNotFound()
    {
        // Arrange
        var updateRequest = new ToDoItemUpdateRequestDto("NonExisting", "Attempt to update", false, null);

        // Act
        var result = Controller.UpdateById(999, updateRequest);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
