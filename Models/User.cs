using System.ComponentModel.DataAnnotations;

namespace UserAuthApi.Models;

public class User {
    public int Id { get; set; }
    
    [Required]
    public string Firstname { get; set; }
    
    [Required]
    public string Lastname { get; set; }
    
    [Required, EmailAddress]
    public string Email { get; set; }
    
    [Required]
    public string Password { get; set; }

    public string Role { get; set; }

    public bool Active { get; set; }
}