FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Order-api/Order-api.csproj", "src/Order-api/"]
RUN dotnet restore "src/Order-api/Order-api.csproj"
COPY . .
WORKDIR "/src/src/Order-api"
RUN dotnet build "Order-api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Order-api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Order-api.dll"]