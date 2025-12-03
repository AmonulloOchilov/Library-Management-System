namespace Library_Management_System.Services;
using Library_Management_System.Entities;
using System.Text.Json;

public class BookService
{
    public readonly string filePath;

    public BookService(string customDataPath = null)
    {
        string dataFolder = customDataPath ?? "/Users/amonulloochilov/Desktop/Library Management System/Library Management System/Data";
        if (!Directory.Exists(dataFolder))
        {
            Directory.CreateDirectory(dataFolder);
        }
        filePath = Path.Combine(dataFolder, "books.json");
    }
    
    
    public List<Book> LoadBooks()
    {
        if (!File.Exists(filePath))
        {
            return new List<Book>();
        }
            

        string json = File.ReadAllText(filePath);
        if (string.IsNullOrWhiteSpace(json))
        {
            return new List<Book>();
        }
        
        var result = JsonSerializer.Deserialize<List<Book>>(json);
        if (result != null)
        {
            return result;
        }
        else
        {
            return new List<Book>();
        }
    }
    
    public void SaveBooks(List<Book> books)
    {
        string json = JsonSerializer.Serialize(books, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, json);
    }

    public void AddBook(string title, string author,string isbn, int totalQuantity)
    {
        var books = LoadBooks();
        int newBookId = books.Count > 0 ? books.Max(b => b.BookId) + 1 : 1;
        var book = new Book()
        {
            BookId = newBookId,
            Title = title,
            Author = author,
            ISBN = isbn,
            TotalQuantity = totalQuantity,
            AvailableQuantity = totalQuantity
        };
        books.Add(book);
        SaveBooks(books);
        Console.WriteLine("Book is saved successfully!");
    }

    public void ViewAllBooks()
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine("No books found");
            return;
        }

        string json = File.ReadAllText(filePath);
        var books = LoadBooks();
        if (string.IsNullOrEmpty(json) || books.Count == 0)
        {
            Console.WriteLine("No Books available");
            return;
        }

        Console.WriteLine("{0,-10} {1,-23} {2,-23} {3,-17} {4,-22} {5, -20}",
            "ID", "Title", "Author", "ISBN", "Total Quantity", "Available Quantity");

        foreach (var b in books)
        {
            Console.WriteLine("{0,-10} {1,-23} {2,-23} {3,-17} {4,-22} {5, -20}",
                b.BookId, b.Title, b.Author, b.ISBN, b.TotalQuantity, b.AvailableQuantity);
        }
    }

    public List<Book> SearchBooks(string searchTerm)
    {
        var books = LoadBooks();
        return books.Where(book =>
            book.Title.ToLower().Contains(searchTerm) || book.Author.ToLower().Contains(searchTerm) ||
            book.ISBN.ToLower().Contains(searchTerm)).ToList();
    }
    public void UpdateBook(Book updatedBook)
    {
        var books = LoadBooks();
        var bookIndex = books.FindIndex(b => b.BookId == updatedBook.BookId);
    
        if (bookIndex >= 0)
        {
            books[bookIndex] = updatedBook;
            SaveBooks(books);
        }
    }
}