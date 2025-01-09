namespace BG.Core.Models;

public class ApiResponse
{
    public string Code { get; set; }
    public string Message { get; set; }
    public dynamic Payload { get; set; }
}