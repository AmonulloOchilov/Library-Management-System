using Library_Management_System.Entities;
using System.Text.Json;
using Library_Management_System.Services;

BookService bookService = new BookService();
MemberService memberService = new MemberService();
TransactionService transactionService = new TransactionService(bookService, memberService);
ReportService reportService = new ReportService(bookService, memberService, transactionService);

Console.WriteLine("Welcome to Library Management System");
void ShowMenu()
{
    Console.WriteLine("Main Menu");
    Console.WriteLine("1.  Add Book");
    Console.WriteLine("2.  View All Books");
    Console.WriteLine("3.  Search Books");
    Console.WriteLine("4.  Register Member");
    Console.WriteLine("5.  View All Members");
    Console.WriteLine("6.  Issue Book to Member");
    Console.WriteLine("7.  Return Book");
    Console.WriteLine("8.  View Borrowing History");
    Console.WriteLine("9.  View Overdue Books");
    Console.WriteLine("10. View Library Statistics");
    Console.WriteLine("11. Exit");
    Console.Write("Select an option (1-11): ");
}

while (true)
{
    ShowMenu();
    string choice = Console.ReadLine()!;
    switch (choice)
    {
        case "1":
            AddBookMenu();
            WaitForUser();
            break;
        case "2":
            bookService.ViewAllBooks();
            WaitForUser();
            break;
        case "3":
            SearchBooksMenu();
            WaitForUser();
            break;
        case "4":
            AddMemberMenu();
            WaitForUser();
            break;
        case "5":
            memberService.ViewAllMembers();
            WaitForUser();
            break;
        case "6":
            IssueBookMenu();
            WaitForUser();
            break;
        case "7":
            ReturnBookMenu();
            WaitForUser();
            break;
        case "8":
            transactionService.DisplayBorrowingHistory();
            WaitForUser();
            break;
        case "9":
            transactionService.DisplayOverdueBooks();
            WaitForUser();
            break;
        case "10":
            reportService.DisplayLibraryStats();
            WaitForUser();
            break;
        case "11":
            Environment.Exit(0);
            break;
    }
}

void AddBookMenu()
{
    Console.WriteLine("Add new Book");
    try
    {
        Console.Write("Enter Title: ");
        string bookTitle = Console.ReadLine()!;
        if (string.IsNullOrWhiteSpace(bookTitle))
        {
            Console.WriteLine("Title cannot be empty!");
            return;
        }
        Console.Write("Author: ");
        string bookAuthor = Console.ReadLine()!;
        Console.Write("ISBN: ");
        string bookIsbn = Console.ReadLine()!;
        Console.Write("Total Quantity: ");
        int totalQuantity = int.Parse(Console.ReadLine()!);
        
        bookService.AddBook(bookTitle, bookAuthor, bookIsbn, totalQuantity);
    }
    catch (Exception e)
    {
        Console.WriteLine($"Error: {e.Message}");
    }
}

void SearchBooksMenu()
{
    Console.Write($"Enter search term (title, author, or ISBN): ");
    string searchTerm = Console.ReadLine()!.ToLower();
    var results = bookService.SearchBooks(searchTerm);
    if (results.Any())
    {
        Console.WriteLine("{0,-10} {1,-23} {2,-23} {3,-17} {4,-22} {5, -20}",
            "ID", "Title", "Author", "ISBN", "Total Quantity", "Available Quantity");

        foreach (var b in results)
        {
            Console.WriteLine("{0,-10} {1,-23} {2,-23} {3,-17} {4,-22} {5, -20}",
                b.BookId, b.Title, b.Author, b.ISBN, b.TotalQuantity, b.AvailableQuantity);
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
        Console.Write("Enter Firstname: ");
        string memberFirstName = Console.ReadLine()!;
        Console.Write("Enter Lastname: ");
        string memberLastName = Console.ReadLine()!;
        Console.Write("Enter email: ");
        string email = Console.ReadLine()!;
        Console.Write("Enter Phone number: ");
        string phone = Console.ReadLine()!;
        memberService.AddMembers(memberFirstName, memberLastName, email, phone);
    }
    catch (Exception e)
    {
        Console.WriteLine($"Error: {e.Message}");
    }
}
void IssueBookMenu()
{
    Console.WriteLine("Issue Book to Member");
    try
    {
        Console.Write("Enter Member ID: ");
        int memberId = int.Parse(Console.ReadLine()!);
        Console.Write("Enter Book ID: ");
        int bookId = int.Parse(Console.ReadLine()!);
        
        transactionService.IssueBookToMember(memberId, bookId);
    }
    catch (Exception e)
    {
        Console.WriteLine($"Error: {e.Message}");
    }
}
void ReturnBookMenu()
{
    Console.WriteLine("Return Book");
    try
    {
        Console.Write("Enter Transaction ID: ");
        int transactionId = int.Parse(Console.ReadLine()!);
        
        transactionService.ReturnBook(transactionId);
    }
    catch (Exception e)
    {
        Console.WriteLine($"Error: {e.Message}");
    }
    
}
void WaitForUser()
{
    Console.WriteLine("\nPress any key to continue...");
    Console.ReadKey();
    Console.Clear();
}