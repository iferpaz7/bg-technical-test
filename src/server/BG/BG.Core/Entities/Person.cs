using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BG.Core.Entities;

[Table("Person", Schema = "Config")]
public class Person : BaseEntity
{
    [Required, MaxLength(150)] public string FirstName { get; set; }
    [Required, MaxLength(150)] public string LastName { get; set; }
    [Required, MaxLength(25)] public string IdCard { get; set; }
    [Required, MaxLength(100)] public string Email { get; set; }
    public int IdentificationTypeId { get; set; }
    public virtual IdentificationType IdentificationType { get; set; }
    [Required, MaxLength(27)] public string Code { get; set; } //IdCard + IdentificationTypeCode
    [Required, MaxLength(300)] public string FullName { get; set; }
}