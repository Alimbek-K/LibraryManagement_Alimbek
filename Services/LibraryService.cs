
using LibraryManagement_Alimbek.Data;
using LibraryManagement_Alimbek.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement_Alimbek.Services;

public class LibraryService
{
    private readonly IDbContextFactory<LibraryDbContext> _factory;

    public LibraryService(IDbContextFactory<LibraryDbContext> factory)
    {
        _factory = factory;
    }

    public async Task<(bool Success, string Message)> BorrowBookAsync(
        int bookId, int memberId, DateTime dueDate)
    {
        if (dueDate.Date <= DateTime.Today)
            return (false, "Due date must be in the future.");

        await using var db = await _factory.CreateDbContextAsync();
        await using var transaction = await db.Database.BeginTransactionAsync();

        var book = await db.Books.FindAsync(bookId);
        var member = await db.Members.FindAsync(memberId);

        if (book == null || member == null)
            return (false, "Book or member not found.");

        if (book.AvailableQuantity <= 0)
            return (false, "Book is not available.");

        book.AvailableQuantity--;

        db.Loans.Add(new Loan
        {
            BookId = bookId,
            MemberId = memberId,
            BorrowDate = DateTime.Today,
            DueDate = dueDate.Date
        });

        await db.SaveChangesAsync();
        await transaction.CommitAsync();

        return (true, "Book issued successfully.");
    }

    public async Task<(bool Success, string Message)> ReturnBookAsync(
        int loanId)
    {
        await using var db = await _factory.CreateDbContextAsync();
        await using var transaction = await db.Database.BeginTransactionAsync();

        var loan = await db.Loans
            .Include(l => l.Book)
            .FirstOrDefaultAsync(l => l.Id == loanId);

        if (loan == null || loan.IsReturned || loan.Book == null)
            return (false, "Loan not found or already returned.");

        loan.ReturnDate = DateTime.Today;
        loan.Book.AvailableQuantity++;

        await db.SaveChangesAsync();
        await transaction.CommitAsync();

        return (true, "Book returned successfully.");
    }
}
