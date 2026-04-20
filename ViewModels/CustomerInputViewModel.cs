using System.ComponentModel.DataAnnotations;

namespace ftpms.ViewModels;

public class CustomerInputViewModel
{
    [Required]
    [StringLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string LastName { get; set; } = string.Empty;

    [EmailAddress]
    [StringLength(255)]
    public string Email { get; set; } = string.Empty;

    [StringLength(50)]
    public string PhoneNumber { get; set; } = string.Empty;

    [StringLength(300)]
    public string Address { get; set; } = string.Empty;
}
