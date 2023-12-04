using AutoMapper;
using Microsoft.Extensions.Logging.Console;
using Work.ApiModels;
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

// Read configuration
#if DEBUG
builder.Configuration.AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true);
#endif

logger.LogInformation("*** Application started");

// Configure logging providers
SetupLogging(builder);

// Add services to the container.
SetupDependencyInjection(builder);
logger.LogInformation("*** DI initialized.");

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

void SetupLogging(WebApplicationBuilder b)
{

    b.Logging.ClearProviders();
    b.Logging.AddConsole(r =>
    {
        r.TimestampFormat = "yyyy.MM.dd HH:mm:ss";
        r.IncludeScopes = true;
        r.UseUtcTimestamp = true;
    });
    b.Logging.AddConsole();
    b.Logging.AddDebug();
}

void SetupDependencyInjection(WebApplicationBuilder b)
{
    var connectionString = builder.Configuration["ConnectionStrings:DbConnectionString"];
    if (String.IsNullOrWhiteSpace(connectionString))
        throw new ApplicationException("Missing connection string");
    var dbConfig = new DbConfig(connectionString);

    b.Services.AddSingleton(dbConfig);
    b.Services.AddScoped<IService<UserVm, Guid>, UserService>();
    b.Services.AddScoped<IRepository<UserDto, Guid>, UserRepository>();
    b.Services.AddSingleton<IMapperWithValidation<UserDto, UserVm>, MapperWithValidation<UserDto, UserVm>>();

    var mapperConfig = new MapperConfiguration(mc =>
    {
        mc.AddProfile(new MappingProfile());
    });

    IMapper mapper = mapperConfig.CreateMapper();
    b.Services.AddSingleton(mapper);
}