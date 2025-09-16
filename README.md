# AgroScan - Plant Management Application API

A comprehensive .NET 8 Web API for plant management and inspection analysis built with Clean Architecture principles.

## Features

- **JWT Authentication & Authorization**: Secure user authentication with role-based access control
- **Clean Architecture**: Separated layers for Models, Repositories, Services, and Controllers
- **Entity Framework Core**: Code-First approach with SQL Server database
- **Swagger Documentation**: Complete API documentation with JWT authentication support
- **CRUD Operations**: Full CRUD functionality for all entities
- **Role-Based Access**: Farmers can manage their own data, Admins can manage all data

## Project Structure

```
AgroScan/
├── src/
│   ├── AgroScan.API/                 # Web API layer
│   │   ├── Controllers/              # API Controllers
│   │   ├── Services/                 # Business logic services
│   │   └── Program.cs                # Application startup
│   ├── AgroScan.Core/                # Core business layer
│   │   ├── Entities/                 # Domain entities
│   │   ├── Enums/                    # Enumerations
│   │   ├── Interfaces/               # Repository interfaces
│   │   └── DTOs/                     # Data transfer objects
│   └── AgroScan.Infrastructure/      # Data access layer
│       ├── Data/                     # DbContext
│       ├── Repositories/             # Repository implementations
│       └── Migrations/               # Database migrations
└── AgroScan.sln                      # Solution file
```

## Database Schema

### Users Table
- `id` (PK) - Primary key
- `first_name` - User's first name
- `last_name` - User's last name
- `role` - User role (Farmer/Admin)
- `email` - User's email (unique)
- `password` - Hashed password
- `created_at` - Creation timestamp
- `updated_at` - Last update timestamp

### Inspections Table
- `id` (PK) - Primary key
- `plant_name` - Name of the plant
- `inspection_date` - Date of inspection
- `country` - Country location
- `state` - State location
- `city` - City location
- `notes` - Additional notes
- `status` - Inspection status
- `category` - Plant/Vegetable category
- `user_id` (FK) - Reference to Users table
- `created_at` - Creation timestamp
- `updated_at` - Last update timestamp

### Inspection Images Table
- `id` (PK) - Primary key
- `inspection_id` (FK) - Reference to Inspections table
- `image` - Image path/URL
- `created_at` - Creation timestamp
- `updated_at` - Last update timestamp

### Inspection Analysis Table
- `id` (PK) - Primary key
- `inspection_id` (FK) - Reference to Inspections table
- `status` - Analysis status
- `confidence_score` - Analysis confidence (0.0-1.0)
- `description` - Analysis description
- `treatment_recommendation` - Treatment recommendations
- `created_at` - Creation timestamp
- `updated_at` - Last update timestamp

## Prerequisites

- .NET 9 SDK
- Visual Studio 2022 or VS Code
- SQLite (included with .NET)

## Getting Started

### 1. Clone the Repository
```bash
git clone <repository-url>
cd backend
```

### 2. Update Connection String
The project is configured to use SQLite by default (cross-platform). The connection string in `src/AgroScan.API/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=AgroScanDb.db"
  }
}
```

### 3. Update JWT Settings
Update the JWT secret key in `src/AgroScan.API/appsettings.json`:
```json
{
  "JwtSettings": {
    "SecretKey": "YourSuperSecretKeyThatIsAtLeast32CharactersLong!",
    "Issuer": "AgroScanAPI",
    "Audience": "AgroScanUsers",
    "ExpirationMinutes": 60
  }
}
```

### 4. Build and Run
```bash
# Build the solution
dotnet build

# Run the API
cd src/AgroScan.API
dotnet run
```

The API will be available at:
- HTTP: `http://localhost:5002`
- Swagger UI: `http://localhost:5002` (root URL)

## API Endpoints

### Authentication
- `POST /api/auth/register` - Register a new user
- `POST /api/auth/login` - Login user and get JWT token

### Users (Admin only)
- `GET /api/users` - Get all users
- `GET /api/users/{id}` - Get user by ID
- `POST /api/users` - Create new user
- `PUT /api/users/{id}` - Update user
- `DELETE /api/users/{id}` - Delete user

### Inspections
- `GET /api/inspections` - Get user's inspections (or all if admin)
- `GET /api/inspections/{id}` - Get inspection by ID
- `POST /api/inspections` - Create new inspection
- `PUT /api/inspections/{id}` - Update inspection
- `DELETE /api/inspections/{id}` - Delete inspection

### Inspection Images
- `GET /api/inspectionimages/inspection/{inspectionId}` - Get images for inspection
- `GET /api/inspectionimages/{id}` - Get image by ID
- `POST /api/inspectionimages` - Upload new image
- `DELETE /api/inspectionimages/{id}` - Delete image

### Inspection Analysis
- `GET /api/inspectionanalyses/inspection/{inspectionId}` - Get analyses for inspection
- `GET /api/inspectionanalyses/inspection/{inspectionId}/latest` - Get latest analysis
- `GET /api/inspectionanalyses/{id}` - Get analysis by ID
- `POST /api/inspectionanalyses` - Create new analysis
- `PUT /api/inspectionanalyses/{id}` - Update analysis
- `DELETE /api/inspectionanalyses/{id}` - Delete analysis

## Authentication

### Register a New User
```json
POST /api/auth/register
{
  "firstName": "John",
  "lastName": "Doe",
  "email": "john.doe@example.com",
  "password": "password123"
}
```

### Login
```json
POST /api/auth/login
{
  "email": "john.doe@example.com",
  "password": "password123"
}
```

### Using JWT Token
Include the JWT token in the Authorization header:
```
Authorization: Bearer <your-jwt-token>
```

## User Roles

### Farmer
- Can create, read, update, and delete their own inspections
- Can manage images and analyses for their own inspections
- Cannot access other users' data

### Admin
- Can manage all users
- Can view and manage all inspections, images, and analyses
- Full system access

## Swagger Documentation

The API includes comprehensive Swagger documentation with:
- Complete endpoint descriptions
- Request/response schemas
- JWT authentication support
- Example requests and responses

Access Swagger UI at the root URL when running the application.

## Database Migrations

The project includes Entity Framework migrations. The database will be created automatically when you first run the application.

## Security Features

- JWT-based authentication
- Password hashing using BCrypt
- Role-based authorization
- CORS configuration
- Input validation
- SQL injection protection through Entity Framework

## Error Handling

The API includes comprehensive error handling with:
- Proper HTTP status codes
- Detailed error messages
- Logging for debugging
- Validation error responses

## Development

### Adding New Features
1. Create entities in `AgroScan.Core/Entities`
2. Add DTOs in `AgroScan.Core/DTOs`
3. Create repository interfaces in `AgroScan.Core/Interfaces`
4. Implement repositories in `AgroScan.Infrastructure/Repositories`
5. Create services in `AgroScan.API/Services`
6. Add controllers in `AgroScan.API/Controllers`
7. Update database context and create migrations

### Testing
The API is ready for testing with tools like:
- Postman
- Swagger UI
- Unit testing frameworks
- Integration testing

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request

## License

This project is licensed under the MIT License.
