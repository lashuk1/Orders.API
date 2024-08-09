using MediatR;
using Microsoft.EntityFrameworkCore;
using Orders.Application.Commands;
using Orders.Infrastructure.Data;
using Orders.API.Hubs;
using Orders.Application;
using System.Reflection;
using Orders.Application.Middleware;
using Orders.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(5001);
});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Order API", Version = "v1" });
});

// Configure CORS to allow all origins, methods, and headers (for development purposes)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", corsBuilder =>
    {
        corsBuilder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// Register ApplicationDbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register MediatR - Ensure all assemblies containing MediatR handlers are included
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateOrderCommand).Assembly));

// Register OrderMatcher as a scoped service
builder.Services.AddScoped<OrderMatcher>();

// Register SignalR
builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Order API V1");
        c.RoutePrefix = string.Empty;  // Serve Swagger UI at the root
    });
    app.ApplyMigrations();
    // Use CORS policy
    app.UseCors("AllowAll");
}

app.UseHttpsRedirection();
app.UseAuthorization();

// Register the exception handling middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.MapControllers();
app.MapHub<OrderHub>("/orderHub");

app.Run();