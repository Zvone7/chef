using AutoMapper;
using Microsoft.Extensions.Logging.Console;
using Work.Database;
using Work.Implementation;
using Work.Interfaces;
using Work.Mappings;

var builder = WebApplication.CreateBuilder(args);

var loggerFactory = LoggerFactory.Create(c =>
{
    c.AddConsole();
    c.AddDebug();
});

var logger = loggerFactory.CreateLogger<Program>();

logger.LogInformation("*** Application started");

builder.Logging.ClearProviders();
builder.Logging.AddConsole(r =>
{
    r.TimestampFormat = "yyyy.MM.dd HH:mm:ss";
    r.IncludeScopes = true;
    r.UseUtcTimestamp = true;
});
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Add services to the container.
builder.Services.AddSingleton(new MockDatabase(3));
builder.Services.AddScoped<IRepository<User, Guid>, UserRepository>();
var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MappingProfile());
});

logger.LogInformation("*** DI initialized.");

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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