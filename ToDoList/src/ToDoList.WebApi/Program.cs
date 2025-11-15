using Microsoft.EntityFrameworkCore;
using ToDoList.Domain.Models;
using ToDoList.Persistence;
using ToDoList.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);
{
    //Configure DI dependency Injection
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddDbContext<ToDoItemsContext>(options =>
    {
        options.UseSqlite("Data Source=../../data/localdb.db");
    });

    builder.Services.AddScoped<IRepository<ToDoItem>, ToDoItemsRepository>();
}
var app = builder.Build();
{
    // Configure Middleware (HTTP request pipeline)
    app.MapControllers();
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHttpsRedirection();
}
app.Run();
