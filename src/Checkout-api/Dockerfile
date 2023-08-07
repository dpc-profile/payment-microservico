FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/checkout-api/Checkout-api.csproj", "src/checkout-api/"]
RUN dotnet restore "src/checkout-api/Checkout-api.csproj"
COPY . .
WORKDIR "/src/src/checkout-api"
RUN dotnet build "Checkout-api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Checkout-api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Checkout-api.dll"]