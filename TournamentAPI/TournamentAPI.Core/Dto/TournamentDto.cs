namespace TournamentAPI.Core.Dto;

public class TournamentDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public DateTime StartTime { get; set; }
    public DateTime EndTime => StartTime.AddMonths(3);
}
