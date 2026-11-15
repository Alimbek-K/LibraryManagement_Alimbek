
# Library Management System - Architecture

## Architecture Overview

The application uses a layered architecture
based on ASP.NET Core Blazor and Entity Framework Core.

## Presentation Layer

Blazor components provide the user interface.

Main pages:
- Home.razor - Dashboard and statistics
- Books.razor - Book management
- Members.razor - Member management
- Loans.razor - Borrowing and returning

## Business Logic Layer

LibraryService.cs contains the main borrowing rules.

BorrowBookAsync:
- Validates the due date
- Checks whether the book and member exist
- Checks book availability
- Decreases available quantity
- Creates a loan record

ReturnBookAsync:
- Checks whether the loan exists
- Prevents repeated returns
- Records the return date
- Restores available quantity

## Data Access Layer

LibraryDbContext uses Entity Framework Core
to communicate with SQLite.

Entities:
- Book
- Member
- Loan

Books and Members pages currently use
LibraryDbContext directly.

Loans uses LibraryService.

## Database Relationships

Book (1) ---- (Many) Loan

Member (1) ---- (Many) Loan

Each loan belongs to one book and one member.

## Dependency Injection

LibraryDbContext is registered using
AddDbContextFactory.

LibraryService is registered using AddScoped.

## Testing

The application includes a separate xUnit project.

Tests cover:
- Model properties
- Loan status calculations
- Book borrowing
- Book returns
- Book availability
- Repeated return prevention
- Due date validation

## Security

NuGet package vulnerability checks were performed.
No known vulnerable packages were reported by the
configured package sources after dependency updates.
