using MachineryFleet.Core.Entities;
using MachineryFleet.Core.Repository;
using MachineryFleet.Core.Services;
using MachineryFleet.Persistence.Data;
using MachineryFleet.Web.Components;
using Microsoft.EntityFrameworkCore;
using Zentient.Extensions;
using Zentient.Repository;

namespace MachineryFleet.Web
{
    public static class SeedData
    {
        public static async Task Initialize(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var context = serviceProvider.GetRequiredService<MachineryFleetDbContext>();
                //var machineService = serviceProvider.GetRequiredService<MachineService>();
                var machineService = serviceProvider.GetService<IMachineService>();
                ArgumentNullException.ThrowIfNull(machineService, "MachineService is null");
                context.Database.EnsureCreated();
                //var repository = serviceProvider.GetRequiredService<IMachineRepository>();
                //ArgumentNullException.ThrowIfNull(repository, "MachineRepository is null");
                if (context.Machines.Any())
                {
                    return;
                }
                // generate code for more realistic machines as seed data
                var random = new Random();
                foreach (var machine in new object[]
                {
                    "Dumper",
                    "Excavator",
                    "Bulldozer",
                    "Crane",
                    "Forklift",
                    "Grader",
                    "Loader",
                    "Paver",
                    "Road roller",
                    "Skid-steer loader",
                    "Trencher",
                    "Tractor",
                    "Truck",
                    "Truck-mounted crane"
                    }.Shuffle().Take(5))
                {
                    context.Machines.Add(new Machine
                    {
                        Id = Guid.NewGuid(),
                        Name = machine.ToString(),
                        Status = random.Next(0, 2) == 0 ? MachineStatus.Inactive : MachineStatus.Active,
                        LogEntries = []
                    });
                }

                await context.SaveChangesAsync();

                var machines = await machineService.GetAllAsync();

                foreach (var machine in machines)
                {
                    DateTime time = DateTime.UtcNow.AddMinutes(-random.Next(0, 100));
                    for (int i = 0; i < 10; i++)
                    {
                        MachineService.LogType logType = (MachineService.LogType)random.Next(0, 4);
                        await machineService.SendDataToLog(machine, logType, new Random().Next(0, 100), time);
                        time = time.AddMinutes(random.Next(1, 9)).AddSeconds(random.Next(1, 60));
                    }
                }
            }
        }
    }
}
