
namespace ToDoList.Frontend.Clients;

using ToDoList.Domain.DTOs;
using ToDoList.Frontend.Models;

public class ToDoItemsClient(HttpClient httpClient) : IToDoItemsClient
{
    private readonly HttpClient httpClient = httpClient;

    public async Task<List<ToDoItemView>> ReadItemsAsync()
    {
        var toDoItemViews = new List<ToDoItemView>();
        var response = await httpClient.GetFromJsonAsync<List<ToDoItemGetResponseDto>>("api/ToDoItems")
                        ?? [];
        toDoItemViews = [.. response.Select(dto => new ToDoItemView()
        {
            Id = dto.Id,
            Name = dto.Name,
            Description = dto.Description,
            IsCompleted = dto.IsCompleted,
            Category = dto.Category
        })];

        return toDoItemViews;
    }

    public async Task<ToDoItemView?> ReadItemByIdAsync(int itemId)
    {
        var response = await httpClient.GetFromJsonAsync<ToDoItemGetResponseDto>($"api/ToDoItems/{itemId}");

        if (response is null)
        {
            return null;
        }

        var toDoItem = new ToDoItemView()
        {
            Id = response.Id,
            Name = response.Name,
            Description = response.Description,
            IsCompleted = response.IsCompleted,
            Category = response.Category
        };
        return toDoItem;
    }

    public async Task UpdateItemAsync(ToDoItemView item)
    {
        // try {}
        var itemRequest = new ToDoItemUpdateRequestDto(item.Name, item.Description, item.IsCompleted, item.Category);
        var response = await httpClient.PutAsJsonAsync($"api/ToDoItems/{item.Id}", itemRequest);
    }

    public async Task<ToDoItemView?> DeleteItemAsync(int itemId)
    {
        var response = await httpClient.DeleteAsync($"api/ToDoItems/{itemId}");

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        return new ToDoItemView { Id = itemId };
    }

    public async Task CreateItemAsync(ToDoItemView item)
    {
        var request = new ToDoItemCreateRequestDto(
            item.Name,
            item.Description,
            item.IsCompleted,
            item.Category
        );
        await httpClient.PostAsJsonAsync("api/ToDoItems", request);
    }
}




