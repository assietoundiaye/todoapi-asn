namespace TodoApi_AsN.Models;

public class RegisterDTO
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Role { get; set; } = "user";
}

public class LoginDTO
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
}
