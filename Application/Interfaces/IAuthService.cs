using Domain.Entities;
using Domain.Enums;

namespace Application.Interfaces;

public interface IAuthService
{
    Task<string> RegisterAsync(User user, string password);
    Task<string?> LoginAsync(string email, string password);
}
