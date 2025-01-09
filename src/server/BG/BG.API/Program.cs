using Autofac.Extensions.DependencyInjection;
using BG.API.Extensions;
using BG.API.Extensions.Documentation;
using BG.API.Middleware;
using BG.Infrastructure.Data;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Context;
using System.IO.Compression;

var builder = WebApplication.CreateBuilder(args);

const string corsPolicy = "BgCorsPolicy";

var safeOrigins = builder.Configuration.GetSection("AllowedCorsOrigins").Get<string>().Split(",")
                  ?? throw new ArgumentException("AllowedCorsOrigins configuration is missing.");

var isDevelopment = builder.Environment.IsDevelopment();

// Add services to the container.

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddDbContextPool<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
                x => x.MigrationsAssembly("BG.Infrastructure")) //Migrations in infrastructure library
            .EnableSensitiveDataLogging(isDevelopment) // Enable only in Development
            .EnableDetailedErrors(isDevelopment)
            .LogTo(Log.Information, LogLevel.Information) // Log EF Core queries using Serilog
);


builder.Services.AddApplicationServices(builder.Configuration);

builder.Services.AddIdentityService(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicy, policy =>
    {
        policy.WithOrigins(safeOrigins)
            .SetIsOriginAllowed(_ => true)
            .SetIsOriginAllowedToAllowWildcardSubdomains()
            .AllowCredentials()
            .AllowAnyHeader()
            .WithMethods("GET", "POST", "PUT", "DELETE")
            .WithExposedHeaders("Content-Disposition")
            .SetPreflightMaxAge(TimeSpan.FromSeconds(3600));
    });
});

builder.Services.AddSwaggerDocumentation();

builder.Services.AddVersioningAndSwagger();

builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
});

builder.Services.Configure<BrotliCompressionProviderOptions>(options => { options.Level = CompressionLevel.Fastest; });
builder.Services.Configure<GzipCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.SmallestSize;
});

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerAndVersioning();
    app.MapOpenApi();
}

app.UseResponseCompression();

app.UseMiddleware<ExceptionMiddleware>();

//Middleware to validate the client id
//app.UseMiddleware<ClientIdValidationMiddleware>();

app.UseDefaultFiles();

app.UseStaticFiles();

app.UseCors(corsPolicy);

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseSerilogRequestLogging(options =>
{
    options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
});

// Middleware to enrich logs with custom properties
app.Use(async (context, next) =>
{
    using (LogContext.PushProperty("UserId", context.User?.Identity?.Name ?? "Anonymous"))
    {
        await next();
    }
});


app.MapControllers().RequireAuthorization();

app.MapFallbackToFile("/index.html");

//Insert seed data on application starts

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var context = services.GetRequiredService<ApplicationDbContext>();
var logger = services.GetRequiredService<ILogger<Program>>();
try
{
    await context.Database.MigrateAsync();
    await ApplicationDbContextSeed.SeedAsync(context);
}
catch (Exception ex)
{
    logger.LogError(ex, "An error occured during migration");
}

await app.RunAsync();