using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using TodoApi_AsN.Models;

namespace TodoApi_AsN.Services;

public class AuthService
{
    private readonly IMongoCollection<User> _users;
    private readonly IConfiguration _config;

    public AuthService(IConfiguration config)
    {
        _config = config;
        var client = new MongoClient(config.GetValue<string>("MongoDb:ConnectionString"));
        var database = client.GetDatabase(config.GetValue<string>("MongoDb:DatabaseName"));
        _users = database.GetCollection<User>("Users");
    }

    public async Task<User?> RegisterAsync(RegisterDTO dto)
    {
        var existing = await _users.Find(u => u.Username == dto.Username).FirstOrDefaultAsync();
        if (existing != null) return null;

        var user = new User
        {
            Username = dto.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = dto.Role
        };

        await _users.InsertOneAsync(user);
        return user;
    }

    public async Task<string?> LoginAsync(LoginDTO dto)
    {
        var user = await _users.Find(u => u.Username == dto.Username).FirstOrDefaultAsync();
        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            return null;

        return GenerateToken(user);
    }

    private string GenerateToken(User user)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id!),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
