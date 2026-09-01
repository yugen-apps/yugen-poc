# Entity Framework

## Add Migration

Go To `Poc.Identity.Blazor` and run

> dotnet ef migrations add InitialCreate --project ..\Poc.Identity.Persistence\

## Docker SQL

/opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "Password@12345#" -No

CREATE DATABASE [PocIdentity]
GO