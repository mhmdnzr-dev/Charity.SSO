using Duende.IdentityServer.Models;


namespace CharityHub.SSO;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };
    

    public static IEnumerable<Client> Clients =>
    new List<Client>
    {
       new Client
        {
            ClientId = "CharityHubAdminPanelDevClient",
            ClientSecrets = { new Secret("K8T1L7J9V0D3R+4W6Fz5X2Q8B1N7P3C4G0A9J7R8H6=".Sha256()) },
            AllowedGrantTypes = GrantTypes.Code,
            RequirePkce = true,
            AllowOfflineAccess = true,
            RedirectUris = { "https://localhost:7091/signin-oidc" },
            FrontChannelLogoutUri = "https://localhost:7091/signout-oidc",
            PostLogoutRedirectUris = { "https://localhost:7091/signout-callback" },
            AllowedScopes = {
                "openid",
                "profile",
                "roles",
                "name"
            },
            AccessTokenLifetime = 3600, // 1 hour
            IdentityTokenLifetime = 300, // 5 minutes
            AbsoluteRefreshTokenLifetime = 2592000, // 30 days
            SlidingRefreshTokenLifetime = 1296000, // 15 days
            RequireClientSecret = true,
            ClientName = "CharityHubAdminDevUI",
            RequireConsent = false,
            AllowRememberConsent = true,
            AlwaysIncludeUserClaimsInIdToken = true, // Ensure claims are included
            AllowAccessTokensViaBrowser = false,
            Enabled = true
        },
        new Client
        {
            ClientId = "CharityHubAdminPanelProdClient",
            ClientSecrets = { new Secret("P7L3J9V0D1R+6W8F5X2Q4B1N7P0C8G9A6J7R3H5=".Sha256()) },
            AllowedGrantTypes = GrantTypes.Code,
            RequirePkce = true,
            AllowOfflineAccess = true,
            RedirectUris = { "http://185.137.27.35:5050/signin-oidc" },
            FrontChannelLogoutUri = "http://185.137.27.35:5050/signout-oidc",
            PostLogoutRedirectUris = { "http://185.137.27.35:5050/signout-callback" },
            AllowedScopes = {
                "openid",
                "profile",
                "roles",
                "name",
            },
            AccessTokenLifetime = 3600, // 1 hour
            IdentityTokenLifetime = 300, // 5 minutes
            AbsoluteRefreshTokenLifetime = 2592000, // 30 days
            SlidingRefreshTokenLifetime = 1296000, // 15 days
            RequireClientSecret = true,
            ClientName = "CharityHubAdminProdUI",
            RequireConsent = false,
            AllowRememberConsent = true,
            AlwaysIncludeUserClaimsInIdToken = true, // Ensure claims are included
            AllowAccessTokensViaBrowser = false,
            Enabled = true
        }
    };


}
