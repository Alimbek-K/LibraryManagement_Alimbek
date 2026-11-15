
using LibraryManagement_Alimbek.Data;
using LibraryManagement_Alimbek.Models;
using LibraryManagement_Alimbek.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LibraryManagement.Tests;

public class LibraryServiceTests
{
    private static async Task<(SqliteConnection Connection,
        IDbContextFactory<LibraryDbContext> Factory)>
        CreateDatabaseAsync()
    {
        var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<LibraryDbContext>()
            .UseSqlite(connection)
            .Options;

        var factory = new TestLibraryContextFactory(options);

        await using var db = await factory.CreateDbContextAsync();
        await db.Database.EnsureCreatedAsync();

        db.Books.Add(new Book
        {
            Title = "Clean Code",
            Author = "Robert C. Martin",
            Category = "Programming",
            Quantity = 2,
            AvailableQuantity = 2
        });

        db.Members.Add(new Member
        {
            FullName = "Test Member",
            Email = "test@example.com"
        });

        await db.SaveChangesAsync();

        return (connection, factory);
    }

    [Fact]
    public async Task Borrow_ShouldDecreaseAvailableCopies()
    {
        var (connection, factory) = await CreateDatabaseAsync();
        await using var _ = connection;

        var service = new LibraryService(factory);

        var result = await service.BorrowBookAsync(
            1, 1, DateTime.Today.AddDays(14));

        Assert.True(result.Success);

        await using var db = await factory.CreateDbContextAsync();

        var book = await db.Books.FindAsync(1);
        Assert.Equal(1, book!.AvailableQuantity);
        Assert.Equal(1, await db.Loans.CountAsync());
    }

    [Fact]
    public async Task Return_ShouldRestoreAvailableCopies()
    {
        var (connection, factory) = await CreateDatabaseAsync();
        await using var _ = connection;

        var service = new LibraryService(factory);

        await service.BorrowBookAsync(
            1, 1, DateTime.Today.AddDays(14));

        var result = await service.ReturnBookAsync(1);

        Assert.True(result.Success);

        await using var db = await factory.CreateDbContextAsync();

        var book = await db.Books.FindAsync(1);
        var loan = await db.Loans.FindAsync(1);

        Assert.Equal(2, book!.AvailableQuantity);
        Assert.True(loan!.IsReturned);
    }

    [Fact]
    public async Task Borrow_ShouldRejectUnavailableBook()
    {
        var (connection, factory) = await CreateDatabaseAsync();
        await using var _ = connection;

        await using (var db = await factory.CreateDbContextAsync())
        {
            var book = await db.Books.FindAsync(1);
            book!.AvailableQuantity = 0;
            await db.SaveChangesAsync();
        }

        var service = new LibraryService(factory);

        var result = await service.BorrowBookAsync(
            1, 1, DateTime.Today.AddDays(14));

        Assert.False(result.Success);
        Assert.Equal("Book is not available.", result.Message);
    }

    [Fact]
    public async Task Return_ShouldRejectDoubleReturn()
    {
        var (connection, factory) = await CreateDatabaseAsync();
        await using var _ = connection;

        var service = new LibraryService(factory);

        await service.BorrowBookAsync(
            1, 1, DateTime.Today.AddDays(14));

        await service.ReturnBookAsync(1);

        var secondReturn = await service.ReturnBookAsync(1);

        Assert.False(secondReturn.Success);

        await using var db = await factory.CreateDbContextAsync();
        var book = await db.Books.FindAsync(1);

        Assert.Equal(2, book!.AvailableQuantity);
    }

    [Fact]
    public async Task Borrow_ShouldRejectPastDueDate()
    {
        var (connection, factory) = await CreateDatabaseAsync();
        await using var _ = connection;

        var service = new LibraryService(factory);

        var result = await service.BorrowBookAsync(
            1, 1, DateTime.Today.AddDays(-1));

        Assert.False(result.Success);
        Assert.Equal("Due date must be in the future.", result.Message);
    }
}


public class TestLibraryContextFactory
    : IDbContextFactory<LibraryDbContext>
{
    private readonly DbContextOptions<LibraryDbContext> _options;

    public TestLibraryContextFactory(
        DbContextOptions<LibraryDbContext> options)
    {
        _options = options;
    }

    public LibraryDbContext CreateDbContext()
    {
        return new LibraryDbContext(_options);
    }

    public Task<LibraryDbContext> CreateDbContextAsync(
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(CreateDbContext());
    }



    
}

