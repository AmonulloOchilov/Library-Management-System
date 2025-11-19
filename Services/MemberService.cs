using System.Text.Json;
using Library_Management_System.Entities;

namespace Library_Management_System.Services;

public class MemberService
{
    public readonly string filePath;

    public MemberService()
    {
        string dataFolder = "/Users/amonulloochilov/Desktop/Library Management System/Library Management System/Data";
        if (!Directory.Exists(dataFolder))
        {
            Directory.CreateDirectory(dataFolder);
        }

        filePath = Path.Combine(dataFolder, "members.json");
    }

    private List<Member> LoadMembers()
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

    public void AddMembers(int memberId, string firstName, string lastName, string email, string phoneNumber,
        DateTime membershipDate)
    {
        var members = LoadMembers();
        var member = new Member()
        {
            MemberId = memberId,
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Phone = phoneNumber,
            MembershipDate = membershipDate
        };
        members.Add(member);
        SaveMembers(members);
        Console.WriteLine("Member is saved successfully");
    }
}