# PartManager

A modern electronic parts management system with a Vue.js frontend, .NET backend, and PostgreSQL database. Designed for organizing electronic components like ICs, transistors, and resistors in Gridfinity drawer systems.

## Features

- **Parts Management**: Add, edit, delete, and search electronic parts
- **Drawer Organization**: Manage physical storage drawers with Gridfinity grid positions
- **Category System**: Organize parts by categories (ICs, Transistors, Resistors, etc.)
- **Modern UI**: Responsive Vue.js interface with real-time updates
- **RESTful API**: Clean .NET Core Web API backend
- **PostgreSQL Database**: Reliable data storage with Entity Framework Core

## Tech Stack

- **Frontend**: Vue.js 3, Vue Router, Axios
- **Backend**: .NET 9.0, ASP.NET Core Web API
- **Database**: PostgreSQL 16
- **ORM**: Entity Framework Core with Npgsql

## Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download)
- [Node.js 20+](https://nodejs.org/)
- [PostgreSQL 16](https://www.postgresql.org/download/)
- [Docker](https://www.docker.com/) (optional, for PostgreSQL)

## Quick Start

### 1. Database Setup

#### Using Docker (Recommended)

```bash
docker-compose up -d
```

This will start a PostgreSQL container with the following credentials:
- **Database**: partmanager
- **Username**: postgres
- **Password**: postgres
- **Port**: 5432

#### Manual PostgreSQL Setup

If you prefer to use an existing PostgreSQL installation:

1. Create a database named `partmanager`
2. Update the connection string in `backend/PartManager.Api/appsettings.json`

### 2. Backend Setup

```bash
cd backend/PartManager.Api

# Restore dependencies
dotnet restore

# Create database migrations
dotnet ef migrations add InitialCreate

# Apply migrations to database
dotnet ef database update

# Run the backend API
dotnet run
```

The API will be available at `http://localhost:5000` and `https://localhost:5001`

### 3. Frontend Setup

```bash
cd frontend

# Install dependencies
npm install

# Run development server
npm run dev
```

The frontend will be available at `http://localhost:5173`

## Project Structure

```
PartManager/
├── backend/
│   └── PartManager.Api/
│       ├── Controllers/          # API endpoints
│       ├── Data/                 # DbContext and database configuration
│       ├── Models/               # Data models (Part, Drawer, Category)
│       ├── Program.cs            # Application entry point
│       └── appsettings.json      # Configuration
├── frontend/
│   ├── src/
│   │   ├── components/          # Reusable Vue components
│   │   ├── views/               # Page components
│   │   ├── router/              # Vue Router configuration
│   │   ├── services/            # API service layer
│   │   └── App.vue              # Root component
│   └── package.json
└── docker-compose.yml           # PostgreSQL container configuration
```

## API Endpoints

### Parts
- `GET /api/parts` - Get all parts
- `GET /api/parts/{id}` - Get a specific part
- `POST /api/parts` - Create a new part
- `PUT /api/parts/{id}` - Update a part
- `DELETE /api/parts/{id}` - Delete a part

### Drawers
- `GET /api/drawers` - Get all drawers
- `GET /api/drawers/{id}` - Get a specific drawer
- `POST /api/drawers` - Create a new drawer
- `PUT /api/drawers/{id}` - Update a drawer
- `DELETE /api/drawers/{id}` - Delete a drawer

### Categories
- `GET /api/categories` - Get all categories
- `GET /api/categories/{id}` - Get a specific category
- `POST /api/categories` - Create a new category
- `PUT /api/categories/{id}` - Update a category
- `DELETE /api/categories/{id}` - Delete a category

## Development

### Backend Development

```bash
cd backend/PartManager.Api
dotnet watch run
```

The API supports hot reload and will automatically restart on code changes.

### Frontend Development

```bash
cd frontend
npm run dev
```

Vite provides hot module replacement (HMR) for instant updates during development.

### Database Migrations

After modifying the data models:

```bash
cd backend/PartManager.Api
dotnet ef migrations add MigrationName
dotnet ef database update
```

## Production Build

### Frontend

```bash
cd frontend
npm run build
```

The production build will be created in the `frontend/dist` directory.

### Backend

```bash
cd backend/PartManager.Api
dotnet publish -c Release -o publish
```

## Configuration

### Backend Configuration

Update `backend/PartManager.Api/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=partmanager;Username=postgres;Password=postgres"
  }
}
```

### Frontend Configuration

Update `frontend/.env`:

```
VITE_API_URL=http://localhost:5000/api
```

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.