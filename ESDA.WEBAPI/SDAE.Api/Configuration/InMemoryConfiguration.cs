using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace SDAE.Api.Configuration
{
    public static class InMemoryConfiguration
    {
        public static IEnumerable<ApiResource> ApiResources()
        {
            return new[]
            {
                new ApiResource("SDAE.Api", "SDAE Api"),
                //new ApiResource("SDAE.Web.AgentPortal", "Agent Portal"),
                //new ApiResource("SDAE.AgentPortal.Server","Agent Portal Server"),
                new ApiResource(IdentityServerConstants.LocalApi.ScopeName),
            };
        }
        public static IEnumerable<IdentityResource> ApiIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
            };
        }

        public static IEnumerable<Client> ApiClient()
        {
            //var code = GrantTypes.Code;
            //Implicit
            return new[]
            {
                 new Client{
                    ClientId="SDAE_Web",
                    ClientSecrets=new[]{ new Secret("secret".Sha256()) },
                    AllowedGrantTypes=GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    AllowedScopes=new []{
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,"SDAE.Api",
                        IdentityServerConstants.LocalApi.ScopeName
                    },
                    AllowOfflineAccess=true,
                     //Access token life time is  36000 seconds (10 hour)
                    AccessTokenLifetime = 36000,
                    //TODO: KK remove the hard coded values and replace from identity settings
                     AllowedCorsOrigins = { "http://localhost:4200", "http://testesda.techraq.com", "http://esda.techraq.com"}
                     //http://esda.techraq.com
                     //http://testesda.techraq.com
                     //http://localhost:4200
                }
            };
        }
    }
}
