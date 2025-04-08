var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddControllers();
var app = builder.Build();

app.MapOpenApi("/openapi.json");

app.MapControllers();
app.UseHttpsRedirection();
app.Run();