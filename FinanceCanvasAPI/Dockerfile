# Use the official .NET 9.0 SDK image for build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

# Use the official .NET 9.0 ASP.NET runtime image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENV db:SQLCIPHER_KEY=password123
EXPOSE 8080
ENTRYPOINT ["dotnet", "FinanceCanvasAPI.dll"]
