# Business Card Management API

A robust .NET Core Web API for managing business card information with import/export capabilities, built following clean architecture principles.

## Description

The Business Card Management API is a RESTful backend service that provides comprehensive CRUD operations for business card data. It features file import/export in CSV and XML formats, photo handling with Base64 encoding, and advanced filtering capabilities.

## Features

- **CRUD Operations**: Complete Create, Read, Update, Delete functionality
- **File Import**: CSV and XML file processing with validation
- **File Export**: Generate CSV and XML files from database records
- **Photo Management**: Base64 encoding with 1MB size validation
- **Advanced Filtering**: Filter by name, gender, date of birth, email, and phone
- **Database Integration**: Entity Framework Core with SQL Server
- **Unit Testing**: Comprehensive test coverage with xUnit
- **API Documentation**: Swagger/OpenAPI integration

## Tech Stack

- **.NET Core**: 6.0
- **Language**: C#
- **ORM**: Entity Framework Core 9.0.10
- **Database**: SQL Server 2019+
- **Testing**: xUnit, FakeItEasy, FluentAssertions
- **File Processing**: CsvHelper 33.1.0

## Prerequisites

- **Operating System**: Windows 10/11, macOS, or Linux
- **.NET SDK**: Version 6.0 or higher ([Download](https://dotnet.microsoft.com/download))
- **Database**: SQL Server 2019+ or SQL Server Express
- **IDE** (Optional): Visual Studio 2022 or Visual Studio Code

## Installation

### 1. Clone the Repository

```bash
git clone <repository-url>
cd BusinessCardManagement/BusinessCardAPI
```

### 2. Restore NuGet Packages

```bash
dotnet restore
```

### 3. Configure Database Connection

Edit `appsettings.json` and update the connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=BusinessCardDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

**SQL Server:**
```json
"Server=localhost\\SQLEXPRESS;Database=BusinessCardDB;Trusted_Connection=True;TrustServerCertificate=True;"
```

### 4. Build the Project

```bash
dotnet build
```

### 5. Run Database Migrations

```bash
dotnet ef database update
```

Or simply run the application (migrations will run automatically):

```bash
dotnet run
```

## Running the Application

### Start the API Server

```bash
dotnet run
```

Expected output:
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:7001
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
```

## API Endpoints

### Business Card Operations

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/BusinessCard` | Retrieve all business cards |
| GET | `/api/BusinessCard/{id}` | Get a specific card by ID |
| POST | `/api/BusinessCard` | Create a new business card |
| DELETE | `/api/BusinessCard/{id}` | Delete a business card |
| GET | `/api/BusinessCard/filter` | Filter cards with query parameters |

### Import/Export Operations

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/BusinessCard/import/csv` | Import cards from CSV file |
| POST | `/api/BusinessCard/import/xml` | Import cards from XML file |
| GET | `/api/BusinessCard/export/csv` | Export all cards to CSV |
| GET | `/api/BusinessCard/export/xml` | Export all cards to XML |

## API Usage Examples

### Create a Business Card

```bash
curl -X POST https://localhost:7001/api/BusinessCard \
  -H "Content-Type: application/json" \
  -d '{
    "name": "John Doe",
    "gender": "Male",
    "dateOfBirth": "1990-01-15",
    "email": "john.doe@example.com",
    "phone": "+1234567890",
    "address": "123 Main Street"
  }'
```

### Filter Business Cards

```bash
curl "https://localhost:7001/api/BusinessCard/filter?name=John&gender=Male"
```

### Import CSV File

```bash
curl -X POST https://localhost:7001/api/BusinessCard/import/csv \
  -F "file=@test-cards.csv"
```

### Export to XML

```bash
curl https://localhost:7001/api/BusinessCard/export/xml -o cards.xml
```

## Sample Data Formats

### CSV Format (`test-cards.csv`)

```csv
Name,Gender,DateOfBirth,Email,Phone,Address
John Doe,Male,1990-01-15,john.doe@example.com,+1234567890,123 Main Street
Jane Smith,Female,1985-05-20,jane.smith@example.com,+0987654321,456 Oak Avenue
```

### XML Format (`test-cards.xml`)

```xml
<?xml version="1.0" encoding="utf-8"?>
<BusinessCards>
  <BusinessCard>
    <Name>John Doe</Name>
    <Gender>Male</Gender>
    <DateOfBirth>1990-01-15</DateOfBirth>
    <Email>john.doe@example.com</Email>
    <Phone>+1234567890</Phone>
    <Address>123 Main Street</Address>
    <Photo></Photo>
  </BusinessCard>
</BusinessCards>
```

## Testing

### Run All Tests

```bash
cd ../BusinessCardAPI.Tests
dotnet test
```

### Run Tests with Detailed Output

```bash
dotnet test --logger "console;verbosity=detailed"
```

### Generate Test Coverage Report

```bash
dotnet test /p:CollectCoverage=true
```

## Configuration

### CORS Settings

By default, CORS is configured to allow requests from `http://localhost:4200`. To modify this, edit `Program.cs`:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy => policy.WithOrigins("http://localhost:4200")
                       .AllowAnyMethod()
                       .AllowAnyHeader());
});
```


## Troubleshooting

### Database Connection Issues

**Problem**: Cannot connect to SQL Server

**Solution**:
```bash
# Verify SQL Server is running
# Update connection string in appsettings.json
# Try running migrations again
dotnet ef database update
```

### Migration Issues

**Problem**: Migrations fail or database schema is incorrect

**Solution**:
```bash
# Drop database and recreate
dotnet ef database drop
dotnet ef database update
```

### Port Already in Use

**Problem**: Port 7001 is already in use

**Solution**: Edit `launchSettings.json` to change the port:
```json
"applicationUrl": "https://localhost:7002;http://localhost:5002"
```

### Package Restore Errors

**Problem**: NuGet packages fail to restore

**Solution**:
```bash
# Clear NuGet cache
dotnet nuget locals all --clear

# Restore packages
dotnet restore
```

## Architecture & Design Patterns

- **Clean Architecture**: Separation of concerns with clear boundaries
- **Repository Pattern**: Abstraction of data access logic
- **Service Layer Pattern**: Business logic isolation
- **Dependency Injection**: Loose coupling and testability
- **DTO Pattern**: Data transfer objects for API contracts
- **SOLID Principles**: Maintainable and extensible code

## License

This project is licensed under the MIT License - see the LICENSE.md file for details.

