using Microsoft.AspNetCore.Mvc;
using TodoApi_AsN.Models;
using TodoApi_AsN.Services;

namespace TodoApi_AsN.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDTO dto)
    {
        var user = await _authService.RegisterAsync(dto);
        if (user == null)
            return BadRequest("Nom d'utilisateur déjà pris.");

        return Ok(new { message = "Utilisateur créé avec succès.", role = user.Role });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDTO dto)
    {
        var token = await _authService.LoginAsync(dto);
        if (token == null)
            return Unauthorized("Identifiants invalides.");

        return Ok(new { token });
    }
}
