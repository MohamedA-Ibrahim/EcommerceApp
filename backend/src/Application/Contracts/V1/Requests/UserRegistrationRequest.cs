﻿using System.ComponentModel.DataAnnotations;

namespace Application.Contracts.V1.Requests;

public class UserRegistrationRequest
{
    [EmailAddress] 
    public string Email { get; set; }

    public string Password { get; set; }

    public string Phone { get; set; }
    public string ProfileName { get; set; }
}