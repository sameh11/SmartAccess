using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SmartAccess.LockEvents.API.Migrations;
using SmartAccess.LockEvents.API.Repositories;
using SmartAccess.Services.LockEvents.API.Data;
using SmartAccess.Services.LockEvents.API.Model;

namespace SmartAccess.LockEvents.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Logging.AddConsole();
            builder.Services.AddHttpLogging(logging =>
            {
                logging.LoggingFields = HttpLoggingFields.RequestPath | HttpLoggingFields.RequestMethod;
                logging.RequestBodyLogLimit = 4096;
                logging.ResponseBodyLogLimit = 4096;
            });
            builder.Services.AddHealthChecks();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Smart Access Locking API",
                    Description = "An Smart Access Locking API",
                });
            });
            var sqlConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            var migrationsAssembly = typeof(Program).Assembly.GetName().Name;
            builder.Services.AddDbContext<LockEventsDbContext>(options =>
                options.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly(migrationsAssembly))
            );
            builder.Services.AddScoped<IRepositoryBase<LockEvent>, LockEventsRepository>();
            builder.Services.AddControllers();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseDeveloperExceptionPage();
                app.UseHttpLogging();
                app.EnsureMigrated();

            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
