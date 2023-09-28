using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Repositories.Models.Users;

[Index(nameof(Username))]
public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)] // Define a suitable maximum length for your username
    public string Username { get; set; }

    [Required]
    [MaxLength(100)] // Define a suitable maximum length for your email
    public string Email { get; set; }

    [Required]
    [MaxLength(100)] // Define a suitable maximum length for hashed password storage
    public string PasswordHash { get; set; }

    [MaxLength(50)]
    public string FirstName { get; set; }

    [MaxLength(50)]
    public string LastName { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsActive { get; set; } = true;

    public bool IsAdmin { get; set; } = false;
}