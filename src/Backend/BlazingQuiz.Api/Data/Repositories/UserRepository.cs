using BlazingQuiz.Api.Entities;
using BlazingQuiz.Api.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BlazingQuiz.Api.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly QuizDbContext _context;

    public UserRepository(QuizDbContext context)
    {
        _context = context;
    }

    public async Task<User?> ExistsByEmail(string email)
    {
        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Email.Equals(email));

        return user;
    }
}
