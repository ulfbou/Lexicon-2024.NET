using TournamentAPI.Extensions;
using Microsoft.EntityFrameworkCore;
using Serilog;
using TournamentAPI.Data.Repositories;
using TournamentAPI.Core.Repositories;

namespace TournamentAPI;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllers().AddNewtonsoftJson();
        builder.Services.AddDbContext<TournamentContext>(options =>
            options.UseSqlServer(
                builder.Configuration.GetConnectionString("TournamentAPIContext") ?? 
                throw new InvalidOperationException("Connection string 'TournamentAPIContext' not found.")));
        builder.Services.AddScoped<IUoW, UoW>();
        builder.Services.AddAutoMapper(typeof(TournamentMappings));
        builder.Services
            .AddControllers()
            .AddNewtonsoftJson(options => {
                options.SerializerSettings.Converters.Add(new DateFormatConverter());
                options.SerializerSettings.ContractResolver = new CustomDateContractResolver();
            });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        await app.SeedDataAsync();

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
