using System.ComponentModel.DataAnnotations;

namespace UserAuthApi.DTOs;

public class UserDTO {
    [Required]
    public required string Firstname { get; set; }
    
    [Required]
    public required string Lastname { get; set; }
    
    [Required, EmailAddress]
    public required string Email { get; set; }

    public string Role { get; set; } = "default";

    public bool Active { get; set; } = true;
}