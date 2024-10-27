using BlazingQuiz.Api.Entities;

namespace BlazingQuiz.Api.Repositories;

public interface IUserRepository
{
    Task<User?> ExistsByEmail(string email);
}
