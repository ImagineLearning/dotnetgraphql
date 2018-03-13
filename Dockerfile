FROM microsoft/aspnetcore-build:1.1 AS build-env
WORKDIR /app
COPY StarWars.Core/*.csproj ./StarWars.Core/
COPY StarWars.Data/*.csproj ./StarWars.Data/
COPY StarWars.Api/*.csproj ./StarWars.Api/

RUN dotnet restore ./StarWars.Api/*.csproj

COPY . ./
RUN dotnet publish -c Release ./StarWars.Api/*.csproj

FROM microsoft/aspnetcore:1.1
EXPOSE 5000
WORKDIR /app
COPY --from=build-env /app/StarWars.Api/bin/Release/netcoreapp1.1/publish/ .
ENTRYPOINT ["dotnet", "StarWars.Api.dll"]
