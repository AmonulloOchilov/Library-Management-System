namespace Library_Management_System.Entities;

public class Category
{
    public int CategoryId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<Book> Books { get; set; }
}