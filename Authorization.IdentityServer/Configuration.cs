using IdentityServer4.Models;
using IdentityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4;

namespace Authorization.IdentityServer
{
    public static class Configuration
    {
        public static IEnumerable<Client> GetClients() =>
        new List<Client>
        {
            //как клиенты будут общаться -в нашем случае Users будут делать запросы на Orders
            new Client
            {
                 ClientId = "client_id",
                ClientSecrets = { new Secret("client_secret".ToSha256()) },
                //по какому принципу будет авторизовываться
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                //какие скопы разрешены для подключения
                AllowedScopes =
                {
                    "OrdersAPI"
                }
            },
            new Client
            {
                ClientId = "client_id_mvc",
                ClientSecrets = { new Secret("client_secret_mvc".ToSha256()) },
                AllowedGrantTypes = GrantTypes.Code,
                AllowedScopes =
                {
                    "OrdersAPI",
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile
                    //"openid"

                },
                RedirectUris = {"https://localhost:2001/signin-oidc"},

                RequireConsent = false

            }
        };

        public static IEnumerable<IdentityResource> GetIdentityResourses() =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };

        public static IEnumerable<ApiResource> GetApiResources() =>
            new List<ApiResource>
            //создаем ресурсы Order.Api и Users.Api - это ресурсы
            {
                new ApiResource("OrdersAPI")
                //причем в AllowedScopes - "OrdersAPI" и ApiResource("OrdersAPI") - OrdersAPI это одно и то же
                //new Client можно ходить на этот ресурс new ApiResource("OrdersAPI")
            };
        /// <summary>
        /// IdentityServer4 version 4.x.x changes
        /// </summary>
        /// <returns></returns>
        //public static IEnumerable<ApiScope> GetApiScopes()
        //{
        //    yield return new ApiScope("SwaggerAPI", "Swagger API");
        //    yield return new ApiScope("blazor", "Blazor WebAssembly");
        //    yield return new ApiScope("OrdersAPI", "Orders API");
        //}
    }
}
