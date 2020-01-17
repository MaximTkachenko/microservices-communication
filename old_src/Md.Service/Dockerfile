FROM mcr.microsoft.com/dotnet/core/runtime:3.0-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["Md.Service/Md.Service.csproj", "Md.Service/"]
RUN dotnet restore "Md.Service/Md.Service.csproj"
COPY . .
WORKDIR "/src/Md.Service"
RUN dotnet build "Md.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Md.Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Md.Service.dll"]