# SmartDeliveryApp - Technical Architecture

## 📋 Table of Contents
- [System Overview](#system-overview)
- [Detailed Architecture](#detailed-architecture)
- [Data Flow Diagrams](#data-flow-diagrams)
- [Service Communication](#service-communication)
- [Database Architecture](#database-architecture)
- [Security Architecture](#security-architecture)
- [Deployment Architecture](#deployment-architecture)

## 🏗️ System Overview

SmartDeliveryApp is a microservices-based food delivery platform that follows Domain-Driven Design (DDD) and Clean Architecture principles. The system is designed for scalability, maintainability, and high availability.

### Core Principles
- **Separation of Concerns**: Each service has a single responsibility
- **Loose Coupling**: Services communicate through well-defined APIs
- **High Cohesion**: Related functionality is grouped together
- **Eventual Consistency**: Services can operate independently
- **Fault Tolerance**: Services are resilient to failures

## 🏛️ Detailed Architecture

### Service Layer Architecture

```mermaid
graph TB
    subgraph "Presentation Layer"
        API[API Controllers]
        Middleware[Middleware Stack]
    end
    
    subgraph "Application Layer"
        CQRS[CQRS Pattern]
        MediatR[MediatR Pipeline]
        Validators[FluentValidation]
    end
    
    subgraph "Domain Layer"
        Entities[Domain Entities]
        ValueObjects[Value Objects]
        DomainServices[Domain Services]
        Interfaces[Domain Interfaces]
    end
    
    subgraph "Infrastructure Layer"
        Repositories[Repository Implementation]
        DbContext[Entity Framework]
        ExternalAPIs[External APIs]
        MessageBus[Message Bus]
    end
    
    subgraph "Cross-Cutting Concerns"
        Logging[Serilog + Elasticsearch]
        Auth[JWT Authentication]
        Correlation[Correlation IDs]
        Resilience[Polly Policies]
    end
    
    API --> Middleware
    Middleware --> CQRS
    CQRS --> MediatR
    MediatR --> Validators
    Validators --> Entities
    Entities --> DomainServices
    DomainServices --> Interfaces
    Interfaces --> Repositories
    Repositories --> DbContext
    Repositories --> ExternalAPIs
    Repositories --> MessageBus
    
    Logging -.-> API
    Auth -.-> API
    Correlation -.-> Middleware
    Resilience -.-> ExternalAPIs
```

### Order Service Internal Structure

```mermaid
graph TB
    subgraph "OrderService.API"
        OrderController[OrderController]
        Program[Program.cs]
        Startup[Startup Configuration]
    end
    
    subgraph "OrderService.Application"
        Commands[Commands]
        Queries[Queries]
        Handlers[Command/Query Handlers]
        Validators[Validators]
        DTOs[DTOs]
    end
    
    subgraph "OrderService.Domain"
        Order[Order Entity]
        OrderItem[OrderItem Entity]
        OrderStatus[OrderStatus Enum]
        OrderStatusHistory[OrderStatusHistory Entity]
        IOrderRepository[IOrderRepository]
        IOrderUnitOfWork[IOrderUnitOfWork]
    end
    
    subgraph "OrderService.Infra"
        OrderRepository[OrderRepository]
        OrderDbContext[OrderDbContext]
        UnitOfWork[UnitOfWork]
        Migrations[EF Migrations]
    end
    
    OrderController --> Commands
    OrderController --> Queries
    Commands --> Handlers
    Queries --> Handlers
    Handlers --> Validators
    Handlers --> DTOs
    Handlers --> Order
    Handlers --> IOrderRepository
    IOrderRepository --> OrderRepository
    OrderRepository --> OrderDbContext
    OrderRepository --> UnitOfWork
    OrderDbContext --> Migrations
```

### Restaurant Service Internal Structure

```mermaid
graph TB
    subgraph "RestaurantService.API"
        RestaurantController[RestaurantController]
        Program[Program.cs]
        Startup[Startup Configuration]
    end
    
    subgraph "RestaurantService.Application"
        Commands[Commands]
        Queries[Queries]
        Handlers[Command/Query Handlers]
        Validators[Validators]
        DTOs[DTOs]
        Mappers[AutoMapper Profiles]
    end
    
    subgraph "RestaurantService.Domain"
        Restaurant[Restaurant Entity]
        MenuItem[MenuItem Entity]
        Category[Category Entity]
        IRestaurantRepository[IRestaurantRepository]
        IRestaurantUnitOfWork[IRestaurantUnitOfWork]
    end
    
    subgraph "RestaurantService.Infra"
        RestaurantRepository[RestaurantRepository]
        RestaurantDbContext[RestaurantDbContext]
        UnitOfWork[UnitOfWork]
        Migrations[EF Migrations]
        SeedData[Seed Data]
    end
    
    RestaurantController --> Commands
    RestaurantController --> Queries
    Commands --> Handlers
    Queries --> Handlers
    Handlers --> Validators
    Handlers --> DTOs
    Handlers --> Mappers
    Handlers --> Restaurant
    Handlers --> IRestaurantRepository
    IRestaurantRepository --> RestaurantRepository
    RestaurantRepository --> RestaurantDbContext
    RestaurantRepository --> UnitOfWork
    RestaurantDbContext --> Migrations
    RestaurantDbContext --> SeedData
```

### Payment Service Internal Structure

```mermaid
graph TB
    subgraph "PaymentService.API"
        PaymentController[PaymentController]
        Program[Program.cs]
        Startup[Startup Configuration]
    end
    
    subgraph "PaymentService.Application"
        Commands[Commands]
        DTOs[DTOs]
        Handlers[Command Handlers]
        Contracts[Payment Contracts]
    end
    
    subgraph "PaymentService.Domain"
        Payment[Payment Entity]
        IPaymentService[IPaymentService]
    end
    
    subgraph "PaymentService.Infra"
        MockPaymentService[MockPaymentService]
        ExternalPaymentProvider[External Payment Provider]
    end
    
    PaymentController --> Commands
    Commands --> Handlers
    Handlers --> DTOs
    Handlers --> Contracts
    Handlers --> Payment
    Handlers --> IPaymentService
    IPaymentService --> MockPaymentService
    IPaymentService --> ExternalPaymentProvider
```

## 🔄 Data Flow Diagrams

### Order Creation Flow

```mermaid
sequenceDiagram
    participant Client
    participant OrderAPI
    participant OrderApp
    participant OrderDomain
    participant OrderInfra
    participant RestaurantAPI
    participant PaymentAPI
    participant Database
    
    Client->>OrderAPI: POST /api/order
    OrderAPI->>OrderApp: CreateOrderCommand
    OrderApp->>OrderApp: Validate Request
    OrderApp->>RestaurantAPI: Verify Restaurant
    RestaurantAPI-->>OrderApp: Restaurant Details
    OrderApp->>OrderDomain: Create Order Entity
    OrderDomain->>OrderInfra: Save Order
    OrderInfra->>Database: Insert Order
    Database-->>OrderInfra: Order Created
    OrderInfra-->>OrderDomain: Order with ID
    OrderDomain-->>OrderApp: Order Created
    OrderApp->>PaymentAPI: Create Payment Intent
    PaymentAPI-->>OrderApp: Payment Intent
    OrderApp-->>OrderAPI: Order Response
    OrderAPI-->>Client: 201 Created
```

### Restaurant Management Flow

```mermaid
sequenceDiagram
    participant Client
    participant RestaurantAPI
    participant RestaurantApp
    participant RestaurantDomain
    participant RestaurantInfra
    participant Database
    
    Client->>RestaurantAPI: POST /api/restaurants
    RestaurantAPI->>RestaurantApp: CreateRestaurantCommand
    RestaurantApp->>RestaurantApp: Validate Request
    RestaurantApp->>RestaurantDomain: Create Restaurant Entity
    RestaurantDomain->>RestaurantInfra: Save Restaurant
    RestaurantInfra->>Database: Insert Restaurant
    Database-->>RestaurantInfra: Restaurant Created
    RestaurantInfra-->>RestaurantDomain: Restaurant with ID
    RestaurantDomain-->>RestaurantApp: Restaurant Created
    RestaurantApp-->>RestaurantAPI: Restaurant Response
    RestaurantAPI-->>Client: 201 Created
```

### Payment Processing Flow

```mermaid
sequenceDiagram
    participant Client
    participant PaymentAPI
    participant PaymentApp
    participant PaymentDomain
    participant PaymentProvider
    participant Database
    
    Client->>PaymentAPI: POST /api/payments/intents
    PaymentAPI->>PaymentApp: CreatePaymentIntentCommand
    PaymentApp->>PaymentApp: Validate Request
    PaymentApp->>PaymentDomain: Create Payment Intent
    PaymentDomain->>PaymentProvider: Create Intent
    PaymentProvider-->>PaymentDomain: Payment Intent
    PaymentDomain->>Database: Save Payment Record
    Database-->>PaymentDomain: Payment Saved
    PaymentDomain-->>PaymentApp: Payment Intent Response
    PaymentApp-->>PaymentAPI: Payment Intent
    PaymentAPI-->>Client: 200 OK
```

## 🌐 Service Communication

### Inter-Service Communication Patterns

```mermaid
graph TB
    subgraph "Synchronous Communication"
        HTTP[HTTP/REST APIs]
        gRPC[gRPC (Future)]
    end
    
    subgraph "Asynchronous Communication"
        MessageQueue[Message Queue (Future)]
        EventBus[Event Bus (Future)]
    end
    
    subgraph "Service Discovery"
        ServiceRegistry[Service Registry]
        LoadBalancer[Load Balancer]
    end
    
    subgraph "Resilience Patterns"
        CircuitBreaker[Circuit Breaker]
        RetryPolicy[Retry Policy]
        Timeout[Timeout Policy]
    end
    
    HTTP --> CircuitBreaker
    gRPC --> CircuitBreaker
    CircuitBreaker --> RetryPolicy
    RetryPolicy --> Timeout
    Timeout --> ServiceRegistry
    ServiceRegistry --> LoadBalancer
```

### Current Service Dependencies

```mermaid
graph TB
    subgraph "Order Service"
        OrderAPI[Order API]
        OrderApp[Order Application]
        OrderDomain[Order Domain]
        OrderInfra[Order Infrastructure]
    end
    
    subgraph "Restaurant Service"
        RestaurantAPI[Restaurant API]
        RestaurantApp[Restaurant Application]
        RestaurantDomain[Restaurant Domain]
        RestaurantInfra[Restaurant Infrastructure]
    end
    
    subgraph "Payment Service"
        PaymentAPI[Payment API]
        PaymentApp[Payment Application]
        PaymentDomain[Payment Domain]
        PaymentInfra[Payment Infrastructure]
    end
    
    subgraph "Shared Services"
        SharedSvc[Shared Services]
        SharedData[Shared Data]
    end
    
    OrderApp --> RestaurantAPI
    OrderApp --> PaymentAPI
    OrderInfra --> SharedData
    RestaurantInfra --> SharedData
    PaymentInfra --> SharedData
    OrderAPI --> SharedSvc
    RestaurantAPI --> SharedSvc
    PaymentAPI --> SharedSvc
```

## 🗄️ Database Architecture

### Database Per Service Pattern

```mermaid
graph TB
    subgraph "Order Service Database"
        OrderDB[(Order DB)]
        OrderTables[Order Tables]
        OrderIndexes[Order Indexes]
    end
    
    subgraph "Restaurant Service Database"
        RestaurantDB[(Restaurant DB)]
        RestaurantTables[Restaurant Tables]
        RestaurantIndexes[Restaurant Indexes]
    end
    
    subgraph "Payment Service Database"
        PaymentDB[(Payment DB)]
        PaymentTables[Payment Tables]
        PaymentIndexes[Payment Indexes]
    end
    
    OrderDB --> OrderTables
    OrderTables --> OrderIndexes
    RestaurantDB --> RestaurantTables
    RestaurantTables --> RestaurantIndexes
    PaymentDB --> PaymentTables
    PaymentTables --> PaymentIndexes
```

### Database Schema Relationships

```mermaid
erDiagram
    %% Order Service Database
    Orders {
        Guid OrderId PK
        Guid UserId
        Guid RestaurantId
        OrderStatus Status
        decimal TotalAmount
        DateTime CreatedAt
        DateTime UpdatedAt
        bool IsCancelled
        DateTime CancelledAt
        string CancellationReason
        bool IsRefunded
        DateTime RefundedAt
        string RefundReason
        string Notes
        DateTime DeliveredAt
    }
    
    OrderItems {
        Guid OrderItemId PK
        Guid OrderId FK
        Guid ProductId
        int Quantity
        decimal Price
        string ProductName
    }
    
    OrderStatusHistory {
        Guid StatusId PK
        Guid OrderId FK
        OrderStatus Status
        DateTime Timestamp
        string Notes
    }
    
    %% Restaurant Service Database
    Restaurants {
        Guid Id PK
        string Name
        string Description
        bool IsActive
        DateTime CreatedAt
        string CoverImageUrl
        string LogoUrl
        string Address
        decimal DeliveryFee
        decimal MinOrderAmount
        double AverageRating
        int EstimatedDeliveryTime
        string PhoneNumber
    }
    
    Categories {
        Guid Id PK
        Guid RestaurantId FK
        string Name
        int DisplayOrder
    }
    
    MenuItems {
        Guid Id PK
        Guid RestaurantId FK
        Guid CategoryId FK
        string Name
        string Description
        decimal Price
        string ImageUrl
        bool IsVegetarian
        bool IsVegan
        bool IsAvailable
        DateTime CreatedAt
    }
    
    %% Relationships
    Orders ||--o{ OrderItems : contains
    Orders ||--o{ OrderStatusHistory : tracks
    Restaurants ||--o{ Categories : has
    Restaurants ||--o{ MenuItems : offers
    Categories ||--o{ MenuItems : contains
```

## 🔒 Security Architecture

### Authentication and Authorization Flow

```mermaid
sequenceDiagram
    participant Client
    participant API Gateway
    participant Auth Service
    participant Microservice
    participant Database
    
    Client->>API Gateway: Request with JWT Token
    API Gateway->>Auth Service: Validate Token
    Auth Service->>Database: Check Token
    Database-->>Auth Service: Token Valid
    Auth Service-->>API Gateway: Token Valid + Claims
    API Gateway->>Microservice: Forward Request with Claims
    Microservice->>Microservice: Authorize Request
    Microservice-->>API Gateway: Response
    API Gateway-->>Client: Response
```

### Security Layers

```mermaid
graph TB
    subgraph "Transport Security"
        HTTPS[HTTPS/TLS]
        Certificates[SSL Certificates]
    end
    
    subgraph "Authentication"
        JWT[JWT Tokens]
        Bearer[Bearer Authentication]
        Claims[Claims-based Authorization]
    end
    
    subgraph "Authorization"
        Roles[Role-based Access Control]
        Policies[Authorization Policies]
        Permissions[Permission-based Access]
    end
    
    subgraph "Data Protection"
        Encryption[Data Encryption]
        Validation[Input Validation]
        Sanitization[Data Sanitization]
    end
    
    HTTPS --> JWT
    JWT --> Bearer
    Bearer --> Claims
    Claims --> Roles
    Roles --> Policies
    Policies --> Permissions
    Permissions --> Encryption
    Encryption --> Validation
    Validation --> Sanitization
```

## 🚀 Deployment Architecture

### Container Deployment

```mermaid
graph TB
    subgraph "Load Balancer"
        LB[Load Balancer]
    end
    
    subgraph "API Gateway"
        Gateway[API Gateway]
    end
    
    subgraph "Microservices"
        OrderContainer[Order Service Container]
        RestaurantContainer[Restaurant Service Container]
        PaymentContainer[Payment Service Container]
    end
    
    subgraph "Databases"
        OrderDB[(Order Database)]
        RestaurantDB[(Restaurant Database)]
        PaymentDB[(Payment Database)]
    end
    
    subgraph "Monitoring"
        Elasticsearch[Elasticsearch]
        Kibana[Kibana]
        Logs[Application Logs]
    end
    
    LB --> Gateway
    Gateway --> OrderContainer
    Gateway --> RestaurantContainer
    Gateway --> PaymentContainer
    OrderContainer --> OrderDB
    RestaurantContainer --> RestaurantDB
    PaymentContainer --> PaymentDB
    OrderContainer --> Logs
    RestaurantContainer --> Logs
    PaymentContainer --> Logs
    Logs --> Elasticsearch
    Elasticsearch --> Kibana
```

### Development Environment

```mermaid
graph TB
    subgraph "Development Tools"
        VS[Visual Studio]
        VS Code[VS Code]
        Docker[Docker Desktop]
    end
    
    subgraph "Local Services"
        SQL Server[SQL Server LocalDB]
        Elasticsearch[Elasticsearch Container]
        Kibana[Kibana Container]
    end
    
    subgraph "Application Services"
        OrderService[Order Service]
        RestaurantService[Restaurant Service]
        PaymentService[Payment Service]
    end
    
    VS --> OrderService
    VS Code --> RestaurantService
    VS Code --> PaymentService
    OrderService --> SQL Server
    RestaurantService --> SQL Server
    PaymentService --> SQL Server
    OrderService --> Elasticsearch
    RestaurantService --> Elasticsearch
    PaymentService --> Elasticsearch
    Elasticsearch --> Kibana
    Docker --> Elasticsearch
    Docker --> Kibana
```

## 📊 Monitoring and Observability

### Logging Architecture

```mermaid
graph TB
    subgraph "Application Layer"
        Serilog[Serilog Logger]
        CorrelationID[Correlation ID Middleware]
        RequestLogging[Request Logging]
    end
    
    subgraph "Transport Layer"
        HTTP[HTTP Client Logging]
        Database[Database Query Logging]
    end
    
    subgraph "Centralized Logging"
        Elasticsearch[Elasticsearch]
        Logstash[Logstash]
        Kibana[Kibana]
    end
    
    subgraph "Monitoring"
        Metrics[Application Metrics]
        Alerts[Alerting]
        Dashboards[Dashboards]
    end
    
    Serilog --> Elasticsearch
    CorrelationID --> Elasticsearch
    RequestLogging --> Elasticsearch
    HTTP --> Elasticsearch
    Database --> Elasticsearch
    Elasticsearch --> Logstash
    Logstash --> Kibana
    Elasticsearch --> Metrics
    Metrics --> Alerts
    Metrics --> Dashboards
```

### Health Checks and Monitoring

```mermaid
graph TB
    subgraph "Health Checks"
        Liveness[Liveness Probe]
        Readiness[Readiness Probe]
        Startup[Startup Probe]
    end
    
    subgraph "Metrics Collection"
        Prometheus[Prometheus]
        CustomMetrics[Custom Metrics]
        SystemMetrics[System Metrics]
    end
    
    subgraph "Alerting"
        AlertManager[Alert Manager]
        EmailAlerts[Email Alerts]
        SlackAlerts[Slack Alerts]
    end
    
    Liveness --> Prometheus
    Readiness --> Prometheus
    Startup --> Prometheus
    CustomMetrics --> Prometheus
    SystemMetrics --> Prometheus
    Prometheus --> AlertManager
    AlertManager --> EmailAlerts
    AlertManager --> SlackAlerts
```

## 🔄 CQRS Implementation

### Command and Query Separation

```mermaid
graph TB
    subgraph "Commands (Write Operations)"
        CreateOrder[Create Order Command]
        UpdateOrder[Update Order Command]
        CancelOrder[Cancel Order Command]
        CommandHandlers[Command Handlers]
    end
    
    subgraph "Queries (Read Operations)"
        GetOrder[Get Order Query]
        GetOrders[Get Orders Query]
        GetOrderHistory[Get Order History Query]
        QueryHandlers[Query Handlers]
    end
    
    subgraph "Domain Layer"
        OrderAggregate[Order Aggregate]
        OrderRepository[Order Repository]
    end
    
    subgraph "Infrastructure Layer"
        OrderDbContext[Order DbContext]
        ReadModels[Read Models]
    end
    
    CreateOrder --> CommandHandlers
    UpdateOrder --> CommandHandlers
    CancelOrder --> CommandHandlers
    CommandHandlers --> OrderAggregate
    OrderAggregate --> OrderRepository
    OrderRepository --> OrderDbContext
    
    GetOrder --> QueryHandlers
    GetOrders --> QueryHandlers
    GetOrderHistory --> QueryHandlers
    QueryHandlers --> ReadModels
    ReadModels --> OrderDbContext
```

## 🧪 Testing Strategy

### Testing Pyramid

```mermaid
graph TB
    subgraph "Unit Tests"
        DomainTests[Domain Tests]
        ApplicationTests[Application Tests]
        RepositoryTests[Repository Tests]
    end
    
    subgraph "Integration Tests"
        ServiceTests[Service Integration Tests]
        DatabaseTests[Database Integration Tests]
        ExternalAPITests[External API Tests]
    end
    
    subgraph "End-to-End Tests"
        APITests[API End-to-End Tests]
        UserJourneyTests[User Journey Tests]
        PerformanceTests[Performance Tests]
    end
    
    DomainTests --> ServiceTests
    ApplicationTests --> ServiceTests
    RepositoryTests --> DatabaseTests
    ServiceTests --> APITests
    DatabaseTests --> APITests
    ExternalAPITests --> APITests
    APITests --> UserJourneyTests
    UserJourneyTests --> PerformanceTests
```

This technical architecture document provides a comprehensive view of the SmartDeliveryApp's internal structure, data flows, and implementation details. The diagrams help visualize the complex relationships between different components and services. 