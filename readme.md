# BudgetHelper

A comprehensive receipt management system that helps you track expenses by digitizing physical and digital receipts using AI-powered image recognition.

## Overview

BudgetHelper consists of two main components:

- **Backend** (.NET 10) - A RESTful API that manages receipts, receipt lines, and budgets with data persistence in PostgreSQL
- **Frontend** (iOS + SwiftUI) - A native iOS application that allows you to browse receipts and capture new ones via camera, with automatic extraction of purchase details using AI

### Key Features

- 📸 **AI-Powered Receipt Extraction** - Snap a photo of any receipt and let the system automatically extract items, quantities, and prices using a vision model
- 💰 **Budget Tracking** - Set monthly spending limits and track your progress
- 📋 **Receipt Management** - View, create, update, and delete receipts with detailed line items
- 📊 **Electronic Receipt Storage** - Store digital copies of receipts and their extracted data

## Backend

### Tech Stack

- **.NET 10** - Modern, performant server-side framework
- **PostgreSQL** - Relational database for persistent storage
- **Entity Framework Core** - ORM for database operations
- **Elasticsearch** - Search and logging infrastructure
- **Apache Pulsar** - Messaging system for event-driven architecture
- **Ollama + qwen3-vl:8b** - Vision language model for receipt image extraction

### Architecture

The backend follows a layered architecture:

- **Resource Layer** - REST API endpoints (OpenAPI/Swagger enabled in development)
- **Service Layer** - Business logic and calculations
- **Repository Layer** - Data access abstractions for PostgreSQL
- **Client Layer** - Integration with external services (Ollama, Elasticsearch, Pulsar)

### Development

The API exposes the following endpoints:

- Receipts: Create, read, update, delete
- Receipt Lines: Manage individual items within receipts
- Budgets: View and modify monthly spending limits
- Receipt Detection: Submit images for automatic data extraction

## Frontend

### Tech Stack

- **iOS** - Native Apple mobile platform
- **SwiftUI** - Declarative UI framework
- **Swift** - Modern programming language

### Architecture

The frontend is organized into shared and platform-specific code:

- **BudgetAppV3/** - iOS-specific views and controllers
- **BudgetAppV3SharedV2/** - Shared models, services, and clients

### Key Components

- **ReceiptDetectorView** - Camera interface for capturing receipts with automatic extraction
- **BudgetView** - Monthly budget tracking and visualization
- **ReceiptView** - Browse and manage saved receipts and line items
- **Shared Services** - Business logic encapsulated in service classes
- **API Client** - HTTP client with timeout handling and error management

### Features

- Real-time camera integration for receipt capture
- Automatic text extraction from receipt images using the backend AI service
- Intuitive UI for managing expense line items
- Visual budget tracking cards