#Docker file template
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /source
COPY . ./
RUN dotnet restore --disable-parallel
RUN dotnet publish -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app ./

EXPOSE 80

ENTRYPOINT ["dotnet", "application.dll"]