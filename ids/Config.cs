using IdentityServer4.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Test;

namespace ids
{
    public static class Config
    {
        public static List<TestUser> Users
        {
            get
            {
                var address = new
                {
                    street_address = "One Hacker Way",
                    locality = "Heidelberg",
                    postal_code = 69118,
                    country = "Germany"
                };

                return new List<TestUser>
        {
          new TestUser
          {
            SubjectId = "818727",
            Username = "alice",
            Password = "alice",
            Claims =
            {
              new Claim(JwtClaimTypes.Name, "Alice Smith"),
              new Claim(JwtClaimTypes.GivenName, "Alice"),
              new Claim(JwtClaimTypes.FamilyName, "Smith"),
              new Claim(JwtClaimTypes.Email, "AliceSmith@email.com"),
              new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
              new Claim(JwtClaimTypes.Role, "admin"),
              new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
              new Claim(JwtClaimTypes.Address, JsonSerializer.Serialize(address),
                IdentityServerConstants.ClaimValueTypes.Json)
            }
          },
          new TestUser
          {
            SubjectId = "88421113",
            Username = "bob",
            Password = "bob",
            Claims =
            {
              new Claim(JwtClaimTypes.Name, "Bob Smith"),
              new Claim(JwtClaimTypes.GivenName, "Bob"),
              new Claim(JwtClaimTypes.FamilyName, "Smith"),
              new Claim(JwtClaimTypes.Email, "BobSmith@email.com"),
              new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
              new Claim(JwtClaimTypes.Role, "user"),
              new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
              new Claim(JwtClaimTypes.Address, JsonSerializer.Serialize(address),
                IdentityServerConstants.ClaimValueTypes.Json)
            }
          }
        };
            }
        }

        public static IEnumerable<IdentityResource> IdentityResources =>
          new[]
          {
        new IdentityResources.OpenId(),
        new IdentityResources.Profile(),
        new IdentityResource
        {
          Name = "role",
          UserClaims = new List<string> {"role"}
        }
          };

        public static IEnumerable<ApiScope> ApiScopes =>
          new[]
          {
            new ApiScope("weatherapi.read"),
            new ApiScope("weatherapi.write"),
            new ApiScope("fhirUser"),
            new ApiScope("online_access"),
            new ApiScope("launch"),
            new ApiScope("launch/patient"),
            new ApiScope("patient/encounter"),
            new ApiScope("patient/resource"),
            new ApiScope("patient/Patient.read"),
            new ApiScope("patient/Patient.write"),
            new ApiScope("patient/DocumentReference.read"),
            new ApiScope("patient/DocumentReference.write")
          };
        public static IEnumerable<ApiResource> ApiResources => new[]
        {

            new ApiResource("weatherapi")
              {
                Scopes = new List<string> {"weatherapi.read", "weatherapi.write", "launch", "launch/*.*", "patient/*.*"},
                ApiSecrets = new List<Secret> {new Secret("ScopeSecret".Sha256())},
                UserClaims = new List<string> {"role"}
            },
            new ApiResource("dips-fhir-4")
            {
                Scopes = new List<string> {"launch", "launch/patient"},
                ApiSecrets = new List<Secret> {new Secret("ScopeSecret".Sha256())},
                UserClaims = new List<string> {"fhirUser"}
            }
    };

        public static IEnumerable<Client> Clients =>
          new[]
          {
        // m2m client credentials flow client
        new Client
        {
          ClientId = "m2m.client",
          ClientName = "Client Credentials Client",

          AllowedGrantTypes = GrantTypes.ClientCredentials,
          ClientSecrets = {new Secret("SuperSecretPassword".Sha256())},

          AllowedScopes = {"weatherapi.read", "weatherapi.write"}
        },

        new Client
        {
            ClientId = "dips-ehr-pdf",
            ClientSecrets = {new Secret("SuperSecretPassword".Sha256())},
            AllowedGrantTypes = GrantTypes.Code,

            RedirectUris = {
                "https://oauthdebugger.com/debug",
                "http://localhost:8889/app",
                "http%3A%2F%2Flocalhost%3A8889%2Fapp"},
                FrontChannelLogoutUri = "https://localhost:5444/signout-oidc",
            PostLogoutRedirectUris = {"https://localhost:5444/signout-callback-oidc"},

            AllowOfflineAccess = true,
            AllowedScopes = {
                "openid",
                "launch",
                "online_access",
                "fhirUser",
                "launch/patient",
                "patient/DocumentReference.write",
                "patient/DocumentReference.read",
                "patient/Patient.read",
                "patient/encounter",
                "patient/resource",                
            },
            RequirePkce = false,
            RequireConsent = true,
            AllowPlainTextPkce = false
        },
        
        // interactive client using code flow + pkce
        /*new Client
        {
          ClientId = "interactive",
          ClientSecrets = {new Secret("SuperSecretPassword".Sha256())},

          AllowedGrantTypes = GrantTypes.Code,

          RedirectUris = {
                "https://localhost:5444/signin-oidc", 
                "https://oauthdebugger.com/debug"
          },
          FrontChannelLogoutUri = "https://localhost:5444/signout-oidc",
          PostLogoutRedirectUris = {"https://localhost:5444/signout-callback-oidc"},

          AllowOfflineAccess = true,
          AllowedScopes = {"openid", "profile", "weatherapi.read", "launch", "patient/*.*"},
          RequirePkce = false,
          RequireConsent = true,
          AllowPlainTextPkce = false
        }*/
        };
    }
}