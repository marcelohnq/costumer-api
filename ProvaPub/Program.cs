using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Converters;
using ProvaPub.Repository;
using ProvaPub.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.Converters.Add(new StringEnumConverter()));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGenNewtonsoftSupport();

builder.Services.AddSingleton<RandomService>();
builder.Services.AddDbContext<TestDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ctx")));

builder.Services.AddScoped(typeof(IGenericDbContext<>), typeof(GenericDbContext<>));
builder.Services.AddScoped(typeof(CustomerService));
builder.Services.AddScoped(typeof(OrderService));

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

public partial class Program { }