namespace BG.Application.DTOs.Person;

public class CreatePersonDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string IdCard { get; set; }
    public string Email { get; set; }
    public int IdentificationTypeId { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public int UserId { get; set; }
}