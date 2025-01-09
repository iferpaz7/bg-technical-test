namespace BG.Application.DTOs.Person;

public class PersonaFilterDto : PaginationDto
{
    public int UserId { get; set; }
    public string TextSearch { get; set; }
}