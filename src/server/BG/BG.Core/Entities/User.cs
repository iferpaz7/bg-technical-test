using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BG.Core.Entities;

[Table("User", Schema = "Config")]
public class User : BaseEntity
{
    [Required][MaxLength(150)] public string Username { get; set; }
    [Required] public byte[] Password { get; set; }
}