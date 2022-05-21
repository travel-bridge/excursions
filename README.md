# Excursions

## Run postgres docker
    docker run -d --name postgres -e POSTGRES_PASSWORD=postgres -e POSTGRES_USER=postgres -p 5432:5432 postgres

## Create ``excursions-database`` database

## Run Excursions.Migrator with connection string
    User ID=postgres;Password=postgres;Host=localhost;Port=5432;Database=excursions-database;