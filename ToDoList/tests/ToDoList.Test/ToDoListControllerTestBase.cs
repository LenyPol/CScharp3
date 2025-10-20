using System.Reflection;
using ToDoList.Domain.Models;
using ToDoList.WebApi;

namespace ToDoList.Test;

public abstract class ToDoListControllerTestBase
{
    protected readonly ToDoItemsController Controller;
    protected readonly List<ToDoItem> TestItems;

    protected ToDoListControllerTestBase()
    {
        // Vždy nový prázdný seznam pro každý test
        TestItems = new List<ToDoItem>();

        // Vytvoří kontroler s tímto testovacím seznamem
        Controller = new ToDoItemsController(TestItems);
    }
}
