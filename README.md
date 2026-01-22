# Task Management API

A comprehensive Task Management API built with ASP.NET Core 8.0, featuring microservice architecture with RabbitMQ for event-driven communication, Docker containerization, and secure environment variable management.

## Microservice & RabbitMQ Architecture

This project implements a microservice architecture with the following components:

### Architecture Overview

```
┌─────────────────┐      ┌─────────────────┐      ┌─────────────────┐
│   Task API      │──────│   RabbitMQ      │──────│   Audit API     │
│  (Main Service) │      │  (Message Broker)│     │  (Audit Service)│
└─────────────────┘      └─────────────────┘      └─────────────────┘
         │                        │                        │
         │                        │                        │
         ▼                        ▼                        ▼
┌─────────────────┐      ┌─────────────────┐      ┌─────────────────┐
│  SQL Server     │      │                 │      │   MongoDB       │
│  (Task Data)    │      │                 │      │  (Audit Logs)   │
└─────────────────┘      └─────────────────┘      └─────────────────┘
```

### Components

1. **Task API** (Main Service)
   - Handles task and project management
   - User authentication with JWT
   - CRUD operations for tasks, projects, and user assignments
   - Publishes events to RabbitMQ for audit logging

2. **Audit API** (Microservice)
   - Consumes events from RabbitMQ
   - Stores audit logs in MongoDB
   - Provides audit trail for all system activities
   - Separate service for scalability and decoupling

3. **RabbitMQ** (Message Broker)
   - Facilitates asynchronous communication between services
   - Ensures reliable message delivery
   - Enables event-driven architecture
   - Queue: `activities` for audit events

4. **SQL Server** (Primary Database)
   - Stores application data (tasks, projects, users)
   - Entity Framework Core migrations
   - Transactional data management

5. **MongoDB** (Audit Database)
   - Stores audit logs from Audit API
   - Flexible document storage for audit trails
   - Separate from primary database for performance

### Event Flow

1. User performs an action in Task API (e.g., creates a task)
2. Task API publishes an event to RabbitMQ
3. Audit API consumes the event from RabbitMQ
4. Audit API stores the audit log in MongoDB
5. Task API continues processing without waiting for audit completion

### Benefits

- **Decoupling**: Services operate independently
- **Scalability**: Each service can scale independently
- **Reliability**: RabbitMQ ensures message delivery
- **Performance**: Async processing doesn't block main operations
- **Maintainability**: Clear separation of concerns

## Features

