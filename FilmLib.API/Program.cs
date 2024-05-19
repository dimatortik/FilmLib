using System.Reflection;
using FilmLib.Application.FilmActions.Commands.CreateFilm;
using FilmLib.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(configuration.GetConnectionString("FilmDB"));
});

// register dbcontext to use in the application assembly
// builder.Services.AddScoped<AppDbContext>(provider => provider.GetRequiredService<AppDbContext>());

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(typeof(CreateFilmCommand).GetTypeInfo().Assembly, 
        typeof(Program).GetTypeInfo().Assembly);
});

builder.Services.AddControllers().AddApplicationPart(typeof(Program).Assembly);
    
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHsts();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();




app.Run();

