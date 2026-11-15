
using System.ComponentModel.DataAnnotations;

namespace LibraryManagement_Alimbek.Models;

public class Book
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Book title is required")]
    [StringLength(150)]
    public string Title { get; set; } = "";

    [Required(ErrorMessage = "Author is required")]
    [StringLength(100)]
    public string Author { get; set; } = "";

    [Required(ErrorMessage = "Category is required")]
    public string Category { get; set; } = "";

    public int PublishedYear { get; set; }

    [Range(0, 10000, ErrorMessage = "Quantity cannot be negative")]
    public int Quantity { get; set; }

    public int AvailableQuantity { get; set; }
}
