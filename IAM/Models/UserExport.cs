namespace IAM.Models;

public class UserExport
{
  public string Username { get; set; }
  public string Email { get; set; }
  public string FirstName { get; set; }
  public string LastName { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
  public bool IsActive { get; set; } = true;
}