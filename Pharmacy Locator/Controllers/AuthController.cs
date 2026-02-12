using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Pharmacy_Locator.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        // Simple mapping from DTO to Entity handled here or inside core service? 
        // Service expects User entity.
        // I should map here or let service handle DTO. 
        // Current IAuthService.RegisterAsync takes (User, password).
        // I'll map manually for MVP simplicity.
        
        var user = new User
        {
            Name = request.Name,
            Email = request.Email,
            Role = request.Role
        };
        
        try 
        {
            var token = await _authService.RegisterAsync(user, request.Password);
            return Ok(new { Token = token });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var token = await _authService.LoginAsync(request.Email, request.Password);
        if (token == null)
        {
            return Unauthorized("Invalid credentials");
        }
        
        return Ok(new { Token = token });
    }
}
