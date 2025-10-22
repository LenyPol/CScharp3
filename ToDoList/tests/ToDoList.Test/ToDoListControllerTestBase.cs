using System.Reflection;
using ToDoList.Domain.Models;
using ToDoList.WebApi;

namespace ToDoList.Test;

public abstract class ToDoListControllerTestBase
{
    protected ToDoItemsController Controller { get; }

    protected ToDoListControllerTestBase()
    {
        Controller = new ToDoItemsController();
    }

    protected void AddItemToStorage(ToDoItem item)
    {
        var field = typeof(ToDoItemsController)
            .GetField("items", BindingFlags.NonPublic | BindingFlags.Instance)!;
        var items = (List<ToDoItem>)field.GetValue(Controller)!;

        item.ToDoItemId = item.ToDoItemId == 0
            ? (items.Count == 0 ? 1 : items.Max(o => o.ToDoItemId) + 1)
            : item.ToDoItemId;

        items.Add(item);
    }
}

