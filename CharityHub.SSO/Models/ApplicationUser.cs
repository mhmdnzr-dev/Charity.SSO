﻿// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.


using Microsoft.AspNetCore.Identity;

namespace CharityHub.SSO.Models;
// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser<int>
{

    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    public bool IsActive { get;  set; }
}
