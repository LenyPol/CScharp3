using System.Reflection;
using Microsoft.EntityFrameworkCore;
using ToDoList.Domain.Models;
using ToDoList.Persistence;
using ToDoList.WebApi;

namespace ToDoList.Test.IntegrationTests;

public abstract class ToDoListControllerTestBase : IDisposable
{
    protected readonly ToDoItemsContext DbContext;
    protected readonly ToDoItemsController Controller;


    protected ToDoListControllerTestBase()
    {
        var options = new DbContextOptionsBuilder<ToDoItemsContext>()
            .UseSqlite("Data Source=../../../IntegrationTests/data/localdb_test.db")
            .Options;

        DbContext = new TestToDoItemsContext(options);

        DbContext.Database.EnsureCreated();

        Controller = new ToDoItemsController(DbContext);
    }

    public void Dispose()
    {
        try
        {
            DbContext.ToDoItems.RemoveRange(DbContext.ToDoItems);
            DbContext.SaveChanges();
        }
        catch
        {

        }
        DbContext.Dispose();
        GC.SuppressFinalize(this);
    }
}

