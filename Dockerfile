### restore & build
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /sln

COPY ./*.sln ./src/*/*.csproj ./
RUN mkdir src
RUN for file in $(ls *.csproj); do mkdir -p src/${file%.*}/ && mv $file src/${file%.*}/; done
RUN dotnet restore "Excursions.sln"
COPY . .
RUN dotnet build "Excursions.sln" -c Release --no-restore
RUN dotnet publish "./src/Excursions.Services/Excursions.Services.csproj" -c Release --output /dist/services --no-restore
RUN dotnet publish "./src/Excursions.Worker/Excursions.Worker.csproj" -c Release --output /dist/worker --no-restore
RUN dotnet publish "./src/Excursions.Migrator/Excursions.Migrator.csproj" -c Release --output /dist/migrator --no-restore

### tests image
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS tests
WORKDIR /tests
COPY --from=build /sln .
ENTRYPOINT ["dotnet", "test", "-c", "Release", "--no-build"]

### services image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS services
WORKDIR /app
COPY --from=build /dist/services ./
ARG revision=Unknown
ENTRYPOINT [ "dotnet", "Excursions.Services.dll" ]

### worker image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS worker
WORKDIR /app
COPY --from=build /dist/worker ./
ARG revision=Unknown
ENTRYPOINT [ "dotnet", "Excursions.Worker.dll" ]

### migrator image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS migrator
WORKDIR /app
COPY --from=build /dist/migrator ./
ARG revision=Unknown
ENTRYPOINT [ "dotnet", "Excursions.Migrator.dll" ]