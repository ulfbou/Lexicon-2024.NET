using MachineryFleet.Core.Entities;
using MachineryFleet.Core.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Zentient.Extensions;
using Zentient.Repository;

namespace MachineryFleet.Core.Services
{
    public class MachineService : IMachineService
    {
        private readonly IRepository<Machine, Guid> _machineRepository;
        private readonly CancellationToken _cancellation;

        public MachineService(IMachineRepository machineRepository, CancellationToken cancellation = default)
        {
            _machineRepository = machineRepository ?? throw new ArgumentNullException(nameof(machineRepository));
            _cancellation = cancellation == default ? new CancellationToken() : cancellation;
        }

        public event Func<Task>? OnDatabaseChangedAsync;

        public async Task<string> GetLatestLogEntryAsync(Guid id)
        {
            var machine = await GetAsync(id);

            if (machine is null) throw new InvalidOperationException($"Machine with id {id} not found");

            return machine.LogEntries.LastOrDefault() ?? throw new InvalidOperationException($"No log entries found for machine with id {id}");
        }

        public async Task<MachineStatus> GetMachineStatusAsync(Guid id)
        {
            var machine = await GetAsync(id);

            if (machine is null) throw new InvalidOperationException($"Machine with id {id} not found");

            return machine.Status;
        }

        public async Task AddLogEntryAsync(Guid id, string logEntry, DateTime? timeStamp = null)
        {
            ArgumentNullException.ThrowIfNull(logEntry, nameof(logEntry));

            if (timeStamp is null) timeStamp = DateTime.UtcNow;

            var machine = await GetAsync(id);

            if (machine is null) throw new InvalidOperationException($"Machine with id {id} not found");

            machine.LogEntries.Add($"{timeStamp.Value:yyyy-MM-dd HH:mm:ss} {logEntry}");

            var entry = await _machineRepository.UpdateAsync(machine, _cancellation);

            if (entry is null)
            {
                throw new InvalidOperationException($"Failed to add log entry to machine with id {id}");
            }
        }

        public async Task<IEnumerable<Machine>> GetAllAsync()
        {
            try
            {
                //return await _machineRepository.FindAsync(m => m.Status == MachineStatus.Online || m.Status == MachineStatus.Offline, _cancellation);
                return await _machineRepository.GetAllAsync(_cancellation);
            }
            catch (Exception)
            {
                return new List<Machine>();
            }
        }

        public async Task<Machine?> GetAsync(Guid id)
        {
            try
            {
                return await _machineRepository.GetAsync(id, _cancellation);
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        public async Task AddAsync(Machine machine)
        {
            ArgumentNullException.ThrowIfNull(machine, nameof(machine));
            var entry = await _machineRepository.AddAsync(machine, _cancellation);

            if (entry is null)
            {
                throw new InvalidOperationException("Failed to add machine");
            }
        }

        public async Task UpdateAsync(Machine machine)
        {
            ArgumentNullException.ThrowIfNull(machine, nameof(machine));
            var entry = await _machineRepository.UpdateAsync(machine, _cancellation);

            if (entry is null)
            {
                throw new InvalidOperationException($"Failed to update machine with id {machine.Id}");
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var user = await _machineRepository.GetAsync(id);
            if (user is null) throw new InvalidOperationException($"Machine with id {id} not found");

            var entityEntry = await _machineRepository.RemoveAsync(user);

            if (entityEntry is null)
            {
                throw new InvalidOperationException($"Failed to delete machine with id {id}");
            }
        }

        public async Task StartAsync(Guid id)
        {
            var machine = await _machineRepository.GetAsync(id);

            if (machine is null) throw new InvalidOperationException($"Machine with id {id} not found");

            machine.Status = MachineStatus.Active;

            var entry = await _machineRepository.UpdateAsync(machine);

            if (entry is null)
            {
                throw new InvalidOperationException($"Failed to update machine with id {id}");
            }
        }

        public async Task StopAsync(Guid id)
        {
            var machine = await _machineRepository.GetAsync(id);

            if (machine is null) throw new InvalidOperationException($"Machine with id {id} not found");

            machine.Status = MachineStatus.Inactive;

            var entry = await _machineRepository.UpdateAsync(machine);

            if (entry is null)
            {
                throw new InvalidOperationException($"Failed to update machine with id {id}");
            }
        }

        public async Task<IEnumerable<string>> GetLogEntriesAsync(Guid id)
        {
            var machine = await _machineRepository.GetAsync(id);

            if (machine is null) throw new InvalidOperationException($"Machine with id {id} not found");

            return machine.LogEntries;
        }

        public async Task SendDataToLog(Machine machine, LogType logType, object data, DateTime? dateTime = null)
        {
            ArgumentNullException.ThrowIfNull(machine, nameof(machine));

            var logEntry = $"{logType.GetName()}: {data} {LogTypeMetrics[logType]}";

            await AddLogEntryAsync(machine.Id, logEntry, dateTime ?? DateTime.UtcNow);
        }

        public async Task NotifyDatabaseChangedAsync()
        {
            if (OnDatabaseChangedAsync != null)
            {
                foreach (var delegateFunc in OnDatabaseChangedAsync.GetInvocationList())
                {
                    var func = (Func<Task>)delegateFunc;
                    await func.Invoke();
                }
            }
        }

        public enum LogType
        {
            Temperature,
            Pressure,
            Humidity,
            Vibration
        }

        private static readonly Dictionary<LogType, string> LogTypeMetrics = new()
        {
            { LogType.Temperature, "°C" },
            { LogType.Pressure, "psi" },
            { LogType.Humidity, "%" },
            { LogType.Vibration, "Hz" }
        };
    }
}
