# NuGet restore
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY *.sln .
COPY GameMarketAPI/*.csproj GameMarketAPI/

RUN dotnet restore
COPY . .

# publish
FROM build AS publish
WORKDIR /src/GameMarketAPI
RUN dotnet publish -c Release -o /src/publish
RUN dotnet ef database update

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app
COPY --from=publish /src/publish .
# heroku uses the following
CMD ASPNETCORE_URLS=http://*:$PORT dotnet GameMarketAPI.dll
