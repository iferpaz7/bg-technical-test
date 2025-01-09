namespace BG.Application.DTOs.Person;

public class UpdatePersonDto
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string IdCard { get; set; }
    public string Email { get; set; }
    public int IdentificationTypeId { get; set; }
    public bool Enabled { get; set; }
}