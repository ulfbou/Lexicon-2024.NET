namespace TournamentAPI.Data.Repositories;

public interface IUoW
{
    ITournamentRepository TournamentRepository { get; }
    IGameRepository GameRepository { get; }
    Task CompleteAsync();
}
