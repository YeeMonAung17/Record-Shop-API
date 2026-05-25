# Record Shop API

A RESTful ASP.NET Core Web API for managing a record shop's album inventory.

## Overview

Record Shop API provides CRUD endpoints for albums in a music record shop. The project uses Entity Framework Core with SQL Server for persistence and separates responsibilities across controller, service, and repository layers.

## Features

- Create, read, update, and delete albums
- SQL Server persistence with Entity Framework Core
- Repository and service layer architecture
- Swagger/OpenAPI support for exploring endpoints
- Database health check endpoint
- Static image serving from `wwwroot`
- NUnit and Moq tests for controller, service, and repository behavior

## Tech Stack

- C#
- ASP.NET Core 8
- Entity Framework Core
- SQL Server
- Swagger / Swashbuckle
- NUnit
- Moq

## API Endpoints

| Method | Endpoint | Description |
| --- | --- | --- |
| GET | `/api/album` | Get all albums |
| GET | `/api/album/{id}` | Get an album by ID |
| POST | `/api/album` | Create a new album |
| PUT | `/api/album/{id}` | Update an existing album |
| DELETE | `/api/album/{id}` | Delete an album |
| GET | `/api/health` | Check API and database health |

## Album Model

```json
{
  "id": 1,
  "title": "Rumours",
  "artist": "Fleetwood Mac",
  "genre": "Rock",
  "year": 1977,
  "price": 12,
  "stock": 8
}
```

## Local Setup

Clone the repository and restore dependencies:

```powershell
git clone https://github.com/YeeMonAung17/Record-Shop-API.git
cd Record-Shop-API
dotnet restore
```

Set your local SQL Server connection string with .NET User Secrets:

```powershell
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "YOUR_CONNECTION_STRING"
```

Run the API:

```powershell
dotnet run
```

The API runs locally at:

```text
http://localhost:5000
```

Swagger is available at:

```text
http://localhost:5000/swagger
```

## Running Tests

```powershell
dotnet test
```

## Architecture

The project uses a layered structure:

- `Controllers` handle HTTP requests and responses
- `Services` contain application logic
- `Repositories` handle database access
- `Data` contains the Entity Framework `DbContext`
- `Models` contains the domain model
- `Record_Shop.Tests` contains automated tests

## Planned Improvements

- Add JWT authentication and authorization
- Add search and filtering by title, artist, and genre
- Add stronger validation for album input
- Add integration tests
