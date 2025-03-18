using System.ComponentModel.DataAnnotations;

namespace UserAuthApi.Models;

public class User {
    public int Id { get; set; }
    
    [Required]
    public required string Firstname { get; set; }
    
    [Required]
    public required string Lastname { get; set; }
    
    [Required, EmailAddress]
    public required string Email { get; set; }
    
    [Required]
    public required string Password { get; set; }

    [Required]
    public string Role { get; set; } = "default";

    public bool Active { get; set; } = true;
}