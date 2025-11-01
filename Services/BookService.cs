namespace Library_Management_System.Services;
using Library_Management_System.Entities;
using System.Text.Json;

public class BookService
{
    public readonly string filePath;

    public BookService()
    {
        string dataFolder = "/Users/amonulloochilov/Desktop/Library Management System/Library Management System/Data";
        if (!Directory.Exists(dataFolder))
        {
            Directory.CreateDirectory(dataFolder);
        }

        filePath = Path.Combine(dataFolder, "books.json");
    }
    
    
    private List<Book> LoadBooks()
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
    
    private void SaveBooks(List<Book> books)
    {
        string json = JsonSerializer.Serialize(books, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, json);
    }

    public void AddBook(int bookId,string title, string author,string isbn, int totalQuantity)
    {
        var books = LoadBooks();
        var book = new Book()
        {
            BookId = bookId,
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
}