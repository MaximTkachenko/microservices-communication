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