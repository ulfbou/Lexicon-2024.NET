﻿using System.Security.Cryptography;
using TournamentAPI.Core.Repositories;
using TournamentAPI.Data.Data;

namespace TournamentAPI.Data.Repositories;

public class UoW : IUoW
{
    private readonly TournamentContext _context;
    private readonly TournamentRepository _tournamentRepository;
    private readonly GameRepository _gameRepository;

    public UoW(TournamentContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _tournamentRepository = new TournamentRepository(context);
        _gameRepository = new GameRepository(context);
    }

    public ITournamentRepository TournamentRepository => _tournamentRepository;
    public IGameRepository GameRepository => _gameRepository;

    public async Task CompleteAsync()
    {
        await _context.SaveChangesAsync();
    }
}
