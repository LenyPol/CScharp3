namespace ToDoList.Persistence;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ToDoList.Domain.Models;

public class ToDoItemsContext : DbContext
{
    public ToDoItemsContext(DbContextOptions<ToDoItemsContext> options)
        : base(options)
    {
    }

    public DbSet<ToDoItem> ToDoItems { get; set; }
}
