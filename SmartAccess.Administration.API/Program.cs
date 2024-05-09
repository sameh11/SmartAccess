using SmartAccess.Administration.API.DataAccess;

namespace SmartAccess.Administration.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.ConfigureAppLogging();
            builder.ConfigureAppEndPoints();
            builder.ConfigureAppDbContext();
            builder.ConfigureAppAuthentication();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseDeveloperExceptionPage();
                app.UseHttpLogging();
                app.PopulateAdministrationDb();
            }
            app.UseHsts();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
