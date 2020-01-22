#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Tickets.Daemon/Tickets.Daemon.csproj", "Tickets.Daemon/"]
COPY ["Common/Common.csproj", "Common/"]
COPY ["Tickets.Common/Tickets.Common.csproj", "Tickets.Common/"]
RUN dotnet restore "Tickets.Daemon/Tickets.Daemon.csproj"
RUN dotnet restore "Common/Common.csproj"
RUN dotnet restore "Tickets.Common/Tickets.Common.csproj"
COPY . .
WORKDIR "/src/Tickets.Daemon"
RUN dotnet build "Tickets.Daemon.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Tickets.Daemon.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tickets.Daemon.dll"]