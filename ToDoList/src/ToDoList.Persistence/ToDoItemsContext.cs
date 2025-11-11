namespace ToDoList.Persistence;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ToDoList.Domain.Models;

public class ToDoItemsContext : DbContext
{
    private readonly string? connectionString;
    public ToDoItemsContext(string connectionString = "Data Source=../../data/localdb.db")
    {
        this.connectionString = connectionString;
        this.Database.Migrate();
    }
    public DbSet<ToDoItem> ToDoItems { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured && connectionString is not null)
        {
            optionsBuilder.UseSqlite(connectionString);
        }
    }
    public ToDoItemsContext(DbContextOptions<ToDoItemsContext> options)
        : base(options)
    {
    }
}
