# BudgetHelper iOS App

A native iOS application built with SwiftUI for managing personal and household expenses through receipt tracking.

## Overview

BudgetHelper for iOS provides an intuitive interface for capturing, managing, and analyzing your receipts. The app features seamless integration with the backend API and includes an AI-powered receipt scanner that extracts purchase details from photos.

## Features

- 📸 **AI-Powered Receipt Scanner** - Capture photos of receipts and automatically extract line items using the backend's vision AI
- 💰 **Budget Tracking** - Monitor your monthly spending against custom limits
- 📋 **Receipt Management** - Browse, create, edit, and delete receipts with detailed line items
- 📊 **Visual Data Display** - At-a-glance cards showing budget status and receipt information
- 🌐 **API Integration** - Reliable HTTP client with timeout handling and comprehensive error management

## Architecture

The app is organized into logical layers:

### Shared Code (`BudgetAppV3SharedV2/`)

- **Models** - Data structures for Receipt, ReceiptLine, Budget, and extraction requests
- **Services** - Business logic encapsulation (ReceiptService, ReceiptLineService, BudgetService, ReceiptDetectorService)
- **Clients** - HTTP clients for API communication with error handling
- **Result Types** - Unified success/failure result wrapper

### iOS App (`BudgetAppV3/`)

- **Views** - SwiftUI views for the UI (ReceiptDetectorView, Budget views, Receipt views)
- **Controllers** - State management and coordination between views and services
- **Components** - Reusable UI components (cards, editors)

## Tech Stack

- **Swift 5.9+** - Modern Swift features
- **SwiftUI** - Declarative UI framework
- **Swift Concurrency** - Async/await for network operations
- **URLSession** - HTTP client with custom configuration

## Project Structure

```
BudgetAppV3/
├── BudgetAppV3/                    # iOS app target
│   ├── BudgetAppV3App.swift        # App entry point
│   ├── View/                       # SwiftUI views
│   ├── Controller/                 # State management controllers
│   └── ...                        
├── BudgetAppV3SharedV2/            # Shared business logic
│   ├── Model/                      # Data models
│   ├── Service/                    # Business services
│   ├── Client/                     # API clients
│   └── Result/                     # Result types
├── BudgetAppV3Tests/               # Unit tests
└── BudgetAppV3UITests/             # UI tests
```

## API Integration

The app connects to the BudgetHelper backend API. Configuration is in `ApiClient.swift`:

```swift
private let baseUrl = "http://192.168.50.149:3000"
```

Update this URL to point to your backend deployment.

### Endpoints Used

- `POST /api/receipt-extraction` - AI receipt extraction
- `GET /api/receipts` - List receipts
- `POST /api/receipts` - Create receipt
- `GET /api/budgets` - Get budget
- `PUT /api/budgets/{id}` - Update budget

## Development

### Prerequisites

- Xcode 15+ with iOS 17+ SDK
- macOS with Xcode Command Line Tools

### Running

1. Open `BudgetAppV3.xcodeproj` in Xcode
2. Select a destination simulator or connected device
3. Build and run (Cmd+R)

### Testing

Run tests using Xcode's test navigator or the `cmd+U` shortcut.

## Future Enhancements

- iCloud sync for backup and cross-device access
- Export receipts to PDF or CSV
- Monthly expense reports and analytics
- Receipt photo organization and filtering
- Voice input for quick entry

