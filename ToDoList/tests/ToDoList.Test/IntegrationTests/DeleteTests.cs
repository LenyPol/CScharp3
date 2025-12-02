using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.Models;
using ToDoList.WebApi;


namespace ToDoList.Test.IntegrationTests;

public class DeleteTests : ToDoListControllerTestBase
{

    [Fact]
    public async Task Delete_ExistingItem_ReturnsNoContent()
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

        // Act
        var result = await Controller.DeleteById(todoItem.ToDoItemId);

        // Assert
        Assert.IsType<NoContentResult>(result);

        var deleted = DbContext.ToDoItems.SingleOrDefault(i => i.ToDoItemId == todoItem.ToDoItemId);
        Assert.Null(deleted);
    }

    [Fact]
    public async Task Delete_NonExistingItem_ReturnsNotFound()
    {
        // Act
        var result = await Controller.DeleteById(999);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Delete_RemovesItemFromStorage()
    {
        // Arrange
        var todoItem = new ToDoItem
        {
            Name = "Jmeno10",
            Description = "Popis10",
            IsCompleted = false
        };

        DbContext.ToDoItems.Add(todoItem);
        DbContext.SaveChanges();

        // Act
        await Controller.DeleteById(todoItem.ToDoItemId);

        // Assert
        var items = DbContext.ToDoItems.ToList();
        Assert.DoesNotContain(items, i => i.ToDoItemId == todoItem.ToDoItemId);
    }
}
