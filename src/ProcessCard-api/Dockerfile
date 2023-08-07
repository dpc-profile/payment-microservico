FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/ProcessCard-api/ProcessCard-api.csproj", "src/ProcessCard-api/"]
RUN dotnet restore "src/ProcessCard-api/ProcessCard-api.csproj"
COPY . .
WORKDIR "/src/src/ProcessCard-api"
RUN dotnet build "ProcessCard-api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ProcessCard-api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProcessCard-api.dll"]