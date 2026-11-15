
using System;
using LibraryManagement_Alimbek.Models;
using Xunit;

namespace LibraryManagement.Tests;

public class LibraryTests
{
    [Fact]
    public void NewBook_ShouldStoreCorrectQuantity()
    {
        var book = new Book
        {
            Title = "Clean Code",
            Author = "Robert C. Martin",
            Category = "Programming",
            Quantity = 5,
            AvailableQuantity = 5
        };

        Assert.Equal(5, book.Quantity);
        Assert.Equal(5, book.AvailableQuantity);
    }

    [Fact]
    public void BorrowBook_ShouldDecreaseAvailableQuantity()
    {
        var book = new Book
        {
            Title = "Test Book",
            Quantity = 5,
            AvailableQuantity = 5
        };

        book.AvailableQuantity--;

        Assert.Equal(4, book.AvailableQuantity);
    }

    [Fact]
    public void ReturnBook_ShouldIncreaseAvailableQuantity()
    {
        var book = new Book
        {
            Title = "Test Book",
            Quantity = 5,
            AvailableQuantity = 4
        };

        book.AvailableQuantity++;

        Assert.Equal(5, book.AvailableQuantity);
    }

    [Fact]
    public void Loan_ShouldBeMarkedAsReturned()
    {
        var loan = new Loan
        {
            BorrowDate = DateTime.Today.AddDays(-3),
            DueDate = DateTime.Today.AddDays(11),
            ReturnDate = DateTime.Today
        };

        Assert.True(loan.IsReturned);
    }

    [Fact]
    public void Loan_ShouldBeOverdue()
    {
        var loan = new Loan
        {
            BorrowDate = DateTime.Today.AddDays(-20),
            DueDate = DateTime.Today.AddDays(-6),
            ReturnDate = null
        };

        Assert.True(loan.IsOverdue);
    }

    [Fact]
    public void ReturnedLoan_ShouldNotBeOverdue()
    {
        var loan = new Loan
        {
            DueDate = DateTime.Today.AddDays(-6),
            ReturnDate = DateTime.Today
        };

        Assert.False(loan.IsOverdue);
    }
}
