using Microsoft.AspNetCore;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SmartAccess.Identity.API.Migrations.Seed;
using SmartAccess.Services.Identity.Data;
using SmartAccess.Services.Identity.Model;
using System.Net;

namespace SmartAccess.Identity.API
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.

            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            builder.Services.AddHttpLogging(logging =>
            {
                logging.LoggingFields = HttpLoggingFields.RequestPath | HttpLoggingFields.RequestMethod | HttpLoggingFields.RequestHeaders;
                logging.RequestBodyLogLimit = 4096;
                logging.ResponseBodyLogLimit = 4096;
            });
            builder.WebHost.ConfigureKestrel(opts =>
             {
                 opts.Listen(IPAddress.Loopback, port: 5000); // listen on http://localhost:5000
                 opts.ListenAnyIP(500); // listen on http://*:5005
                 opts.ListenLocalhost(5001, listenOptions => listenOptions.UseHttps());  // listen on https://localhost:5001
             });
            builder.Services.AddCors(o => o.AddPolicy("SmartAccess", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            builder.Services.AddControllers();
            builder.Services.AddHealthChecks();
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Smart Access Identity API",
                    Description = "An Smart Access Identity API for App auth",
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
                        new string[]{}
                    }
                });
            });

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            var migrationsAssembly = typeof(Program).Assembly.GetName().Name;
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            builder.Services.AddIdentityApiEndpoints<ApplicationUser>(config =>
            {
                config.SignIn.RequireConfirmedEmail = true;
                config.SignIn.RequireConfirmedAccount = true;
                config.Password.RequiredUniqueChars = 0;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireDigit = false;
                config.Password.RequireUppercase = false;
            }).AddRoles<IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddIdentityServer(options =>
            {
                options.IssuerUri = "https://localhost:5001";
                options.EmitStaticAudienceClaim = false;
            })
            .AddConfigurationStore(options =>
            {
                options.ConfigureDbContext = b => b.UseSqlServer(connectionString,
                    sql => sql.MigrationsAssembly(migrationsAssembly));
            })
            .AddOperationalStore(options =>
            {
                options.ConfigureDbContext = b => b.UseSqlServer(connectionString,
                    sql => sql.MigrationsAssembly(migrationsAssembly));
            });
            builder.Services.AddAuthorization();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseHttpLogging();
                app.SeedUserStoreForDashboard();
                app.PopulateIdentityServer();
            }

            app.UseHttpsRedirection();
            app.MapHealthChecks("/health");
            app.MapGroup("/identity").MapIdentityApi<ApplicationUser>();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}