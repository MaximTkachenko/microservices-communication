FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["Md.Frontend/Md.Frontend.csproj", "Md.Frontend/"]
COPY ["Md.Common/Md.Common.csproj", "Md.Common/"]
RUN dotnet restore "Md.Frontend/Md.Frontend.csproj"
COPY . .
WORKDIR "/src/Md.Frontend"
RUN dotnet build "Md.Frontend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Md.Frontend.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Md.Frontend.dll"]