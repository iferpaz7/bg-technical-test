namespace BG.Application.DTOs.User;

public class UserFilterDto : PaginationDto
{
    public string TextSearch { get; set; }
}