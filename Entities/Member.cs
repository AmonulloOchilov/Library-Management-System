namespace Library_Management_System.Entities;

public class Member
{
    public int MemberId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public DateTime MembershipDate { get; set; }
    public bool IsActive { get; set; } = true;
}