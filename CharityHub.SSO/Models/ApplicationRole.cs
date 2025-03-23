using Microsoft.AspNetCore.Identity;

namespace CharityHub.SSO.Models;

public class ApplicationRole : IdentityRole<int>
{
    public ApplicationRole(string roleName) : base(roleName)
    {
    }

    public ApplicationRole() : base()
    {
    }
}