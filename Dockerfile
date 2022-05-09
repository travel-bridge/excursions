### restore & build
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /sln

COPY ./*.sln ./src/*/*.csproj ./
RUN mkdir src
RUN for file in $(ls *.csproj); do mkdir -p src/${file%.*}/ && mv $file src/${file%.*}/; done
RUN dotnet restore "Excursions.sln"
COPY . .
RUN dotnet build "Excursions.sln" -c Release --no-restore
RUN dotnet publish "./src/Excursions.Api/Excursions.Api.csproj" -c Release --output /dist/api --no-restore
RUN dotnet publish "./src/Excursions.Migrator/Excursions.Migrator.csproj" -c Release --output /dist/migrator --no-restore

### tests image
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS tests
WORKDIR /tests
COPY --from=build /sln .
ENTRYPOINT ["dotnet", "test", "-c", "Release", "--no-build"]

### api image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS api
WORKDIR /app
COPY --from=build /dist/api ./
ARG revision=Unknown
ENTRYPOINT [ "dotnet", "Excursions.Api.dll" ]

### migrator image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS migrator
WORKDIR /app
COPY --from=build /dist/migrator ./
ARG revision=Unknown