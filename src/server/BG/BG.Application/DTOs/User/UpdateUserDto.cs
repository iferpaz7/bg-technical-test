namespace BG.Application.DTOs.User;

public class UpdateUserDto
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public bool Enabled { get; set; }
    public int UserId { get; set; }
}