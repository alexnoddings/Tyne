using System.ComponentModel.DataAnnotations;

namespace Tyne.Aerospace.Data.Entities;

public enum PartSize
{
    Small,
    Medium,
    Large,
    [Display(Name = "Extra large")]
    ExtraLarge
}
