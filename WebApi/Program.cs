using DataAccess.Dbcontexts;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Service.Interfaces;
using Service.Implementations;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    // options.UseSqlite(
    //     builder.Configuration.GetConnectionString("SqliteConnection")
    // );
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("SqlServerConnection")
    );
});
builder.Services.AddCors();
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedHost;
    options.KnownIPNetworks.Clear();
    options.KnownProxies.Clear();
});
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IHealthCheckService, HealthCheckService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
        options => builder.Configuration.Bind("JwtSettings", options))
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
        options => builder.Configuration.Bind("CookieSettings", options));

var app = builder.Build();

app.UseForwardedHeaders();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "v1"));
}

app.UseCors(builder => builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
);

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

// writing Iconfigufation class to console and see if confirguation file are ready to IO

Console.WriteLine(builder.Configuration["JwtSettings:Secret"] ?? "Not Found");
Console.WriteLine(builder.Configuration["JwtSettings:Issuer"] ?? "Not Found");
Console.WriteLine(builder.Configuration["JwtSettings:Audience"] ?? "Not Found");
Console.WriteLine(builder.Configuration["CookieSettings:LoginPath"] ?? "Not Found");
Console.WriteLine(builder.Configuration["CookieSettings:LogoutPath"] ?? "Not Found");
Console.WriteLine(builder.Configuration["CookieSettings:ExpireTime"] ?? "Not Found");
