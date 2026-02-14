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

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);

        // Automatically create profile based on role
        if (user.Role == UserRole.Pharmacy)
        {
            user.Pharmacy = new Pharmacy 
            { 
                Name = user.Name,
                User = user // Bidirectional reference for EF cascading
            };
        }
        else if (user.Role == UserRole.Supplier)
        {
            user.Supplier = new Supplier 
            { 
                Name = user.Name,
                User = user // Bidirectional reference for EF cascading
            };
        }
        else
        {
            throw new Exception("Invalid User Role. Please choose Pharmacy or Supplier.");
        }

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
            User: new UserDto(
                Id: user.Id,
                Name: user.Name,
                Email: user.Email,
                Role: user.Role
            )
        );
    }


    
}
