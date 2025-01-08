namespace BG.Core.Models;

public class ApiResponse
{
    public string code { get; set; }
    public string message { get; set; }
    public dynamic payload { get; set; }
}