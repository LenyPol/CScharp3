var builder = WebApplication.CreateBuilder(args);
{
    //Configure DI dependency Injection
    builder.Services.AddControllers();
}
var app = builder.Build();
{
    // Configure Middlewre (Http request pipeline)
    app.MapControllers();
}

app.Run();
