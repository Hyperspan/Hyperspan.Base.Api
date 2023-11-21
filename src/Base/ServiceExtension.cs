global using System;
global using System.Collections.Generic;
global using System.Threading.Tasks;
global using System.Threading;
global using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using RamsonDevelopers.UtilEmail;
using Hyperspan.Base.Services;
using Hyperspan.Shared.Config;
using Hyperspan.Shared.Modals;

namespace Hyperspan.Base
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddBaseApi(this IServiceCollection services, IConfiguration config)
        {
            var appConfiguration = config.GetSection(AppConfiguration.Label);
            var emailConfig = config.GetSection(EmailConfig.SectionLabel);

            services.AddOptions();
            services.AddControllers();
            services.Configure<AppConfiguration>(appConfiguration);
            services.Configure<EmailConfig>(emailConfig);
            services.AddEmailService();
            services.AddBaseServices();
            services.AddJwtAuthentication(appConfiguration.Get<AppConfiguration>());
            services.AddCors(cors =>
            {
                cors.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            return services;
        }


        internal static IServiceCollection AddJwtAuthentication(
            this IServiceCollection serviceCollection,
            AppConfiguration appConfig)
        {
            serviceCollection
                .AddAuthentication(authentication =>
                {
                    authentication.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    authentication.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(bearer =>
                {
                    bearer.RequireHttpsMetadata = false;
                    bearer.SaveToken = true;

                    bearer.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appConfig.JwtSecurityKey)),
                        ValidateIssuerSigningKey = true,
                        RoleClaimType = ClaimTypes.Role,
                    };
                    bearer.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = c =>
                        {
                            if (c.Exception is SecurityTokenExpiredException)
                            {
                                c.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                                c.Response.ContentType = "application/json";
                                var result = JsonSerializer.Serialize(
                                    ApiResponseModal<object>.FailedAsync("The Token is expired."));
                                return c.Response.WriteAsync(result);
                            }
                            c.NoResult();
                            c.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            c.Response.ContentType = "text/plain";
                            return c.Response.WriteAsync(c.Exception.ToString());
                        },
                        OnChallenge = context =>
                        {
                            context.HandleResponse();
                            context.Response.StatusCode = 401;
                            context.Response.ContentType = "application/json";
                            var result = JsonSerializer.Serialize(
                                ApiResponseModal<object>.FailedAsync("You have been logged out! Please login again."));
                            return context.Response.WriteAsync(result);
                        },
                        OnForbidden = context =>
                        {
                            context.Response.StatusCode = 403;
                            context.Response.ContentType = "application/json";
                            var result = JsonSerializer.Serialize(
                                ApiResponseModal<object>.FailedAsync("You are not authorized to access this resource."));
                            return context.Response.WriteAsync(result);
                        },
                    };
                });

            serviceCollection.AddHttpContextAccessor();
            return serviceCollection;
        }

    }
}
