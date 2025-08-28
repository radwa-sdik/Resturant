# Resturant

> A small **ASP.NET Core** project for managing restaurant operations.

---

## Table of Contents
- [About](#about)
- [Features](#features)
- [Tech Stack](#tech-stack)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Setup & Run](#setup--run)
- [Project Structure](#project-structure)
---

## About
**Resturant** is a lightweight ASP.NET Core application designed to demonstrate core concepts in building a restaurant management system. It serves as a learning or starter project, perfect for exploring CRUD functionality, MVC patterns, and basic web app operations.

---
## Features
- Manage **Ingredients**  
  - View all ingredients  
  - Add new ingredients  
  - Edit existing ingredients  
  - Delete ingredients  

- Manage **Products**  
  - View all products  
  - Add new products with ingredient selection and image upload  
  - Edit existing products  
  - Delete products  

- **User & Cart**  
  - User authentication with session-based cart  
  - Add products to cart  
  - Place orders and view order history  

- **General**  
  - Input validation and error handling  
  - Responsive, user-friendly UI built with modern front-end technologies

---

## Tech Stack
- **Framework:** ASP.NET Core (latest stable version)
- **Backend Language:** C#
- **Frontend:** Razor Pages / MVC Views (HTML, CSS, optionally JavaScript)
- **Data Storage:** SQL Server with Entity Framework Core, plus session-based state management
- **Build & Run:** .NET CLI tools

---

## Getting Started

### Prerequisites
Ensure you have the following installed:
- [.NET SDK (6.x or above)](https://dotnet.microsoft.com/download)
- (Optional) A local database like SQL Server or SQLite if using database integrations

---

### Setup & Run

1. **Clone the repository**

   ```bash
   git clone https://github.com/radwa-sdik/Resturant.git
   cd Resturant
2. **Restore dependencies**
   ```bash
   dotnet restore
3. **Build the project**
   ```bash
   dotnet build
4. **Run the application**
   ```bash
   dotnet run
5. **Access the application**
   Visit https://localhost:5001 or http://localhost:5000 in your browser to explore the app.

---

## Project Structure
```bash
  Restaurant/
  ├── Areas/ # ASP.NET Core Identity (for authentication & user management)
  ├── Controllers/ # MVC controllers (Products, Ingredients, Orders, etc.)
  ├── Data/ # Database context (ApplicationDbContext)
  ├── Models/ # Entity models (Product, Ingredient, Order, etc.)
  ├── Repository/ # Generic repository pattern implementation
  ├── ViewModel/ # View models for passing data to views
  ├── Views/ # Razor views (UI pages)
  ├── wwwroot/ # Static files (CSS, JS, images)
  ├── appsettings.json # Configuration (connection strings, settings)
  ├── Program.cs # Application startup configuration
  └── Properties/ # Project properties (launchSettings.json, etc.)
