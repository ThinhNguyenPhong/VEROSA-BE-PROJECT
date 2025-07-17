# VEROSA Beauty E-commerce API

## Description

VEROSA Beauty is a comprehensive e-commerce platform for beauty and skincare products, built with ASP.NET Core. The API provides functionality for product management, user accounts, orders, reviews, appointments, and more.

## Table of Contents

* [Key Features](#key-features)
* [Tech Stack](#tech-stack)
* [Project Structure](#project-structure)
* [Prerequisites](#prerequisites)
* [Getting Started](#getting-started)
* [Authentication](#authentication)
* [Contributing](#contributing)
* [License](#license)

## Key Features

### Account Management

* User registration and authentication
* Role-based authorization (Customer, Admin)
* Profile management
* Address management

### Product Management

* Product categories
* Product listings with images
* Inventory tracking
* Pricing management

### Shopping Features

* Product reviews and ratings
* Favorites/wishlists
* Shopping cart
* Order processing
* Discount codes

### Beauty Services

* Service bookings/appointments
* Consultant scheduling
* Service ratings

### Content Management

* Blog posts
* Contact forms
* Support tickets

## Tech Stack

* **Framework:** ASP.NET Core
* **ORM:** Entity Framework Core
* **Database:** MySQL 8.0.33+
* **Authentication:** JWT (JSON Web Tokens)
* **Object Mapping:** AutoMapper
* **Email Service:** MailKit
* **Password Hashing:** BCrypt

## Project Structure

```
VEROSA-BE-PROJECT/
│
├── Controllers/         # API controllers
├── Models/              # EF Core entity models
├── Data/                # DbContext and migrations
├── Services/            # Business logic services
├── DTOs/                # Data transfer objects
├── Profiles/            # AutoMapper profiles
├── Helpers/             # Utility and helper classes
├── appsettings.json     # Configuration settings
└── Program.cs           # Application entry point
```

## Prerequisites

* .NET 6.0 or later
* MySQL 8.0.33 or later
* Visual Studio 2022 or VS Code

## Getting Started

1. **Clone the repository:**

   ```bash
   git clone <repository-url>
   cd VEROSA-BE-PROJECT
   ```

2. **Configure the database:**

   * Update `appsettings.json` with your MySQL connection string:

     ```json
     {
       "ConnectionStrings": {
         "DefaultConnection": "Server=localhost;Database=verosa_beauty;Uid=your_user;Pwd=your_password;"
       }
     }
     ```

3. **Apply database migrations:**

   ```bash
   dotnet ef database update
   ```

4. **Run the application:**

   ```bash
   dotnet run
   ```

## Authentication

Protected endpoints require a valid Bearer token in the `Authorization` header. Obtain a token by registering or logging in via the provided account management endpoints.

## Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/YourFeature`)
3. Commit your changes (`git commit -m "Add YourFeature"`)
4. Push to the branch (`git push origin feature/YourFeature`)
5. Submit a pull request


