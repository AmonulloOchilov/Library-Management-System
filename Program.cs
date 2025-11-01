using Library_Management_System.Entities;
using Library_Management_System.Services;

BookService bookService = new BookService();
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
    string choice = Console.ReadLine();
    switch (choice)
    {
        case "1":
            AddBookMenu();
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
