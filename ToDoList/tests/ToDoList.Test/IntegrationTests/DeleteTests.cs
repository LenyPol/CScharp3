using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.Models;
using ToDoList.WebApi;
using System.Reflection;

namespace ToDoList.Test.IntegrationTests;

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

        DbContext.ToDoItems.Add(todoItem);
        DbContext.SaveChanges();

        // Act
        var result = Controller.DeleteById(todoItem.ToDoItemId);

        // Assert
        Assert.IsType<NoContentResult>(result);

        var deleted = DbContext.ToDoItems.SingleOrDefault(i => i.ToDoItemId == todoItem.ToDoItemId);
        Assert.Null(deleted);
    }

    [Fact]
    public void Delete_NonExistingItem_ReturnsNotFound()
    {
        // Act
        var result = Controller.DeleteById(999);

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

        DbContext.ToDoItems.Add(todoItem);
        DbContext.SaveChanges();

        // Act
        Controller.DeleteById(todoItem.ToDoItemId);

        // Assert
        var items = DbContext.ToDoItems.ToList();
        Assert.DoesNotContain(items, i => i.ToDoItemId == todoItem.ToDoItemId);
    }
}
