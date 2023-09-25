using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using IdentityModel;
using System.Collections.Immutable;

namespace Identity.WebApi.InMemory;

public static class InMemoryConfiguration
{
    private static IEnumerable<ApiResource> _apis = new List<ApiResource>
    {
        new ApiResource
        {
            Name = "apigateway",
            DisplayName = "apigateway",
            UserClaims = { JwtClaimTypes.Name }
        },
    }.ToImmutableList();

    public static IEnumerable<ApiResource> Apis { get => _apis; } 

    private static IEnumerable<Client> _clients = new List<Client>
    {
        new Client
        {
            ClientId = "apigateway",
            ClientName = "API Gateway",

            ClientSecrets = { new Secret("apigateway_top_secret".ToSha256())},
            AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
            AllowedScopes = 
            { 
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile,
                "apigateway" 
            },
            RedirectUris =
            {
                "http://.../signin-oidc"
            },
            PostLogoutRedirectUris =
            {
                "http://.../signout-oidc"
            },
            AllowAccessTokensViaBrowser = true,
        },
    }.ToImmutableList();

    public static IEnumerable<Client> Clients { get => _clients; }

    private static IEnumerable<IdentityResource> _identityResources = new List<IdentityResource>
    {
        new IdentityResources.OpenId(),
        new IdentityResources.Profile()
    }.ToImmutableList();

    public static IEnumerable<IdentityResource> IdentityResources { get => _identityResources; }
}
