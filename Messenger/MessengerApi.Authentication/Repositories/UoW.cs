using MessengerAPI.Authentication.Data;
using System.Security.Cryptography;
namespace MessengerAPI.Authentication.Repositories
{

    public class UoW : IUoW
    {
        private readonly MessangerContext _context;
        private readonly MessagesRepository _messagesRepository;
        private readonly UsersRepository _usersRepository;

        public UoW(MessangerContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _messagesRepository = new MessagesRepository(_context.Messages);
            _usersRepository = new UsersRepository(_context.Users);
        }

        public IMessagesRepository Messages { get => _messagesRepository; }
        public IUsersRepository Users { get => _usersRepository; }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
