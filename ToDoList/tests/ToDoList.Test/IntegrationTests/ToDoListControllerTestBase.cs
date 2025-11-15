using System.Reflection;
using Microsoft.EntityFrameworkCore;
using ToDoList.Domain.Models;
using ToDoList.Persistence;
using ToDoList.Persistence.Repositories;
using ToDoList.WebApi;

namespace ToDoList.Test.IntegrationTests;
/// <summary>
/// Základní třída pro integrační testy – nastavuje izolovanou SQLite in-memory databázi.
/// </summary>
public abstract class ToDoListControllerTestBase : IDisposable
{
    protected readonly ToDoItemsContext DbContext;
    protected readonly ToDoItemsController Controller;
    protected readonly IRepository<ToDoItem> Repository;


    protected ToDoListControllerTestBase()
    {
        var options = new DbContextOptionsBuilder<ToDoItemsContext>()
            .UseSqlite("DataSource=:memory:")
            .Options;

        DbContext = new ToDoItemsContext(options);
        DbContext.Database.OpenConnection();
        DbContext.Database.EnsureCreated();

        DbContext.ToDoItems.RemoveRange(DbContext.ToDoItems);
        DbContext.SaveChanges();
        DbContext.Database.ExecuteSqlRaw("DELETE FROM sqlite_sequence WHERE name='ToDoItems';");

        Repository = new ToDoItemsRepository(DbContext);
        Controller = new ToDoItemsController(Repository);
    }

    public void Dispose()
    {
        try
        {
            DbContext.ToDoItems.RemoveRange(DbContext.ToDoItems);
            DbContext.SaveChanges();
            DbContext.Database.ExecuteSqlRaw("DELETE FROM sqlite_sequence WHERE name='ToDoItems';");
        }
        catch
        {

        }
        DbContext.Dispose();
        GC.SuppressFinalize(this);
    }
}

