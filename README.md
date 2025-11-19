# HR Management System

A comprehensive, modern HR Management System built with .NET 10 Blazor WebApp and SQL Server Express.

## Project Overview

This is an enterprise-grade HR management platform designed for SMBs (50-500 employees). It provides complete functionality for:

- Employee management and organizational structure
- Recruitment and applicant tracking
- Leave and attendance management
- Performance evaluations
- Payroll and benefits
- Calendar and event scheduling
- Reporting and analytics

## Technology Stack

- **Framework**: .NET 10 with Blazor WebApp
- **UI**: Blazor Server Components (Interactive)
- **Database**: SQL Server Express 2022
- **ORM**: Entity Framework Core 10
- **Authentication**: ASP.NET Core Identity + JWT
- **Logging**: Serilog
- **Validation**: FluentValidation
- **Mapping**: AutoMapper
- **Testing**: xUnit + Moq

## Project Structure

```
HRManagement/
├── src/
│   ├── HRManagement.Web/           # Main Blazor WebApp
│   ├── HRManagement.Data/          # EF Core DbContext & Entities
│   ├── HRManagement.Services/      # Business logic
│   ├── HRManagement.Models/        # DTOs & Models
│   └── HRManagement.sln            # Solution file
├── tests/
│   └── HRManagement.Tests/         # Unit & Integration tests
├── docs/
│   ├── 01-FEATURE-ROADMAP.md       # Feature phases and priorities
│   ├── 02-DATA-MODEL.md             # Complete database schema
│   ├── 03-ARCHITECTURE-PLAN.md      # System architecture
│   └── 04-SETUP-GUIDE.md            # Detailed setup instructions
├── .gitignore
└── README.md
```

## Prerequisites

- **.NET 10 SDK** - https://dotnet.microsoft.com/download
- **SQL Server Express 2022** - https://www.microsoft.com/sql-server/sql-server-downloads
- **Visual Studio 2022** (or VS Code) - https://visualstudio.microsoft.com/
- **Git** - https://git-scm.com/

## Quick Start

### 1. Verify Prerequisites

```bash
# Check .NET version
dotnet --version

# Should be 10.x.x
```

### 2. Verify SQL Server Instance

Ensure SQL Server Express is running:
- Open **SQL Server Management Studio (SSMS)**
- Connect to `(local)\SQLEXPRESS`
- Verify connection succeeds

### 3. Open Project

```bash
# Navigate to src folder
cd src

# Open solution in Visual Studio
start HRManagement.sln

# Or restore and build from command line
dotnet restore
dotnet build
```

### 4. Create Database

```bash
cd HRManagement.Web

# Apply migration (creates database)
dotnet ef database update -p ../HRManagement.Data/HRManagement.Data.csproj -c HRContext
```

### 5. Run Application

```bash
# In HRManagement.Web directory
dotnet run

# Or press F5 in Visual Studio
```

**Application will be available at**: https://localhost:7001

## Database Setup

### Connection String

Edit `appsettings.json` to match your SQL Server instance:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(local)\\SQLEXPRESS;Database=HRManagement;Integrated Security=true;Encrypt=false;TrustServerCertificate=true;"
}
```

### Apply Migrations

```bash
cd src/HRManagement.Web

# Create migration
dotnet ef migrations add <MigrationName> -p ../HRManagement.Data/HRManagement.Data.csproj

# Apply migration
dotnet ef database update
```

## Development Workflow

### Running Tests

```bash
cd tests/HRManagement.Tests
dotnet test
```

### Building Solution

```bash
cd src
dotnet build
```

### Cleaning Build

```bash
cd src
dotnet clean
```

## Project Configuration

### appsettings.json

**DefaultConnection**: SQL Server connection string for HR Management database
**IdentityConnection**: SQLite connection string for Identity/Auth database
**Jwt**: JWT token configuration
**Logging**: Serilog configuration

### Program.cs

Key configurations:
- Serilog logging setup
- DbContext registration (HRContext for HR, ApplicationDbContext for Identity)
- Authentication/Authorization
- Dependency injection

## Documentation

Complete documentation is available in the `docs/` folder:

1. **01-FEATURE-ROADMAP.md** - Feature priorities and phases
2. **02-DATA-MODEL.md** - Complete database schema (20+ tables)
3. **03-ARCHITECTURE-PLAN.md** - System design and architecture
4. **04-SETUP-GUIDE.md** - Detailed step-by-step setup

## Current Status

✅ Project structure created
✅ All dependencies installed
✅ DbContext configured
✅ Initial migration created
✅ Git repository initialized
⏳ Database migration (awaiting SQL Server instance)
⏳ Core modules development

## Next Steps

1. Ensure SQL Server Express is installed and running
2. Run `dotnet ef database update` to create the database
3. Test application startup: `dotnet run`
4. Begin building core modules:
   - Authentication & Authorization
   - Employee Management
   - Recruitment Module
   - Leave & Attendance

## NuGet Packages

### Core
- `Microsoft.EntityFrameworkCore` 10.0.0
- `Microsoft.EntityFrameworkCore.SqlServer` 10.0.0
- `Microsoft.AspNetCore.Identity.EntityFrameworkCore` 10.0.0

### Services
- `AutoMapper` 13.0.1
- `AutoMapper.Extensions.Microsoft.DependencyInjection` 12.0.1
- `FluentValidation` 11.9.2

### Logging
- `Serilog` 4.2.0
- `Serilog.AspNetCore` 9.0.0
- `Serilog.Sinks.Console` 6.0.0

### Testing
- `xUnit` 2.7.1
- `Moq` 4.20.70

## Troubleshooting

### SQL Server Not Found

**Error**: "A network-related or instance-specific error occurred"

**Solution**:
1. Open SQL Server Configuration Manager
2. Verify SQL Server Express (SQLEXPRESS) service is running
3. Check connection string in appsettings.json

### EF Core Tools Mismatch

**Error**: "The Entity Framework tools version is older..."

**Solution**: Install latest EF Core tools
```bash
dotnet tool update --global dotnet-ef
```

### Port Already in Use

**Error**: "Address already in use"

**Solution**: Change port in `Properties/launchSettings.json`

## Git Setup

### Initial Commit (Already Done)
```bash
git init
git add .
git commit -m "Initial project setup with Blazor WebApp, project structure, and EF Core configuration"
```

### Connect to GitHub

```bash
git remote add origin https://github.com/YOUR_USERNAME/HRManagement.git
git branch -M main
git push -u origin main
```

## Features in Development

### Phase 1 (MVP)
- Authentication & Authorization
- Employee Management
- Recruitment (ATS)
- Leave & Attendance
- Performance Evaluations
- User & Access Management
- Admin & Configuration
- Basic Reporting

### Phase 2
- Payroll & Compensation
- Benefits Management
- Calendar & Events
- Contract Management
- Skills & Competencies
- Training & Development
- Expense Management
- Advanced Reporting

### Phase 3+
- Advanced Performance Management
- Organization & Workforce Management
- Mobile Apps
- API Integrations
- Multi-tenancy

## License

Choose your preferred license (MIT, Apache 2.0, proprietary, etc.)

## Support

For setup issues, refer to:
- `docs/04-SETUP-GUIDE.md` - Detailed installation guide
- `docs/03-ARCHITECTURE-PLAN.md` - System architecture documentation

## Roadmap

See `docs/01-FEATURE-ROADMAP.md` for complete feature roadmap and timeline.
