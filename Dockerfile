# NuGet restore
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY *.sln .
COPY GameMarketAPI/*.csproj GameMarketAPI/
COPY Common/*.csproj Common/
COPY VendorNotifier/*.csproj VendorNotifier/

RUN dotnet restore
COPY . .

# publish
FROM build AS publish
WORKDIR /src/GameMarketAPI
RUN dotnet publish -c Release -o /src/publish

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app
COPY --from=publish /src/publish .
# heroku uses the following
ENTRYPOINT ["dotnet", "GameMarketAPI.dll", "--urls", "http://*:5000;http://*:5001"]