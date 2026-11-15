
using System.ComponentModel.DataAnnotations;

namespace LibraryManagement_Alimbek.Models;

public class Member
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Full name is required")]
    [StringLength(100)]
    public string FullName { get; set; } = "";

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; } = "";

    public string Phone { get; set; } = "";

    public DateTime JoinedDate { get; set; } = DateTime.Today;
}
