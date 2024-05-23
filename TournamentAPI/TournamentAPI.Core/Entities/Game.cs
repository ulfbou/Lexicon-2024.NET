namespace TournamentAPI.Core.Entities;

public class Game
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Time { get; set; } = null!;
}