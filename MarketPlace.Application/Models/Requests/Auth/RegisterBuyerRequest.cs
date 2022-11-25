using System.ComponentModel.DataAnnotations;

namespace MarketPlace.Application.Models.Requests.Auth;

public class RegisterBuyerRequest
{
    [Required] 
    public string FirstName { get; set; } = null!;

    [Required] 
    public string LastName { get; set; } = null!;

    [Required] 
    public string Phone { get; set; } = null!;

    [Required] 
    [EmailAddress] 
    public string Email { get; set; } = null!;

    [Required] 
    public string Password { get; set; } = null!; // _QGrXyvcmTD4aVQJ_ for tests

    [Required] 
    [Compare(nameof(Password))] 
    public string ConfirmPassword { get; set; } = null!;
}