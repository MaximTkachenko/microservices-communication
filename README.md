## Projects
- `Portal` - web application, AzureAd OpenIdConnect, port `5001`
- `User.WebApi` - api microservice used to manage users and claims, JWT, port `5002`
- `Glossary.WebApi` - api microservice used to manage common data like cusromers, offices etc., JWT, port `5003`
- `Tickets.WebApi`- api microservice to manage tickets, JWT, port `5004`
- `Tickets.Daemon` - daemon microservice to process asynchronous operations, JWT, port `5005`

## Hosts file for local development
```
127.0.0.1 users-api
127.0.0.1 glossary-api
127.0.0.1 tickets-api
```

## sqlserver
Run sqlserver for linux in docker:
```bat
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=P@ssw0rd12345!!" --name sqlserver -p 1434:1433 -d -v C:\temp\sqlserver-docker\data:/var/opt/mssql/data -v C:\temp\sqlserver-docker\log:/var/opt/mssql/log -v C:\temp\sqlserver-docker\secrets:/var/opt/mssql/secrets mcr.microsoft.com/mssql/server:2019-latest
```
Test connection:
```bat
sqlcmd -S 127.0.0.1,1434 -U SA -P P@ssw0rd12345!!
```
Connect from docker container
```bat
Server=host.docker.internal,1434;Database=UsersDb;User ID=sa;Password=P@ssw0rd12345!!
```
Connect from host machine
```bat
Server=127.0.0.1,1434;Database=UsersDb;User ID=sa;Password=P@ssw0rd12345!!
```

## ToDos
- cache tokens in web app
- validate tokens from different tenants
- accesstoken and refreshtoken expiration
- access from  different tenants
- proper authorization in microservices
- local environment (ports etc.)
- docker
- k8s
- sf
- aci
- arm templates
- azure k8s cluster for production and staging
- monitoring, logging
- setup script
- actors
- grpc
- secrets
- managed identities
- rbac

JWT sample for delegated permissions:
```json
{
    "typ": "JWT",
    "alg": "RS256",
    "x5t": "piVlloQDSMKxh1m2ygqGSVdgFpA",
    "kid": "piVlloQDSMKxh1m2ygqGSVdgFpA"
}.{
    "aud": "api://theapp.api",
    "iss": "https://sts.windows.net/6b9be1b6-4f80-4ce7-8479-16c4d7726470/",
    "iat": 1579196108,
    "nbf": 1579196108,
    "exp": 1579200008,
    "acr": "1",
    "aio": "AVQAq/8OAAAAHLg2UT5qfZ230dYPJdzk14ooexDdZowHBfKshArz7hAc1CVrWZQ1VzjPmk1eT6Os1+wC7zGXf32LiPCWKJ+as63NbWZ9CoqCneXhNWbcRtY=",
    "amr": [
    "pwd"
    ],
    "appid": "b021b14e-1671-4fe6-b7cc-0a67a248543f",
    "appidacr": "1",
    "email": "oblomov86@gmail.com",
    "family_name": "Tkachenko",
    "given_name": "Maxim",
    "idp": "live.com",
    "ipaddr": "51.174.85.2",
    "name": "Maxim Tkachenko",
    "oid": "03526494-16e1-4e21-99a5-9d734186092e",
    "scp": "Tickets UsersAndClaims",
    "sub": "hI_OiH4kmvVkzY_NU24aOlahR06Dul7zZe5smXJHM90",
    "tid": "6b9be1b6-4f80-4ce7-8479-16c4d7726470",
    "unique_name": "live.com#oblomov86@gmail.com",
    "uti": "IMYPesSovk2YXZAm5Og9AQ",
    "ver": "1.0"
}.[Signature]
```

JWT sample for application permissions:
```json
{
    "typ": "JWT",
    "alg": "RS256",
    "x5t": "piVlloQDSMKxh1m2ygqGSVdgFpA",
    "kid": "piVlloQDSMKxh1m2ygqGSVdgFpA"
}.{
    "aud": "api://theapp.api",
    "iss": "https://sts.windows.net/6b9be1b6-4f80-4ce7-8479-16c4d7726470/",
    "iat": 1579195869,
    "nbf": 1579195869,
    "exp": 1579199769,
    "aio": "42NgYNBT87n6dabGJua9Hbf/nX76DAA=",
    "appid": "da51a2ec-058f-4025-a75a-41af428be001",
    "appidacr": "1",
    "idp": "https://sts.windows.net/6b9be1b6-4f80-4ce7-8479-16c4d7726470/",
    "oid": "e3b6fc55-78eb-4dc9-9e74-dd6a1ddf11e9",
    "roles": [
    "Daemon"
    ],
    "sub": "e3b6fc55-78eb-4dc9-9e74-dd6a1ddf11e9",
    "tid": "6b9be1b6-4f80-4ce7-8479-16c4d7726470",
    "uti": "s3znyXXUUkWXBssYBL09AQ",
    "ver": "1.0"
}.[Signature]
```

## Links

- https://joonasw.net/view/defining-permissions-and-roles-in-aad
- https://github.com/AzureAD/microsoft-authentication-library-for-dotnet/wiki/Acquiring-tokens-with-authorization-codes-on-web-apps
- https://auth0.com/blog/introduction-to-microservices-part-4-dependencies/
- https://stackoverflow.com/questions/46566717/net-core-2-0-get-aad-access-token-to-use-with-microsoft-graph
- https://joonasw.net/view/aspnet-core-2-azure-ad-authentication
- https://docs.microsoft.com/en-us/samples/azure-samples/active-directory-dotnet-webapp-webapi-openidconnect/active-directory-dotnet-webapp-webapi-openidconnect/
- https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-v2-aspnet-core-webapp
- https://dzone.com/articles/azures-infrastructure-as-code-azure-resource-manag
- https://www.ovh.com/blog/getting-external-traffic-into-kubernetes-clusterip-nodeport-loadbalancer-and-ingress/
- https://www.dotnetcurry.com/devops/1518/aspnet-core-cicd-azure-kubernetes-service
- https://medium.com/google-cloud/kubernetes-nodeport-vs-loadbalancer-vs-ingress-when-should-i-use-what-922f010849e0
- https://theithollow.com/2019/02/05/kubernetes-service-publishing/

- https://medium.com/google-cloud/understanding-kubernetes-networking-pods-7117dd28727
- https://medium.com/google-cloud/understanding-kubernetes-networking-services-f0cb48e4cc82
- https://medium.com/google-cloud/understanding-kubernetes-networking-ingress-1bc341c84078

- https://gardener.cloud/050-tutorials/content/howto/service-access/

- https://joonasw.net/view/aspnet-core-2-azure-ad-authentication
- https://docs.microsoft.com/en-us/azure/architecture/multitenant-identity/web-api
- https://docs.microsoft.com/en-us/azure/active-directory/develop/setup-multi-tenant-app
- https://docs.microsoft.com/en-us/azure/active-directory/develop/v2-permissions-and-consent
- authorization and data protection
