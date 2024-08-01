namespace Domain.Domain.Abstractions;
public interface IUserRepository
{
    Task<User> GetUserByChatIdAsync(long chatId, CancellationToken cancellationToken);
    Task AddUserAsync(User user, CancellationToken cancellationToken);
}