namespace BG.Application.DTOs.User;

public class UserFilterDto : PaginationDto
{
    public int UserId { get; set; }
    public string TextSearch { get; set; }
}