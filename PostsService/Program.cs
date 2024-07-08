using Microsoft.EntityFrameworkCore;
using PostsService.Data;


//TODO: https://learn.microsoft.com/en-us/aspnet/core/performance/rate-limit?view=aspnetcore-8.0

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("Default"));
});

// CORS policy name (could be any)
var crossProjectAccess = "_crossProjectAccess";

builder.Services.AddCors(opt =>
{
    opt.AddPolicy(name: crossProjectAccess, policy =>
    {
        policy.WithOrigins(
            "http://localhost:5173",
            "https://localhost:46801"
        )
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});

var app = builder.Build();

//Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseCors(crossProjectAccess);

app.UseAuthorization();

app.MapControllers();

app.Run();
