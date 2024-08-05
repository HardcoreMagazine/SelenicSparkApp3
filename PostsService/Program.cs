using Generics.Models;
using Microsoft.EntityFrameworkCore;
using PostsService.Data;
using PostsService.Models.Data;
using PostsService.Service;


//TODO?: https://learn.microsoft.com/en-us/aspnet/core/performance/rate-limit?view=aspnetcore-8.0

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("Default"));
});

// don't put this before adding DbContext service, otherwise might cause errors

// note to self: AddScoped: we are telling .net that if someone needs IRepository<Post> instance -
// .net must create object PostManager to satisfy this requirement ("""dependency""")
builder.Services.AddScoped<IRepository<Post>, PostManager>();

// CORS policy name (could be any)
var crossProjectAccess = "_crossProjectAccess";

builder.Services.AddCors(opt =>
{
    opt.AddPolicy(name: crossProjectAccess, policy =>
    {
        //policy.AllowAnyOrigin();
        policy.WithOrigins(
            "http://localhost:5173", // bad practice, this should be moved in appsettings.json or changed completely
            "https://localhost:46801"
        )
        //.AllowCredentials()
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});

var app = builder.Build();

//Configure the HTTP request pipeline.
/*if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}*/

app.UseHttpsRedirection();

app.UseCors(crossProjectAccess);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
