using System.Text.Json.Serialization;

namespace Library_Management_System.Entities;

public class Book
{
    public int BookId { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string ISBN { get; set; }
    public int TotalQuantity { get; set; }
    public int AvailableQuantity { get; set; }
    [JsonIgnore]
    public bool IsAvailable => AvailableQuantity > 0;
}