using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BG.Core.Entities;

[Table("IdentificationType", Schema = "Config")]
public class IdentificationType : BaseEntity
{
    [Required, MaxLength(2)] public string Code { get; set; }
    [Required, MaxLength(50)] public string Name { get; set; }
}