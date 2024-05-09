using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using SmartAccess.Locking.API.DataAccess;
using SmartAccess.Locking.API.Model;
using SmartAccess.Locking.API.Repositories;
using SmartAccess.Locking.API.Service;
using SmartAccess.Locking.API.Services;
using System.Net;
public static class Startup
{
    public static void ConfigureAppLogging(this WebApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();
        builder.Services.AddHttpLogging(logging =>
        {
            logging.LoggingFields = HttpLoggingFields.RequestPath | HttpLoggingFields.RequestMethod;
            logging.RequestBodyLogLimit = 4096;
            logging.ResponseBodyLogLimit = 4096;
        });
    }
    public static void ConfigureAppEndPoints(this WebApplicationBuilder builder)
    {
        builder.Services.AddMemoryCache();
        builder.WebHost.ConfigureKestrel(opts =>
        {
            opts.Listen(IPAddress.Loopback, port: 6000); // listen on http://localhost:6000
            opts.ListenAnyIP(6006); // listen on http://*:6006
            opts.ListenLocalhost(6001, listenOptions => listenOptions.UseHttps());  // listen on https://localhost:6001
        });
        builder.Services.AddCors(o => o.AddPolicy("SmartAccess", builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        }));

        builder.Services.AddControllers();
        builder.Services.AddHealthChecks();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Smart Access Locking API",
                Description = "An Smart Access Locking API",
            });
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer"
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
    }
    public static void ConfigureAppAuthentication(this WebApplicationBuilder builder)
    {
        var identityServerUrl = builder.Configuration.GetValue<string>("ServiceBaseUrl:IdentityServerUrl");
        builder.Services.AddSingleton(new ClientCredentialsTokenRequest
        {
            Address = $"{identityServerUrl}/connect/token",
            ClientId = "client",
            ClientSecret = "secret"
        });

        // Add services to the container.
        builder.Services.AddAuthentication("Bearer").AddJwtBearer(x =>
        {
            x.Authority = identityServerUrl;
            x.RequireHttpsMetadata = false;
            x.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidAudiences = ["locking"]
            };
            x.Events = new JwtBearerEvents();
        })
        .AddOpenIdConnect("oidc", options =>
        {
            options.Authority = "https://localhost:5001";

            options.ClientId = "client";
            options.ClientSecret = "secret";
            options.ResponseType = "code";

            options.Scope.Clear();
            options.Scope.Add("openid");
            options.Scope.Add("profile");

            options.MapInboundClaims = false; // Don't rename claim types

            options.SaveTokens = true;
        });
        builder.Services.AddAuthorization();
    }
    public static void ConfigureAppServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddHttpClient("LockEventsClient", httpClient =>
        {
            string lockEventsApi = builder.Configuration.GetValue<string>("ServiceBaseUrl:AccessLockEvents");

            httpClient.BaseAddress = new Uri(lockEventsApi);
        });
        builder.Services.AddScoped<IRepositoryBase<Lock>, LocksRespository>();
        builder.Services.AddScoped<IRepositoryBase<User>, UsersRespository>();
        builder.Services.AddScoped<ILockAccessService, LockAccessService>();
        builder.Services.AddScoped<ILockEventsService, LockEventsService>();
        builder.Services.AddScoped<ILockService, LockService>();
    }
    public static void ConfigureAppDbContext(this WebApplicationBuilder builder)
    {
        var sqlConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        var migrationsAssembly = typeof(Program).Assembly.GetName().Name;
        builder.Services.AddDbContext<LockingDbContext>(options =>
            options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly(migrationsAssembly))
        );
    }

}