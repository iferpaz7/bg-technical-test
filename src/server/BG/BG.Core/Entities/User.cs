using System.ComponentModel.DataAnnotations;

namespace BG.Core.Entities;

public class User : BaseEntity
{
    [Required, MaxLength(150)] public string Username { get; set; }
    [Required, MaxLength(150)] public string Password { get; set; }
}