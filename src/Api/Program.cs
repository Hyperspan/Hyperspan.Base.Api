using System.Text.Json.Serialization;
using Api;
using Domain;
using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RamsonDevelopers.UtilEmail;
using Serilog;
using Shared;
using Shared.Config;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.File("Logs/Logs.txt", rollingInterval: RollingInterval.Day)
    .WriteTo.Console()
    .CreateLogger();

try
{
    builder.Services.AddControllers(options =>
        {
            options.CacheProfiles.Add("Default", new CacheProfile
            {
                Location = ResponseCacheLocation.None,
                NoStore = true
            });
        })
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        });
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();


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
        .AddRoles<ApplicationRole<Guid>>()
        .AddEntityFrameworkStores<Context>()
        .AddDefaultTokenProviders();

    // For using Authorization
    builder.Services.AddAuthorization();

    var connectionSection = builder.Configuration.GetSection(ConnectionString.Label);
    var connectionString = connectionSection.Get<ConnectionString>();
    builder.Services.Configure<ConnectionString>(connectionSection);

    var appConfigSection = builder.Configuration.GetSection(AppConfiguration.Label);
    var appConfig = appConfigSection.Get<AppConfiguration>();
    builder.Services.Configure<AppConfiguration>(appConfigSection);

    if (connectionString == null) throw new ApiErrorException(BaseErrorCodes.NullConnectionString);
        $if$($featureChoice$ == "Feature1")
    {
        builder.Services.AddMySqlDbService(connectionString.DbConnection);
    }
    var emailConfig = builder.Configuration.GetSection(EmailConfig.SectionLabel);
    builder.Services.Configure<EmailConfig>(emailConfig);
    builder.Services.AddEmailService();

    // For DI Base Api services.

    builder.Services.AddServices();

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("Production", policy =>
        {
            policy.WithOrigins("http://localhost:5173", "https://drive.ramson-developers.com/")
                .SetIsOriginAllowedToAllowWildcardSubdomains()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        });
    });

    builder.Services.Configure<FormOptions>(options =>
    {
        options.ValueLengthLimit = int.MaxValue;
        options.MultipartBodyLengthLimit = long.MaxValue;
        options.MultipartHeadersLengthLimit = int.MaxValue;
    });

    builder.Host.UseSerilog(Log.Logger);
    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    else
    {
        app.UseHttpsRedirection();
    }

    app.UseStaticFiles();
    app.UseResponseCaching();
    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();

    app.UseMiddleware<ErrorHandlerMiddleware>();
    app.UseCors("Production");
    app.MapControllers();
    app.MapFallbackToFile("index.html");
    app.UseSerilogRequestLogging();
    app.Run(appConfig?.LocalhostUrl ?? "http://localhost:5005");
}
catch (Exception ex)
{
    Log.Error(ex, "Exception");
}