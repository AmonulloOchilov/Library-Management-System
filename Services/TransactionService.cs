using System;
using System.Text.Json;
using Library_Management_System.Entities;

namespace Library_Management_System.Services;

public class TransactionService
{
    private readonly string filePath;
    private readonly BookService bookService;
    private readonly MemberService memberService;

    public TransactionService(BookService bookService, MemberService memberService, string customDataPath = null)
    {
        string dataFolder = customDataPath ?? "/Users/amonulloochilov/Desktop/Library Management System/Library Management System/Data";
        if (!Directory.Exists(dataFolder))
        {
            Directory.CreateDirectory(dataFolder);
        }

        filePath = Path.Combine(dataFolder, "transactions.json");
        this.bookService = bookService;
        this.memberService = memberService;
    }
    public List<Transaction> LoadTransactions()
    {
        if (!File.Exists(filePath))
        {
            return new List<Transaction>();
        }

        string json = File.ReadAllText(filePath);
        if (string.IsNullOrWhiteSpace(json))
        {
            return new List<Transaction>();
        }

        var result = JsonSerializer.Deserialize<List<Transaction>>(json);
        if (result != null)
        {
            return result;
        }
        else
        {
            return new List<Transaction>();
        }
    }
    private void SaveTransactions(List<Transaction> transactions)
    {
        string json = JsonSerializer.Serialize(transactions, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, json);
    }
    public void IssueBookToMember(int memberId, int bookId)
    {
        var transactions = LoadTransactions();
        var members = memberService.LoadMembers();
        var member = members.FirstOrDefault(m => m.MemberId == memberId);
        if (member == null)
        {
            Console.WriteLine($"Member with ID {memberId} not found.");
            return;
        }
    
        var books = bookService.LoadBooks();
        var book = books.FirstOrDefault(b => b.BookId == bookId);
        if (book == null)
        {
            Console.WriteLine($"Book with ID {bookId} not found.");
            return;
        }
    
        if (book.AvailableQuantity <= 0)
        {
            Console.WriteLine($"Book '{book.Title}' is not available.");
            return;
        }
        int newTransactionId = transactions.Count > 0 ? transactions.Max(t => t.TransactionId) + 1 : 1;
        var transaction = new Transaction
        {
            TransactionId = newTransactionId,
            BookId = bookId,
            MemberId = memberId,
            IssueDate = DateTime.Now,
            DueDate = DateTime.Now.AddDays(14), // 2 weeks borrowing period
            ReturnDate = null,
            Status = "Issued",
            FineAmount = null
        };
        book.AvailableQuantity--;
    
        transactions.Add(transaction);
        SaveTransactions(transactions);
        bookService.UpdateBook(book);
    
        Console.WriteLine($"Book '{book.Title}' issued to {member.FirstName} {member.LastName}. Due date: {transaction.DueDate:yyyy-MM-dd}");
    }
    
    public void ReturnBook(int transactionId)
    {
        var transactions = LoadTransactions();
        var transaction = transactions.FirstOrDefault(t => t.TransactionId == transactionId);
    
        if (transaction == null)
        {
            Console.WriteLine($"Transaction with ID {transactionId} not found.");
            return;
        }
    
        if (transaction.Status == "Returned")
        {
            Console.WriteLine("This book has already been returned.");
            return;
        }
    
        transaction.ReturnDate = DateTime.Now;
        transaction.Status = "Returned";
    
        if (transaction.ReturnDate > transaction.DueDate)
        {
            int overdueDays = (int)(transaction.ReturnDate.Value - transaction.DueDate).TotalDays;
            decimal fine = overdueDays * 1.0m; 
            transaction.FineAmount = fine;
            Console.WriteLine($"Overdue fine: ${fine} for {overdueDays} days.");
        }
    
        var books = bookService.LoadBooks();
        var book = books.FirstOrDefault(b => b.BookId == transaction.BookId);
        if (book != null)
        {
            book.AvailableQuantity++;
            bookService.UpdateBook(book);
        }
    
        SaveTransactions(transactions);
        Console.WriteLine("Book returned successfully.");
    }
    public void DisplayBorrowingHistory()
    {
        var transactions = LoadTransactions();
    
        if (transactions.Count == 0)
        {
            Console.WriteLine("No borrowing history found.");
            return;
        }
    
        Console.WriteLine("{0,-15} {1,-10} {2,-10} {3,-15} {4,-15} {5,-10}",
            "Transaction ID", "Book ID", "Member ID", "Issue Date", "Due Date", "Status");
    
        foreach (var t in transactions)
        {
            string returnStatus = t.ReturnDate.HasValue ? "Returned" : "Issued";
            Console.WriteLine("{0,-15} {1,-10} {2,-10} {3,-15} {4,-15} {5,-10}",
                t.TransactionId, t.BookId, t.MemberId, 
                t.IssueDate.ToString("yyyy-MM-dd"), 
                t.DueDate.ToString("yyyy-MM-dd"), 
                returnStatus);
        }
    }
    
    public void DisplayOverdueBooks()
    {
        var transactions = LoadTransactions();
        var overdueTransactions = transactions
            .Where(t => t.Status == "Issued" && DateTime.Now > t.DueDate)
            .ToList();
    
        if (overdueTransactions.Count == 0)
        {
            Console.WriteLine("No overdue books found.");
            return;
        }
    
        Console.WriteLine($"Found {overdueTransactions.Count} overdue book(s):");
        Console.WriteLine("{0,-15} {1,-10} {2,-10} {3,-15} {4,-15} {5,-10} {6,-10}",
            "Transaction ID", "Book ID", "Member ID", "Issue Date", "Due Date", "Days Overdue", "Estimated Fine");
    
        foreach (var t in overdueTransactions)
        {
            int daysOverdue = (int)(DateTime.Now - t.DueDate).TotalDays;
            decimal estimatedFine = daysOverdue * 1.0m; // $1 per day
        
            Console.WriteLine("{0,-15} {1,-10} {2,-10} {3,-15} {4,-15} {5,-10} {6,-10:C}",
                t.TransactionId, t.BookId, t.MemberId,
                t.IssueDate.ToString("yyyy-MM-dd"),
                t.DueDate.ToString("yyyy-MM-dd"),
                daysOverdue, estimatedFine);
        }
    }
}