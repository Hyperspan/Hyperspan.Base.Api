using Base.Api;
using Base.Api.Domain;
using Hyperspan.Auth.Domain.DatabaseModals;
using Hyperspan.Base;
using Hyperspan.Shared.Config;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// For using Authorization
builder.Services.AddAuthorization();

var connectionSection = builder.Configuration.GetSection(ConnectionString.Label);
var connectionString = connectionSection.Get<ConnectionString>();
builder.Services.Configure<ConnectionString>(connectionSection);

// For DI Base Api services.
builder.Services.AddBaseApi(builder.Configuration);
builder.Services.AddServices(connectionString.PgDatabase);

builder.Services.AddCors(corsBuilder =>
{
    corsBuilder.AddDefaultPolicy(_ =>
    {
        //TODO: Configure default policy.
    });
});


// Identity configuration
builder.Services
    .AddIdentity<ApplicationUser<Guid>, ApplicationRole<Guid>>(options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.User.RequireUniqueEmail = true;
    })

    //TODO: Add The appropriate Role class for roles
    .AddRoles<ApplicationRole<Guid>>()
    .AddEntityFrameworkStores<Context>()
    .AddDefaultTokenProviders();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

// if you have a index.html page in your wwwroot folder.
app.MapFallbackToFile("index.html");

app.Run();
