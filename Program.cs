using Library_Management_System.Entities;
using System.Text.Json;
using Library_Management_System.Services;

BookService bookService = new BookService();
MemberService memberService = new MemberService();
Console.WriteLine("Welcome to Library Management System");
void ShowMenu()
{
    Console.WriteLine("Main Menu");
    Console.WriteLine("1. Add Book");
    Console.WriteLine("2. View All Books");
    Console.WriteLine("3. Search Books");
    Console.WriteLine("4. Register Member");
    Console.WriteLine("5. View All Members");
    Console.WriteLine("6. Issue Book to Member");
    Console.WriteLine("7. Return Book");
    Console.WriteLine("8. View Borrowing History");
    Console.WriteLine("9. View Overdue Books");
    Console.WriteLine("10. Exit");
    Console.Write("Select an option (1-10): ");
}

while (true)
{
    ShowMenu();
    string choice = Console.ReadLine()!;
    switch (choice)
    {
        case "1":
            AddBookMenu();
            break;
        case "2":
            bookService.ViewAllBooks();
            break;
        case "3":
            SearchBooksMenu();
            break;
        case "4":
            AddMemberMenu();
            break;
        case "10":
            Environment.Exit(0);
            break;
    }
}

void AddBookMenu()
{
    Console.WriteLine("Add new Book");
    try
    {
        Console.Write("Enter Book ID: ");
        int bookId = int.Parse(Console.ReadLine()!);
        Console.Write("Enter Title: ");
        string bookTitle = Console.ReadLine()!;
        Console.Write("Author: ");
        string bookAuthor = Console.ReadLine()!;
        Console.Write("ISBN: ");
        string bookIsbn = Console.ReadLine()!;
        Console.Write("Total Quantity: ");
        int totalQuantity = int.Parse(Console.ReadLine()!);
        
        bookService.AddBook(bookId, bookTitle, bookAuthor, bookIsbn, totalQuantity);
    }
    catch (Exception e)
    {
        Console.WriteLine($"Error: {e.Message}");
        throw;
    }
}

void SearchBooksMenu()
{
    Console.Write($"Enter search term (title, author, or ISBN): ");
    string searchTerm = Console.ReadLine()!.ToLower();
    var results = bookService.SearchBooks(searchTerm);
    if (results.Any())
    {
        Console.WriteLine($"Found {results.Count} book(s)");
        foreach (var book in results)
        {
            Console.WriteLine($"ID: {book.BookId} - Title: {book.Title} - Author: {book.Author} - " +
                              $"ISBN: {book.ISBN} - Total Quantity: {book.TotalQuantity} - Available Quantity: " +
                              $"{book.AvailableQuantity} - Is Available: {book.IsAvailable}");
        }
    }
    else
    {
        Console.WriteLine("No books found");
    }
}

void AddMemberMenu()
{
    Console.WriteLine("Add new Member");
    try
    {
        Console.Write("Enter Member ID: ");
        int memberId = int.Parse(Console.ReadLine()!);
        Console.Write("Enter Firstname: ");
        string memberFirstName = Console.ReadLine()!;
        Console.Write("Enter Lastname: ");
        string memberLastName = Console.ReadLine()!;
        Console.Write("Enter email: ");
        string email = Console.ReadLine()!;
        Console.Write("Enter Phone number: ");
        string phone = Console.ReadLine()!;
        Console.Write("Enter Membership date: ");
        DateTime membershipDate = DateTime.Parse(Console.ReadLine()!);
        memberService.AddMembers(memberId, memberFirstName, memberLastName, email, phone, membershipDate);
    }
    catch (Exception e)
    {
        Console.WriteLine($"Error: {e.Message}");
        throw;
    }
}