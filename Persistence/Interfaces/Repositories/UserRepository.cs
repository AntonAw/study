using Domain.Domain;
using Domain.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Interfaces.Repositories;
public class UserRepository(IDatabaseContext _context) : IUserRepository
{
    public async Task<User> GetUserByChatIdAsync(long chatId, CancellationToken cancellationToken)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.ChatId == chatId, cancellationToken);
    }

    public async Task AddUserAsync(User user, CancellationToken cancellationToken)
    {
        var saved = await _context.Users.FirstOrDefaultAsync(u => u.ChatId == user.ChatId, cancellationToken);

        if (saved == null)
            await _context.Users.AddAsync(user, cancellationToken);
        else
            saved!.PhoneNumber = user.PhoneNumber;

        await _context.SaveChangesAsync(cancellationToken);
    }
}