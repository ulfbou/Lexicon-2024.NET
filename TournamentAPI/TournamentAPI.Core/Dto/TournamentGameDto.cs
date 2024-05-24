namespace TournamentAPI.Core.Dto;

public class TournamentGameDto
{
    public int TournamentId { get; set; }
    public int GameId { get; set; }
    public string Title { get; set; } = null!;
    public DateTime Time { get; set; }
}
