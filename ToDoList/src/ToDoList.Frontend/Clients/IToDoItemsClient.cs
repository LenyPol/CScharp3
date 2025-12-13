namespace ToDoList.Frontend.Clients;

using ToDoList.Frontend.Models;

public interface IToDoItemsClient
{
    public Task<List<ToDoItemView>> ReadItemsAsync();

    public Task<ToDoItemView?> ReadItemByIdAsync(int itemId);

    public Task UpdateItemAsync(ToDoItemView item);

    public Task<ToDoItemView?> DeleteItemAsync(int itemId);

    public Task CreateItemAsync(ToDoItemView item);
}


