using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.Models;
using ToDoList.WebApi;
using System.Reflection;

namespace ToDoList.Test;

public class DeleteTests : ToDoListControllerTestBase
{

    [Fact]
    public void Delete_ExistingItem_ReturnsNoContent()
    {
        //Arrange
        var todoItem = new ToDoItem
        {
            ToDoItemId = 1,
            Name = "Jmeno1",
            Description = "Popis1",
            IsCompleted = false
        };

        AddItemToStorage(todoItem);

        // Act
        var result = Controller.DeleteById(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public void Delete_NonExistingItem_ReturnsNotFound()
    {
        // Arrange
        var controller = new ToDoItemsController();

        // Act
        var result = controller.DeleteById(999);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void Delete_RemovesItemFromStorage()
    {
        // Arrange
        var todoItem = new ToDoItem
        {
            ToDoItemId = 10,
            Name = "Jmeno10",
            Description = "Popis10",
            IsCompleted = false
        };

        AddItemToStorage(todoItem);

        // Act
        Controller.DeleteById(10);

        var result = Controller.Read();
        var value = result.GetValue();

        // Assert
        Assert.NotNull(value);
        Assert.DoesNotContain(value, i => i.Id == 10);
    }
}
