var builder = WebApplication.CreateBuilder(args);

// Регистрируем HttpClient для взаимодействия с ServerApp
builder.Services.AddHttpClient("MyApiClient", client =>
{
    client.BaseAddress = new Uri("http://server:8080/");
});

builder.Services.AddControllers(); // Добавляем поддержку контроллеров
var app = builder.Build();

app.MapControllers(); // Настраиваем маршрутизацию для контроллеров
app.Run();