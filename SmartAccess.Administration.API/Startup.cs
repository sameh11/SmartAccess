using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SmartAccess.Administration.API;
using SmartAccess.Administration.API.DataAccess;
using System.Net;
using System.Text;

public static class Startup
{
    public static void ConfigureAppAuthentication(this WebApplicationBuilder builder)
    {
        var identityServerUrl = builder.Configuration.GetValue<string>("IdentityServerUrl");
        builder.Services.AddAuthentication("Bearer").AddJwtBearer(x =>
        {
            x.Authority = identityServerUrl;
            x.Audience = $"{identityServerUrl}/resources";
            x.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidAudiences = ["locking"],
                ValidIssuer = identityServerUrl,
            };
        });

        builder.Services.AddAuthorization();
    }
    public static void ConfigureAppDbContext(this WebApplicationBuilder builder)
    {
        var sqlConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        var migrationsAssembly = typeof(Program).Assembly.GetName().Name;
        builder.Services.AddDbContext<AdministrationDbContext>(options =>
            options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly(migrationsAssembly))
        );
    }
    public static void ConfigureAppEndPoints(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(o => o.AddPolicy("SmartAccess", builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        }));
        builder.WebHost.ConfigureKestrel(opts =>
        {
            opts.Listen(IPAddress.Loopback, port: 7000); // listen on http://localhost:7000
            opts.ListenAnyIP(5005); // listen on http://*:7007
            opts.ListenLocalhost(7001, listenOptions => listenOptions.UseHttps());  // listen on https://localhost:7001
        });
        builder.Services.AddControllers(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);
        builder.Services.AddHealthChecks();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Smart Access Administration API",
                Description = "An Smart Access Administration API",
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
}
