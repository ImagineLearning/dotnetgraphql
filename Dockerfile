FROM microsoft/aspnetcore-build:2.0-jessie AS build-env
WORKDIR /app
COPY StarWars.Core/*.csproj ./StarWars.Core/
COPY StarWars.Data/*.csproj ./StarWars.Data/
COPY StarWars.Api/*.csproj ./StarWars.Api/

RUN dotnet restore ./StarWars.Api/*.csproj

COPY . ./
RUN dotnet publish -c Release ./StarWars.Api/*.csproj

FROM microsoft/aspnetcore:2.0-jessie
EXPOSE 5000
WORKDIR /app
COPY --from=build-env /app/StarWars.Api/bin/Release/netcoreapp2.0/publish/ .
ENTRYPOINT ["dotnet", "StarWars.Api.dll"]
