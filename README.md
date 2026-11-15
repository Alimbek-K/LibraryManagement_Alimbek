
# Library Management System

## Project Overview

Library Management System is a web application developed
using C#, ASP.NET Core Blazor, Entity Framework Core,
and SQLite.

The application helps librarians manage books, members,
and borrowing operations.

## Main Features

- Dashboard with library statistics
- Add, edit, delete, and search books
- Register and manage library members
- Issue books to registered members
- Return borrowed books
- Automatically update available book quantities
- Track overdue and returned loans
- Store information in SQLite
- Validate user input and handle errors

## Technologies

- C#
- .NET 10
- ASP.NET Core Blazor Web App
- Entity Framework Core 10
- SQLite
- LINQ
- Bootstrap and CSS
- xUnit

## Project Architecture

The application uses a layered structure:

1. Presentation Layer - Blazor Razor components
2. Business Logic Layer - LibraryService
3. Data Access Layer - LibraryDbContext
4. Database Layer - SQLite

## Main Models

Book:
Stores title, author, category, publication year,
total quantity, and available quantity.

Member:
Stores member name, email, phone, and registration date.

Loan:
Stores borrowing date, due date, return date,
and relationships with books and members.

## How to Run

Requirements:
- .NET 10 SDK

Open a terminal in the application folder.

Run:

    dotnet restore
    dotnet build
    dotnet run

Open the localhost address displayed in the terminal.

The application creates the SQLite database automatically
if it does not exist.

## Testing

The solution includes a separate xUnit test project.

Run:

    dotnet test LibraryManagement.Tests/LibraryManagement.Tests.csproj

The test suite contains 11 tests covering model behavior,
book borrowing, book returns, availability validation,
and prevention of repeated returns.

## Future Improvements

- User authentication and authorization
- Advanced book search
- Email notifications for overdue loans
- Export reports to PDF or Excel
- Cloud deployment

## Author

Kengessov Alimbek

## Course

Introduction to Functional Programming