- **User Authentication**: JWT-based authentication for secure access
- **Role Management**: Admin and User roles for managing access and permissions
- **CRUD Operations**: Create, read, update, and delete operations for tasks and projects
- **Many-to-Many Relationships**: Between users and tasks via `UserTask` entity
- **Microservice Architecture**: Event-driven communication with RabbitMQ
- **Audit Logging**: Separate microservice for tracking all activities
- **Docker Support**: Full containerization with Docker Compose
- **Environment Variable Management**: Secure configuration with .env files
- **Swagger Integration**: API documentation and testing via Swagger UI

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/get-started) and [Docker Compose](https://docs.docker.com/compose/install/)
- Git

## Quick Start with Docker

### 1. Clone the Repository

```bash
git clone https://github.com/AbdelrahmanMohamed-m/Task_Managment_API.git
cd Task_Managment_API
```

### 2. Configure Environment Variables

Copy the example environment file and update it with your values:

```bash
cp .env.example .env
```

**Important**: Never commit the `.env` file to version control. It contains sensitive information.

Edit `.env` and update the following variables:

```env
# Database Configuration
DB_SERVER=sqlserver
DB_NAME=TaskDb
DB_USER=sa
DB_PASSWORD=YourStrongPasswordHere!

# RabbitMQ Configuration
RABBITMQ_HOST=rabbitmq
RABBITMQ_PORT=5672
RABBITMQ_USERNAME=guest
RABBITMQ_PASSWORD=guest
RABBITMQ_QUEUE=activities

# MongoDB Configuration
MONGO_CONNECTION_STRING=mongodb://mongodb:27017
MONGO_DATABASE=AuditDb

# Stripe Configuration
STRIPE_SECRET_KEY=sk_test_your_stripe_secret_key_here

# JWT Configuration
JWT_ISSUER=http://localhost:5248
JWT_AUDIENCE=http://localhost:5248
JWT_SIGNING_KEY=your_jwt_signing_key_here

# Audit API Configuration
AUDIT_API_BASE_URL=http://audit-api:8080

# Application Configuration
ASPNETCORE_ENVIRONMENT=Development
ASPNETCORE_URLS=http://+:8080
```

### Environment Variables Reference

| Variable | Description | Example |
|----------|-------------|---------|
| `DB_SERVER` | SQL Server hostname | `sqlserver` |
| `DB_NAME` | Database name | `TaskDb` |
| `DB_USER` | Database username | `sa` |
| `DB_PASSWORD` | Database password | `YourStrongPassword123!` |
| `RABBITMQ_HOST` | RabbitMQ hostname | `rabbitmq` |
| `RABBITMQ_PORT` | RabbitMQ port | `5672` |
| `RABBITMQ_USERNAME` | RabbitMQ username | `guest` |
| `RABBITMQ_PASSWORD` | RabbitMQ password | `guest` |
| `RABBITMQ_QUEUE` | RabbitMQ queue name | `activities` |
| `MONGO_CONNECTION_STRING` | MongoDB connection string | `mongodb://mongodb:27017` |
| `MONGO_DATABASE` | MongoDB database name | `AuditDb` |
| `STRIPE_SECRET_KEY` | Stripe API secret key | `sk_test_...` |
| `JWT_ISSUER` | JWT token issuer | `http://localhost:5248` |
| `JWT_AUDIENCE` | JWT token audience | `http://localhost:5248` |
| `JWT_SIGNING_KEY` | JWT signing key | `your-secret-key-here` |
| `AUDIT_API_BASE_URL` | Audit API base URL | `http://audit-api:8080` |
| `ASPNETCORE_ENVIRONMENT` | ASP.NET Core environment | `Development` |
| `ASPNETCORE_URLS` | Application URLs | `http://+:8080` |

### 3. Build and Run with Docker Compose

```bash
docker compose up -d --build
```

This will:
- Build and start SQL Server
- Build and start RabbitMQ
- Build and start MongoDB
- Build and start Audit API
- Build and start Task API

### 4. Verify Services

Check that all services are running:

```bash
docker compose ps
```

Access the services:
- **Task API**: http://localhost:5000
- **Task API Swagger**: http://localhost:5000/swagger
- **Audit API**: http://localhost:5080
- **RabbitMQ Management UI**: http://localhost:15672 (guest/guest)

### 5. Stop Services

```bash
docker compose down
```

To remove volumes (delete all data):

```bash
docker compose down -v
```

## Local Development

### Option 1: Using User Secrets (Recommended)

User secrets store configuration data in a file located in the user profile directory, outside of your project tree. This is the recommended approach for local development.

1. Initialize user secrets:

```bash
dotnet user-secrets init
```

2. Set your secrets:

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Data Source=YOUR_SERVER;Initial Catalog=TaskManagement;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
dotnet user-secrets set "Stripe:SecretKey" "sk_test_your_stripe_secret_key_here"
dotnet user-secrets set "JWT:signingKey" "your_jwt_signing_key_here"
dotnet user-secrets set "RabbitMQ:HostName" "localhost"
dotnet user-secrets set "RabbitMQ:Port" "5672"
dotnet user-secrets set "RabbitMQ:UserName" "guest"
dotnet user-secrets set "RabbitMQ:Password" "guest"
dotnet user-secrets set "RabbitMQ:QueueName" "activities"
dotnet user-secrets set "AuditApi:BaseUrl" "http://localhost:5081"
```

3. Run the application:

```bash
dotnet run
```

### Option 2: Using Environment Variables

Set environment variables in your system before running the application.

**Windows (PowerShell):**

```powershell
$env:ConnectionStrings__DefaultConnection="Data Source=YOUR_SERVER;Initial Catalog=TaskManagement;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
$env:Stripe__SecretKey="sk_test_your_stripe_secret_key_here"
$env:JWT__signingKey="your_jwt_signing_key_here"
$env:RabbitMQ__HostName="localhost"
$env:RabbitMQ__Port="5672"
$env:RabbitMQ__UserName="guest"
$env:RabbitMQ__Password="guest"
$env:RabbitMQ__QueueName="activities"
$env:AuditApi__BaseUrl="http://localhost:5081"
dotnet run
```

**Linux/Mac:**

```bash
export ConnectionStrings__DefaultConnection="Data Source=YOUR_SERVER;Initial Catalog=TaskManagement;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
export Stripe__SecretKey="sk_test_your_stripe_secret_key_here"
export JWT__signingKey="your_jwt_signing_key_here"
export RabbitMQ__HostName="localhost"
export RabbitMQ__Port="5672"
export RabbitMQ__UserName="guest"
export RabbitMQ__Password="guest"
export RabbitMQ__QueueName="activities"
export AuditApi__BaseUrl="http://localhost:5081"
dotnet run
```

### Database Setup for Local Development

1. Update the connection string in your user secrets or environment variables
2. Run migrations:

```bash
dotnet ef database update
```

## Configuration

### Environment Variables

The application uses environment variables for configuration. ASP.NET Core automatically maps environment variables with `__` (double underscore) to nested configuration sections.

**Example:**
- Environment variable: `ConnectionStrings__DefaultConnection`
- Maps to: `ConnectionStrings:DefaultConnection` in appsettings.json

### Configuration Files

- **`appsettings.json`**: Base configuration with empty placeholders for secrets
- **`appsettings.Development.json`**: Development-specific settings
- **`.env`**: Environment variables for Docker (not committed to git)
- **`.env.example`**: Template for environment variables (committed to git)

### Security Best Practices

1. **Never commit `.env` file**: It contains sensitive information
2. **Use `.env.example`**: Provide a template for required variables
3. **Use User Secrets**: For local development, use `dotnet user-secrets`
4. **Rotate secrets regularly**: Change passwords and keys periodically
5. **Use strong passwords**: Follow security best practices for all credentials

## Project Structure

```
Task_Managment_API/
├── Config/                    # Configuration classes
├── DataLayer/                 # Data access layer
│   ├── Data/                  # Database context
│   └── Entities/              # Entity models
├── DomainLayer/               # Domain logic
│   ├── ExceptionHandler/      # Global exception handling
│   ├── IRepo/                 # Repository interfaces
│   └── Repo/                  # Repository implementations
├── Extension/                 # Extension methods
├── Migrations/               # Entity Framework migrations
├── Options/                   # Configuration options classes
├── PresentationLayer/         # API controllers
│   └── Controllers/           # API endpoints
├── ServiceLayer/             # Business logic layer
│   ├── Dto/                   # Data transfer objects
│   ├── IService/              # Service interfaces
│   ├── Mappers/               # Object mapping
│   └── Service/               # Service implementations
├── compose.yaml              # Docker Compose configuration
├── Dockerfile                # Docker image configuration
├── .dockerignore             # Files to exclude from Docker
├── .env                      # Environment variables (not committed)
├── .env.example              # Environment variables template
└── Program.cs                # Application entry point
```

## API Endpoints

### Authentication
- `POST /api/account/register` - Register a new user
- `POST /api/account/login` - Login and get JWT token
- `PUT /api/account/profile` - Update user profile

### Projects
- `POST /api/project` - Create a new project
- `GET /api/project` - Get all projects for the logged-in user
- `GET /api/project/{id}` - Get a project by ID
- `PUT /api/project/{id}` - Update an existing project
- `DELETE /api/project/{id}` - Delete a project

### Tasks
- `POST /api/tasks` - Create a new task
- `GET /api/tasks` - Get all tasks for the logged-in user
- `GET /api/tasks/{id}` - Get a task by ID
- `PUT /api/tasks/{id}` - Update an existing task
- `DELETE /api/tasks/{id}` - Delete a task

### User Projects
- `POST /api/userproject` - Assign a user to a project
- `GET /api/userproject` - Get all user-project assignments
- `DELETE /api/userproject/{id}` - Remove user from project

### User Tasks
- `POST /api/usertask` - Assign a user to a task
- `GET /api/usertask` - Get all user-task assignments
- `DELETE /api/usertask/{id}` - Remove user from task

### Payments (Stripe Integration)
- `POST /api/payment/create-customer` - Create a Stripe customer
- `POST /api/payment/create-subscription` - Create a subscription
- `POST /api/payment/customer-portal` - Get customer portal URL

## Testing

### Run Unit Tests

```bash
dotnet test
```

### Run with Coverage

```bash
dotnet test --collect:"XPlat Code Coverage"
```

## Troubleshooting

### Docker Issues

**Container won't start:**
```bash
docker compose logs task-api
docker compose logs sqlserver
docker compose logs rabbitmq
```

**Database connection issues:**
- Ensure SQL Server container is healthy: `docker compose ps`
- Check connection string in `.env` file
- Verify password matches between `.env` and SQL Server container

**RabbitMQ connection issues:**
- Check RabbitMQ is running: `docker compose ps`
- Access RabbitMQ Management UI: http://localhost:15672
- Verify queue exists in RabbitMQ Management UI

### Local Development Issues

**Migration errors:**
```bash
dotnet ef migrations remove
dotnet ef migrations add InitialCreate
dotnet ef database update
```

**Configuration not loading:**
- Verify environment variables are set correctly
- Check user secrets: `dotnet user-secrets list`
- Ensure `appsettings.json` has correct structure

## Recent Changes

### Database Schema Fixes
- Fixed conflicting primary key definitions in `ApplicationDBContext.cs`
- Removed unused `TaskId` property from `Tasks` entity
- Updated `UserTasks` table to use composite primary key `(UserId, TaskId)`
- Applied migration to fix IDENTITY property on `UserTasks.Id` column

### Security Improvements
- Implemented environment variable-based configuration
- Removed hardcoded secrets from `appsettings.json`
- Added `.env` file for Docker configuration
- Added `.env.example` as a template
- Updated `.dockerignore` to exclude `.env` files

### Docker Configuration
- Updated `compose.yaml` to use environment variables from `.env`
- Configured health checks for all services
- Set up proper service dependencies
- Added volume persistence for databases

## Contributing

Contributions are welcome! Please follow these guidelines:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

### Development Guidelines

- Follow existing code style and conventions
- Write unit tests for new features
- Update documentation as needed
- Ensure all tests pass before submitting PR
- Use meaningful commit messages

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Support

For issues, questions, or contributions, please open an issue on GitHub.

---

**Note**: This project uses microservice architecture with RabbitMQ for event-driven communication. Ensure all services are running when testing the full system.
