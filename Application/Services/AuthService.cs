using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services;

public class AuthService : IAuthService
{
    private readonly IRepository<User> _userRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    // Password hashing needed here, usually via interface or helper. 
    // I'll add IPasswordHasher to interfaces.
    
    public AuthService(IRepository<User> userRepository, IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<string> RegisterAsync(User user, string password)
    {
        // Check if email exists
        var existingUsers = await _userRepository.FindAsync(u => u.Email == user.Email);
        if (existingUsers.Any())
        {
            throw new Exception("Email already exists.");
        }

        // Hash password (implementation detail postponed to Infrastructure or simplified here if using BCrypt directly)
        // Ideally use IPasswordHasher. For MVP, I will assume IPasswordHasher is injected or static Use BCrypt directly in infra.
        // Let's rely on an interface.
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);

        await _userRepository.AddAsync(user);
        
        return _jwtTokenGenerator.GenerateToken(user);
    }

    public async Task<string?> LoginAsync(string email, string password)
    {
        var users = await _userRepository.FindAsync(u => u.Email == email);
        var user = users.FirstOrDefault();

        if (user is null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        {
            return null;
        }

        return _jwtTokenGenerator.GenerateToken(user);
    }
}
