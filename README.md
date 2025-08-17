# DevSkill Inventory Management System

## Overview

DevSkill Inventory is a comprehensive inventory management system built with ASP.NET Core. It provides a robust solution for businesses to manage their inventory, sales, customers, and financial transactions. The application follows a clean architecture pattern with domain-driven design principles.

## Features

### Product Management
- Add, update, and delete products
- Categorize products
- Track stock levels with low stock alerts
- Manage product pricing (MRP, wholesale price)
- Upload and resize product images (AWS S3 integration)

### Sales Management
- Create and manage sales transactions
- Support for different sales types (MRP, wholesale)
- Track payment status
- Generate sales reports

### Customer Management
- Maintain customer database
- Track customer balances
- View customer purchase history

### Financial Management
- Multiple account types (Cash, Bank, Mobile)
- Balance transfers between accounts
- Track financial transactions

### User Management
- Hybrid authentication system combining role-based and claim-based access control
- Fine-grained permissions system with claims assigned to roles
- Comprehensive access setup management
- Employee management with user-employee relationships

### Additional Features
- Department management
- Unit management for products
- Category management

## Technical Architecture

### Project Structure

The solution follows a clean architecture pattern with the following projects:

- **DevSkill.Inventory.Domain**: Contains business entities, interfaces, business rules, and DTOs
- **DevSkill.Inventory.Application**: Contains application services, commands, and queries
- **DevSkill.Inventory.Infrastructure**: Contains implementations of repositories, database context, and external services
- **DevSkill.Inventory.Web**: The ASP.NET Core MVC web application
- **DevSkill.Inventory.Worker**: Background worker for processing tasks like image resizing

### Technologies Used

- **ASP.NET Core**: Web framework
- **Entity Framework Core**: ORM for database access
- **MediatR**: For implementing CQRS pattern
- **AutoMapper**: For object-to-object mapping
- **Autofac**: Dependency injection container
- **Serilog**: Logging framework
- **AWS Services**:
  - S3 for image storage
  - SQS for message queuing
- **SQL Server**: Database with stored procedures for optimized data retrieval
- **Identity Framework**: User authentication with extended role and claim management

## Getting Started

### Prerequisites

- .NET 9.0 SDK or later
- SQL Server
- AWS Account (for S3 and SQS features)
- Visual Studio 2022 or any compatible IDE

### Configuration

1. Clone the repository
2. Update the connection string in `appsettings.json` to point to your SQL Server instance
3. Configure AWS credentials in `appsettings.json`:
   ```json
   "AWS": {
     "Profile": "your-profile",
     "Region": "your-region",
     "BucketName": "your-bucket-name",
     "SqsUrl": "your-sqs-url",
     "UrlExpiryMinutes": 15
   }
   ```
4. Run database migrations:
   ```
   dotnet ef database update --project DevSkill.Inventory.Web
   ```

### Running the Application

1. Set `DevSkill.Inventory.Web` as the startup project
2. Run the application
3. Default admin credentials:
   - Username: admin@example.com
   - Password: Admin@123

## Architecture Details

### Domain-Driven Design

The application follows DDD principles with a rich domain model. Key entities include:

- Product
- Customer
- Sale
- SaleItem
- Category
- Unit
- Employee
- Various account types (Cash, Bank, Mobile)

### CQRS Pattern

The application uses the Command Query Responsibility Segregation pattern:

- Commands: For operations that change state (Add, Update, Delete)
- Queries: For operations that read state

### Repository Pattern

Data access is abstracted through repositories, with a Unit of Work pattern for transaction management. The system leverages stored procedures for complex data retrieval operations, including:

- GetProducts: Advanced product search with filtering and pagination
- GetCustomers: Customer data retrieval with filtering options
- GetSales: Sales transaction reporting
- GetEmployees: Employee data management
- GetUsers: User account management
- GetBalanceTransfers: Financial transaction tracking

### Background Processing

The application includes a background worker for processing image resizing tasks using AWS SQS for message queuing.
