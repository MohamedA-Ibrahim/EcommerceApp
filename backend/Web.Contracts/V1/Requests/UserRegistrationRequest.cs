using System.ComponentModel.DataAnnotations;

namespace Web.Contracts.V1.Requests;

public class UserRegistrationRequest
{
    [EmailAddress]
    public string Email { get; set; }

    public string Password { get; set; }

    public string Phone { get; set; }
    public string ProfileName { get; set; }
}