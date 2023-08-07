FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/produto-api/Produto-api.csproj", "src/produto-api/"]
RUN dotnet restore "src/produto-api/Produto-api.csproj"
COPY . .
WORKDIR "/src/src/produto-api"
RUN dotnet build "Produto-api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Produto-api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Produto-api.dll"]