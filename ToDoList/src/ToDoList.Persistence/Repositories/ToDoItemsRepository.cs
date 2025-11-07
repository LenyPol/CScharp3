namespace ToDoList.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;
using ToDoList.Domain.Models;

public class ToDoItemsRepository(ToDoItemsContext dbContext) : IRepository<ToDoItem>
{
    public void Create(ToDoItem item)
    {
        dbContext.ToDoItems.Add(item);
        dbContext.SaveChanges();
    }
    public IEnumerable<ToDoItem> ReadAll()
    {
        return dbContext.ToDoItems
            .AsNoTracking()
            .ToList();
    }
    public ToDoItem? ReadById(int id)
    {
        return dbContext.ToDoItems
            .SingleOrDefault(i => i.ToDoItemId == id);
    }
    public void Update(ToDoItem item)
    {
        var existing = dbContext.ToDoItems.Find(item.ToDoItemId);
        if (existing is null)
            return;

        dbContext.Entry(existing).CurrentValues.SetValues(item);
        dbContext.SaveChanges();
        dbContext.ChangeTracker.Clear();
    }
    public void Delete(ToDoItem item)
    {
        var existing = dbContext.ToDoItems.Find(item.ToDoItemId);
        if (existing != null)
        {
            dbContext.ToDoItems.Remove(existing);
            dbContext.SaveChanges();
            dbContext.ChangeTracker.Clear();
        }
    }
}

