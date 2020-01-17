FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["Md.EmailService/Md.EmailService.csproj", "Md.EmailService/"]
COPY ["Md.Common/Md.Common.csproj", "Md.Common/"]
RUN dotnet restore "Md.EmailService/Md.EmailService.csproj"
COPY . .
WORKDIR "/src/Md.EmailService"
RUN dotnet build "Md.EmailService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Md.EmailService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Md.EmailService.dll"]