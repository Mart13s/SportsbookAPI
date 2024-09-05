using SportsbookAPI.Adapters;
using SportsbookAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Services
builder.Services.AddSingleton<IEventAdapter>(provider => new EventJsonAdapter("./Data/Events.json"));
builder.Services.AddSingleton<IPlayerAdapter>(provider => new PlayerJsonAdapter("./Data/Players.json"));
builder.Services.AddSingleton<IBetAdapter>(provider => new BetJsonAdapter("./Data/Bets.json"));
builder.Services.AddSingleton<IBetLogger>(provider => new BetLogger("./Data/BetsLog.json"));
builder.Services.AddScoped<IBetService, BetService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
