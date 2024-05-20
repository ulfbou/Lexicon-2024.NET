namespace TournamentAPI.Core.Dto;

public class GameDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public DateTime Time { get; set; }
}
