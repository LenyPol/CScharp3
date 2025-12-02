namespace ToDoList.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;
using ToDoList.Domain.Models;

public class ToDoItemsRepository(ToDoItemsContext dbContext) : IRepositoryAsync<ToDoItem>
{
    public async Task Create(ToDoItem item)
    {
        dbContext.ToDoItems.Add(item);
        await dbContext.SaveChangesAsync();
    }
    public async Task<IEnumerable<ToDoItem>> ReadAll()
    {
        return await dbContext.ToDoItems
            .AsNoTracking()
            .ToListAsync();
    }
    public async Task<ToDoItem?> ReadById(int id)
    {
        return await dbContext.ToDoItems
            .AsNoTracking()
            .SingleOrDefaultAsync(i => i.ToDoItemId == id);
    }
    public async Task Update(ToDoItem item)
    {
        var existing = await dbContext.ToDoItems.FindAsync(item.ToDoItemId);
        if (existing is null)
            return;

        dbContext.Entry(existing).CurrentValues.SetValues(item);
        await dbContext.SaveChangesAsync();
    }
    public async Task Delete(ToDoItem item)
    {
        var existing = await dbContext.ToDoItems.FindAsync(item.ToDoItemId);
        if (existing != null)
        {
            dbContext.ToDoItems.Remove(existing);
            await dbContext.SaveChangesAsync();
        }
    }
}

