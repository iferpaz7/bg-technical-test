namespace BG.Application.DTOs;

public class PaginationDto
{
    public int PageIndex { get; set; } = 1; // Default to page 1
    public int PageSize { get; set; } = 10; // Default to 10 items per page
}