using System.Reflection;
using FilmLib.API.Extensions;
using FilmLib.Application.Films.Commands.Create;
using FilmLib.Persistence;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var services = builder.Services;

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(configuration.GetConnectionString("FilmDB"));
});

services.AddCloudStorage(configuration);

services.AddDistributedMemoryCache();
services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromDays(1);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.WebHost.ConfigureKestrel(options => options.Limits.MaxRequestBodySize = 1 * 1024 * 1024 * 1024);

services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(typeof(CreateFilmCommand).GetTypeInfo().Assembly, 
        typeof(Program).GetTypeInfo().Assembly);
});

services.AddCors(options =>
{
    options.AddPolicy("AllowAll",policyBuilder =>
    {
        policyBuilder
            .WithOrigins("http://localhost:4200")
            .WithExposedHeaders("Set-Cookie")
            .SetIsOriginAllowedToAllowWildcardSubdomains()
            .AllowCredentials()
            .AllowAnyMethod()
            .AllowAnyHeader();
    
    });
});

services.AddCustomAuthentication(configuration);


services.AddControllers().AddApplicationPart(typeof(Program).Assembly);
    
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
app.UseCors("AllowAll");
app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.None,
    HttpOnly = HttpOnlyPolicy.None,
    Secure = CookieSecurePolicy.Always
});
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();



app.Run();

