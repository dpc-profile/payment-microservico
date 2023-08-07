FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/client-app/Client-app.csproj", "src/client-app/"]
RUN dotnet restore "src/client-app/Client-app.csproj"
COPY . .
WORKDIR "/src/src/client-app"
RUN dotnet build "Client-app.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Client-app.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Client-app.dll"]