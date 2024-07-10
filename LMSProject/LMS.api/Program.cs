using LMS.api.Configurations;
using LMS.api.Data;
using LMS.api.Extensions;
using LMS.api.Model;
using LMS.api.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LMS.api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            //using Serilog;
            //using Serilog.Events;

            var builder = WebApplication.CreateBuilder(args);

            // Configure Serilog
            //builder.Host.UseSerilog((context, services, configuration) => configuration
            //    .ReadFrom.Configuration(context.Configuration)
            //    .ReadFrom.Services(services)
            //    .Enrich.FromLogContext()
            //    .WriteTo.Console()
            //    .WriteTo.File(
            //        path: "Logs/sql-queries-.txt",
            //        rollingInterval: RollingInterval.Day,
            //        restrictedToMinimumLevel: LogEventLevel.Information,
            //        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
            //    ));

            // The rest of your Program.cs configuration...
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationDbContext")
                    ?? throw new InvalidOperationException("Connection string 'ApplicationDbContext' not found.")));

            builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<IGenericResponseService<Activity>, GenericResponseService<Activity>>();
            builder.Services.AddScoped<IGenericResponseService<ApplicationUser, string>, ApplicationUserReponseService>();
            builder.Services.AddScoped<IGenericResponseService<Course>, GenericResponseService<Course>>();
            builder.Services.AddScoped<IGenericResponseService<Document>, GenericResponseService<Document>>();
            builder.Services.AddScoped<IGenericResponseService<Module>, GenericResponseService<Module>>();

            builder.Services.AddAutoMapper(typeof(ModuleMappings));
            builder.Services.AddAutoMapper(typeof(ActivityMappings));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
