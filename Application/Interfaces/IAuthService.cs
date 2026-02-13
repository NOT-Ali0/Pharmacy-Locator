using Application.DTOs;
using Domain.Entities;
using Domain.Enums;

namespace Application.Interfaces;

public interface IAuthService
{
    Task<string> RegisterAsync(User user, string password);
    Task<LoginResponseDto?> LoginAsync(string email, string password);
}
