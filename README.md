- https://joonasw.net/view/defining-permissions-and-roles-in-aad
- https://github.com/AzureAD/microsoft-authentication-library-for-dotnet/wiki/Acquiring-tokens-with-authorization-codes-on-web-apps
- https://auth0.com/blog/introduction-to-microservices-part-4-dependencies/
- https://stackoverflow.com/questions/46566717/net-core-2-0-get-aad-access-token-to-use-with-microsoft-graph
- https://joonasw.net/view/aspnet-core-2-azure-ad-authentication

### Projects
- md.backend.gateway (https, 44301)
- md.emailservice (http, 44302)
- md.frontend (https, 44303)
- md.service

### Workflows
- md.service (oauth2 client_credentils) -> md.backend.gateway -> md.emailservice
- md.frontend (openidconnect + access_token) -> md.backend.gateway -> md.emailservice

### ToDos
- docker
- k8s
- arm templates
- monitoring
- setup script