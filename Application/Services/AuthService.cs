using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services;

public class AuthService(IRepository<User> userRepository, IJwtTokenGenerator jwtTokenGenerator) : IAuthService
{
    public async Task<string> RegisterAsync(User user, string password)
    {
        // Check if email exists
        var existingUsers = await userRepository.FindAsync(u => u.Email == user.Email);
        if (existingUsers.Any())
        {
            throw new Exception("Email already exists.");
        }

        // Hash password (implementation detail postponed to Infrastructure or simplified here if using BCrypt directly)
        // Ideally use IPasswordHasher. For MVP, I will assume IPasswordHasher is injected or static Use BCrypt directly in infra.
        // Let's rely on an interface.
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);

        await userRepository.AddAsync(user);
        
        return jwtTokenGenerator.GenerateToken(user);
    }

    public async Task<LoginResponseDto?> LoginAsync(string email, string password)
    {
        var users = await userRepository.FindAsync(u => u.Email == email);
        var user = users.FirstOrDefault();

        if (user is null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        {
            return null;
        }

        return new LoginResponseDto(

            Token: jwtTokenGenerator.GenerateToken(user),
            new UserDto(
                Id: user.Id,
                Name: user.Name,
                Email: user.Email,
                Role: UserRole.Customer
            )
            );
    }


    
}
