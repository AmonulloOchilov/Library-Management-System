namespace Library_Management_System.Entities;

public class Author
{
    public int AuthorId { get; set; }
    public string Name { get; set; }
    public string Biography { get; set; }
    // Collection of books by this author
    public List<Book> Books { get; set; }
}