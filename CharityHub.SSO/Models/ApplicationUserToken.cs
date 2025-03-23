using Microsoft.AspNetCore.Identity;

namespace CharityHub.SSO.Models;


public class ApplicationUserToken : IdentityUserToken<int>
{
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiration { get; set; }
}
