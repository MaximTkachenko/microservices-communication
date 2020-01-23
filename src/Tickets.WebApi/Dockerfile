#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Tickets.WebApi/Tickets.WebApi.csproj", "Tickets.WebApi/"]
RUN dotnet restore "Tickets.WebApi/Tickets.WebApi.csproj"
COPY . .
WORKDIR "/src/Tickets.WebApi"
RUN dotnet build "Tickets.WebApi.csproj" -c Release -o /app/build --no-restore

FROM build AS publish
RUN dotnet publish "Tickets.WebApi.csproj" -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tickets.WebApi.dll"]