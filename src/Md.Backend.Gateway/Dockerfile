FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["Md.Backend.Gateway/Md.Backend.Gateway.csproj", "Md.Backend.Gateway/"]
COPY ["Md.Common/Md.Common.csproj", "Md.Common/"]
RUN dotnet restore "Md.Backend.Gateway/Md.Backend.Gateway.csproj"
COPY . .
WORKDIR "/src/Md.Backend.Gateway"
RUN dotnet build "Md.Backend.Gateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Md.Backend.Gateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Md.Backend.Gateway.dll"]