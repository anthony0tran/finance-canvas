FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
ARG LITEDB_PASSWORD
WORKDIR /src
COPY ["FinanceCanvasAPI/FinanceCanvasAPI.csproj", "FinanceCanvasAPI/"]
COPY ["FinanceCanvasAPI/appsettings.json", "FinanceCanvasAPI/appsettings.json"]
RUN powershell -Command "(Get-Content FinanceCanvasAPI/appsettings.json) -replace '\"SQLCIPHER_KEY\": \"\"', '\"SQLCIPHER_KEY\": \"'$env:LITEDB_PASSWORD'\"' | Set-Content FinanceCanvasAPI/appsettings.json"
RUN dotnet restore "FinanceCanvasAPI/FinanceCanvasAPI.csproj"
COPY . .
WORKDIR "/src/FinanceCanvasAPI"
RUN dotnet build "./FinanceCanvasAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./FinanceCanvasAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FinanceCanvasAPI.dll"]
