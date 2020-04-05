# Motivation
It's a demo project to show:
- different [microservices patterns](https://microservices.io/patterns/index.html)
- how to configure local environment to develop microservices
- how to deploy microservices into Azure K8S
- how to create/update Azure infrastructure

# Projects
- `Portal` - web application, AzureAd OpenIdConnect, hosted as Azure web app, port `5001`
- `User.WebApi` - api microservice used to manage users and claims, JWT, hosted in AKS, port `5002`
- `Glossary.WebApi` - api microservice used to manage common data like cusromers, offices etc., JWT, hosted in AKS, port `5003`
- `Tickets.WebApi`- api microservice to manage tickets, JWT, hosted in AKS, port `5004`
- `Tickets.Daemon` - daemon microservice to process asynchronous operations, JWT, hosted in AKS, port `5005`

# Setup

## Azure app registration

TODO: add scripts ti regoster applications in AAD
- `theapp.portal`
- `theapp.api`
- `theapp.daemon`

## Azure resources

TODO scripts and/or ARM templates
- web app
- k8s cluster
- 3 databases (glossary, tickets, users)
- monitor

## Azure DevOps pipelines

TODO add pipelines

## Environments
- `Development` - local environment; applications can be executed in docker or on host machine
- `Staging` - Azure staging environment
- `Production` - Azure production environment

## Hosts file for local development
```
127.0.0.1 users-api
127.0.0.1 glossary-api
127.0.0.1 tickets-api
```
In case of dockerswarm and k8s these names are configured as service names.

## SQLServer
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

## Logs and metrics aggergation

### Local
- Seq

```bat
docker run -d --name seq -e ACCEPT_EULA=Y -v C:\temp\seq:/data -p 5341:80 datalust/seq:latest
```

- Azure monitor

## ToDos
- move client_id, client_secrets, connection_strings into [user secrets](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-3.1&tabs=windows)
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
- [microservices patterns](https://microservices.io/patterns/index.html) to implement

## Links

- https://github.com/Azure/azure-service-bus/tree/master/samples/DotNet/Microsoft.ServiceBus.Messaging/AtomicTransactions
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
- [authorization](https://docs.microsoft.com/en-us/aspnet/core/security/authorization/introduction?view=aspnetcore-3.1) and [data protection](https://docs.microsoft.com/en-us/aspnet/core/security/data-protection/introduction?view=aspnetcore-3.1)
- https://www.projectatomic.io/blog/2015/07/what-are-docker-none-none-images/
- https://joonasw.net/view/azure-ad-authentication-aspnet-core-api-part-1
- https://github.com/AzureAD/microsoft-authentication-library-for-dotnet/wiki/Acquiring-tokens-with-authorization-codes-on-web-apps
