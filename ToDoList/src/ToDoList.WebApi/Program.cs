var builder = WebApplication.CreateBuilder(args);
{
    //Configure DI dependency Injection
    builder.Services.AddControllers();
}
var app = builder.Build();
<<<<<<< HEAD
{
    // Configure Middlewre (Http request pipeline)
    app.MapControllers();
}
=======

app.MapGet("/", () => "Hello World!");
app.MapGet("/test", () => "This is a test!");
app.MapGet("/czechitas", () => "Vítej na kurzu Czechitas!");
app.MapGet("/pozdrav/{jmeno}", (string jmeno) => $"Ahoj {jmeno}!");
app.MapGet("/secti/{a}/{b}", (int a, int b) => $"Vysledek {a} + {b} = {a + b}");
app.MapGet("/nazdarSvete", () => "Nazdar světe!");
>>>>>>> ed7fc6fc23bbc92cd7bbd1c31da4757366c7dce1

app.Run();
