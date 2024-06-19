namespace InteractiveCodeLab.Models;

public record RegisterRequest(
    string FirstName,
    string LastName,
    string Email,
    string Password);