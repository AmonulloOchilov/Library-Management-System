using Library_Management_System.Services;

public class ReportService
{
    private readonly BookService _bookService;
    private readonly MemberService _memberService;
    private readonly TransactionService _transactionService;
    
    public ReportService(BookService bookService, MemberService memberService, TransactionService transactionService)
    {
        _bookService = bookService;
        _memberService = memberService;
        _transactionService = transactionService;
    }
    
    public void DisplayLibraryStats()
    {
        var books = _bookService.LoadBooks();
        var members = _memberService.LoadMembers();
        
        int totalBooks = books.Sum(b => b.TotalQuantity);
        int availableBooks = books.Sum(b => b.AvailableQuantity);
        int borrowedBooks = totalBooks - availableBooks;
        
        Console.WriteLine("=== Library Statistics ===");
        Console.WriteLine($"Total Books: {totalBooks}");
        Console.WriteLine($"Available Books: {availableBooks}");
        Console.WriteLine($"Borrowed Books: {borrowedBooks}");
        Console.WriteLine($"Total Members: {members.Count}");
    }
}