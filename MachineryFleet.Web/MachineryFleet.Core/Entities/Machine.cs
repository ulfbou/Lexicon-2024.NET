namespace MachineryFleet.Core.Entities
{
    public class Machine
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public MachineStatus Status { get; set; }
        public IList<string> LogEntries { get; set; } = new List<string>();
    }

    public enum MachineStatus
    {
        Inactive,
        Active
    }
}
