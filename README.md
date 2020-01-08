- https://joonasw.net/view/defining-permissions-and-roles-in-aad
- https://github.com/AzureAD/microsoft-authentication-library-for-dotnet/wiki/Acquiring-tokens-with-authorization-codes-on-web-apps
- https://auth0.com/blog/introduction-to-microservices-part-4-dependencies/
- https://stackoverflow.com/questions/46566717/net-core-2-0-get-aad-access-token-to-use-with-microsoft-graph
- https://joonasw.net/view/aspnet-core-2-azure-ad-authentication
- https://docs.microsoft.com/en-us/samples/azure-samples/active-directory-dotnet-webapp-webapi-openidconnect/active-directory-dotnet-webapp-webapi-openidconnect/
- https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-v2-aspnet-core-webapp
- https://dzone.com/articles/azures-infrastructure-as-code-azure-resource-manag
- https://www.ovh.com/blog/getting-external-traffic-into-kubernetes-clusterip-nodeport-loadbalancer-and-ingress/

### Projects
- md.backend.gateway (http,8301)
- md.emailservice (http, 8302)
- md.frontend (http, 8303)
- md.service

### Workflows
- md.service (oauth2 client_credentils) -> md.backend.gateway -> md.emailservice
- md.frontend (openidconnect + access_token) -> md.backend.gateway -> md.emailservice

### ToDos
- docker
- k8s
- sf
- aci
- local environment
- arm templates
- azure k8s cluster for production and staging
- monitoring, logging
- setup script
- actors
- grpc
- secrets
- managed identities
- rbac