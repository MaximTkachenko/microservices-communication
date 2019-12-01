- https://joonasw.net/view/defining-permissions-and-roles-in-aad
- https://github.com/AzureAD/microsoft-authentication-library-for-dotnet/wiki/Acquiring-tokens-with-authorization-codes-on-web-apps

### Projects
- md.backend.gateway (9901)
- md.emailservice (9902)
- md.frontend (9903)
- md.service

### Workflows
- md.service (oauth2 cliemt_credentils) -> md.backend.gateway -> md.emailservice
- md.frontend (openidconnect + access_token) -> md.backend.gateway -> md.emailservice

### ToDos
- docker
- k8s
- arm templates
- monitoring
- setup script