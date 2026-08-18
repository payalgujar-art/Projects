using TicTacToe.API.Interfaces;
using TicTacToe.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
builder.Services.AddScoped<GameService>();
//builder.Services.AddHttpClient<IOllamaService, OllamaService>();
builder.Services.AddScoped<IMinimaxService, MinimaxService>();
builder.Services.AddScoped<IHybridMoveService, HybridMoveService>();
builder.Services.AddHttpClient<IAiMoveProvider, GroqService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseCors("AngularPolicy");

app.MapControllers();

app.Run();
