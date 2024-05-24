namespace TournamentAPI.Core.Entities;

public class Game : IEntity
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Time { get; set; } = null!;
}
// Compare this snippet from TournamentAPI.Core/Repositories/ITournamentRepository.cs: