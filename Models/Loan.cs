
using System.ComponentModel.DataAnnotations;

namespace LibraryManagement_Alimbek.Models;

public class Loan
{
    public int Id { get; set; }

    [Range(1, int.MaxValue)]
    public int BookId { get; set; }

    public Book? Book { get; set; }

    [Range(1, int.MaxValue)]
    public int MemberId { get; set; }

    public Member? Member { get; set; }

    public DateTime BorrowDate { get; set; } = DateTime.Today;

    public DateTime DueDate { get; set; } = DateTime.Today.AddDays(14);

    public DateTime? ReturnDate { get; set; }

    public bool IsReturned => ReturnDate.HasValue;

    public bool IsOverdue =>
        !IsReturned && DateTime.Today > DueDate.Date;
}
