using MachineryFleet.Core.Entities;

namespace MachineryFleet.Core.Services
{
    public interface IMachineService
    {
        event Func<Task> OnDatabaseChangedAsync;
        Task AddAsync(Machine machine);
        Task AddLogEntryAsync(Guid id, string logEntry, DateTime? timeStamp = null);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<Machine>> GetAllAsync();
        Task<Machine?> GetAsync(Guid id);
        Task<string> GetLatestLogEntryAsync(Guid id);
        Task<IEnumerable<string>> GetLogEntriesAsync(Guid id);
        Task<MachineStatus> GetMachineStatusAsync(Guid id);
        Task SendDataToLog(Machine machine, MachineService.LogType logType, object data, DateTime? dateTime = null);
        Task StartAsync(Guid id);
        Task StopAsync(Guid id);
        Task UpdateAsync(Machine machine);
        Task NotifyDatabaseChangedAsync();
    }
}