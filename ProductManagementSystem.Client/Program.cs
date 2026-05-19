using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ProductManagementSystem.Client;
using ProductManagementSystem.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Конфигурация HTTP клиента для подключения к API серверу
var apiBaseAddress = builder.Configuration["API:BaseAddress"] ?? "http://localhost:5280";

builder.Services.AddScoped(sp => new HttpClient 
{ 
    BaseAddress = new Uri(apiBaseAddress) 
});

// Добавляем сервис для управления темой
builder.Services.AddScoped<ThemeService>();

// Добавляем сервис для работы с товарами
builder.Services.AddScoped<ProductService>();

await builder.Build().RunAsync();
