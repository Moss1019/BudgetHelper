# BudgetHelper Backend

A .NET 10 RESTful API for managing receipts, budgets, and automated receipt extraction using AI.

## Overview

BudgetHelper-web provides a RESTful interface for managing expense tracking data. The API supports creating, retrieving, updating, and deleting receipts and their associated line items. A unique feature is the receipt detection endpoint that accepts image data and returns structured receipt information extracted using an AI vision model.

## Project Structure

This solution uses a layered architecture with clear separation of concerns:

| Project | Description |
|---------|-------------|
| `BudgetHelper-web` | Main web API with REST endpoints |
| `BudgetApp-lib` | Shared models, repository interfaces, and services |
| `BudgetHelper-ef` | Entity Framework Core context and migrations |
| `Elastic-lib` | Elasticsearch client abstraction |
| `Messaging-lib` | Apache Pulsar producer/consumer abstraction |
| `Logging-lib` | Structured logging infrastructure |
| `Logging-sink` | Pulsar consumer that indexes logs to Elasticsearch |

### Layer Details

- **Resource Layer** - REST endpoints using ASP.NET Core minimal APIs
- **Service Layer** - Business logic, validation, and coordinate between components
- **Repository Layer** - Data persistence operations via Entity Framework Core
- **Client Layer** - Integration with external services (Ollama AI, Pulsar, Elasticsearch)

## Tech Stack

- **.NET 10** - Latest LTS version with modern C# features
- **Entity Framework Core** - Database ORM with PostgreSQL provider
- **PostgreSQL** - Relational database for data persistence
- **Elasticsearch** - Search and log storage
- **Apache Pulsar** - Messaging system for decoupled services
- **NUnit** - Unit testing framework

## API Endpoints

The API exposes the following endpoints:

- `GET /api/receipts` - List all receipts
- `POST /api/receipts` - Create a new receipt
- `PUT /api/receipts/{id}` - Update an existing receipt
- `DELETE /api/receipts/{id}` - Delete a receipt
- `GET /api/receipt-lines` - List all receipt lines
- `POST /api/receipt-lines` - Create a new receipt line
- `PUT /api/receipt-lines/{id}` - Update a receipt line
- `DELETE /api/receipt-lines/{id}` - Delete a receipt line
- `GET /api/budgets` - Get the current budget
- `PUT /api/budgets/{id}` - Update the monthly budget
- `POST /api/receipt-extraction` - Submit a receipt image for AI extraction

The API also exposes OpenAPI/Swagger documentation when running in Development mode.

## Development

### Prerequisites

- .NET 10 SDK
- PostgreSQL server
- Elasticsearch server (optional, for logging)
- Apache Pulsar server (optional, for messaging)
- Ollama with the `qwen3-vl:8b` model installed

### Configuration

The application reads configuration from standard ASP.NET Core sources (appsettings.json, environment variables, etc.). Required connection strings:

```json
{
  "ConnectionStrings": {
    "Postgres": "your-postgresql-connection-string"
  }
}
```

### Running the Application

```bash
# Run with HTTP
dotnet run --project BudgetHelper-web/BudgetHelper-web.csproj

# Run with HTTPS
dotnet run --project BudgetHelper-web/BudgetHelper-web.csproj --launch-profile https
```

The API will be available at `http://localhost:3000` or `https://localhost:7009`.

### Running Tests

```bash
dotnet test BudgetHelper-webTest/BudgetHelper-webTest.csproj
```

## AI Integration

The receipt extraction feature uses **Ollama** with the **qwen3-vl:8b** vision language model. Images are sent as base64-encoded data, and the model returns structured data including:

- Receipt date
- Total amount
- List of purchase items with individual prices

The AI endpoint is available at `POST /api/receipt-extraction` and accepts JSON payloads with the image data.