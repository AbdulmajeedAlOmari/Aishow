using System.Reflection;
using Common.API.Clients.Extensions;
using Common.API.Clients.Interfaces;
using Common.API.Filters;
using Common.API.MessageBroker;
using Identity.API.Infrastructure.Data;
using Identity.API.Infrastructure.Helpers;
using Identity.API.Infrastructure.Middlewares;
using Identity.API.Infrastructure.Repositories;
using Identity.API.Infrastructure.Seeders;
using Identity.API.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Sentry;
using Sentry.Reflection;

// Initialize application builder
var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    ApplicationName = typeof(Program).Assembly.GetNameAndVersion().Name,
    ContentRootPath = Directory.GetCurrentDirectory(),
});

// Sentry integration
builder.WebHost.UseAishowSentry();

// Configuration files mapping
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!,
        "appsettings.Common.json"), optional: false)
    .AddJsonFile(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!,
        $"appsettings.Common.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json"), optional: false)
    .AddJsonFile("appsettings.json", optional: true)
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
    .AddEnvironmentVariables();

// Logging configurations
builder.Logging
    .AddJsonConsole()
    .AddSentry();

// add services to DI container
{
    var services = builder.Services;
    services.AddCors();
    services.AddControllers(options =>
    {
        // Add exception handler filter to all controllers
        options.Filters.Add(typeof(HttpCommonExceptionFilter));
    });

    // Register Database Context
    services.AddDbContext<IdentityDataContext>(options =>
    {
        string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
        options.UseNpgsql(connectionString);
    });

    services.AddAutoMapper(typeof(Program));

    // configure strongly typed settings object
    services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

    // configure DI for application services
    services.AddScoped<IUserService, UserService>();
    services.AddScoped<IUserRepository, UserRepository>();
    services.AddScoped<IRoleRepository, RoleRepository>();

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // Common package setup
    services.Configure<MessageBrokerOptions>(builder.Configuration.GetSection("MessageBroker"));
    services.AddSingleton<IMessageBroker, MessageBroker>();
    services.AddHttpClient<System.Net.Http.HttpClient>();
    services.AddSingleton<IServiceClient, Common.API.Clients.Http.HttpClient>();
}


var app = builder.Build();

app.UseSentryTracing();

// Allow timestamps to be saved in PostresSql
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Seed database with initial data
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetService<IdentityDataContext>();
        DataSeeder.Seed(context);
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// configure HTTP request pipeline
{
    // global cors policy
    app.UseCors(x => x
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());

    app.UseHttpsRedirection();

    // custom jwt auth middleware
    app.UseMiddleware<JwtMiddleware>();

    app.UseAuthorization();

    app.MapControllers();
}

SentrySdk.CaptureMessage("Identity Service Started");

app.Run();