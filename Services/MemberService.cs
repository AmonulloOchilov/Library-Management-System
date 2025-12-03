using System.Text.Json;
using Library_Management_System.Entities;

namespace Library_Management_System.Services;

public class MemberService
{
    public readonly string filePath;

    public MemberService(string customDataPath = null)
    {
        string dataFolder = customDataPath ?? "/Users/amonulloochilov/Desktop/Library Management System/Library Management System/Data";
        if (!Directory.Exists(dataFolder))
        {
            Directory.CreateDirectory(dataFolder);
        }
        filePath = Path.Combine(dataFolder, "members.json");
    }

    public List<Member> LoadMembers()
    {
        if (!File.Exists(filePath))
        {
            return new List<Member>();
        }

        string json = File.ReadAllText(filePath);
        if (string.IsNullOrWhiteSpace(json))
        {
            return new List<Member>();
        }

        var result = JsonSerializer.Deserialize<List<Member>>(json);
        if (result != null)
        {
            return result;
        }
        else
        {
            return new List<Member>();
        }
    }

    private void SaveMembers(List<Member> members)
    {
        string json = JsonSerializer.Serialize(members, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath,json);
    }

    public void AddMembers(string firstName, string lastName, string email, string phoneNumber)
    {
        var members = LoadMembers();
        int newMemberId = members.Count > 0 ? members.Max(m => m.MemberId) + 1 : 1;
        var member = new Member()
        {
            MemberId = newMemberId,
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Phone = phoneNumber,
            MembershipDate = DateTime.Now
        };
        members.Add(member);
        SaveMembers(members);
        Console.WriteLine("Member is saved successfully");
    }

    public void ViewAllMembers()
    {
        Console.WriteLine("View All Members:");
        var members = LoadMembers();
        if (members.Count == 0) 
        {
            Console.WriteLine("No members available");
            return;
        }
        Console.WriteLine("{0,-10} {1,-15} {2,-15} {3,-22} {4,-22} {5, -20}",
            "ID", "Name", "Surname", "Email", "Phone Number", "Membership Date");

        foreach (var m in members)
        {
            Console.WriteLine("{0,-10} {1,-15} {2,-15} {3,-22} {4,-22} {5, -20}",
                m.MemberId, m.FirstName, m.LastName, m.Email, m.Phone, m.MembershipDate);
        }
    }
    
}