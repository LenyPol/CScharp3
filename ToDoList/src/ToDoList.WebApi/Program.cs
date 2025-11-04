using ToDoList.Persistence;

var builder = WebApplication.CreateBuilder(args);
{
    //Configure DI dependency Injection
    builder.Services.AddControllers();
    builder.Services.AddDbContext<ToDoItemsContext>();
}
var app = builder.Build();
{
    // Configure Middleware (HTTP request pipeline)
    app.MapControllers();
    app.UseHttpsRedirection();
}
app.Run();
