# 🚗 Pontus Car Shop

Welcome to the Pontus Car Shop project! This guide will help you set up and run the project locally.

## Prerequisites

- **.NET SDK**: Ensure you have the .NET SDK installed, targeting `.NET 7.0`.
- **Docker**: The project uses Docker for services like PostgreSQL and SMTP4DEV. Ensure Docker is installed and running.
- **Visual Studio or Visual Studio Code**: The project contains a Visual Studio solution file (`shop.sln`) and a Visual Studio Code workspace file (`PontusShop.code-workspace`).

## Project Structure

- **Core**: Contains the main business logic, entities, and configurations. It uses packages like `AutoMapper`, `MediatR`, `EntityFrameworkCore`, and more.
- **Web**: This is the web layer of the application, containing the client app built with React. It uses packages like `@coreui/coreui`, `@coreui/react`, and more.
- **Infrastructure**: (Not detailed, but typically contains data access code and other infrastructure concerns).
- **Docker**: The `docker-compose.yml` file in the root sets up services like PostgreSQL and SMTP4DEV.

## Setup

1. **Clone the Repository**:
   ```bash
   git clone https://github.com/pontusp02j/PontusCarShop.git
   cd PontusCarShop
Setup Environment Variables:

Copy the .env file from the root directory and set the POSTGRES_PASSWORD to password123.
Docker Services:

bash
Copy code
docker-compose up -d
Database Setup:

The connection string is set in core/appsettings.json to connect to the PostgreSQL instance. Ensure the database CarShopDB is created or adjust the connection string accordingly.
Run Migrations:

bash
Copy code
dotnet ef database update
Run the Application:

bash
Copy code
dotnet run --project ./Core/Core.csproj
Access the Web Application:

Open a browser and navigate to https://localhost:44420.
Development
React Client App: The React client app is located in Web/shop/clientApp. You can run it separately for development using npm start.
API Development: The API is built using FastEndpoints and other packages. Check the Program.cs files in both Core and Web for the main entry points.
Additional Notes
The application uses smtp4dev for email services, accessible at http://localhost:5000.
PostgreSQL is exposed on port 5432.