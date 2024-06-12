namespace MessengerAPI.Repositories
{
    public interface IUoW
    {
        IMessagesRepository Messages { get; }
        IUsersRepository Users { get; }
        Task CompleteAsync();
    }
}