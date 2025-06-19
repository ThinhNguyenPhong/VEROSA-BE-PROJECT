# VEROSA Beauty - Backend API

An ASP.NET Core Web API backend for **VEROSA Beauty**, an e-commerce platform for beauty products and services.

---

## Table of Contents

* [Description](#description)
* [Technologies](#technologies)
* [Project Structure](#project-structure)
* [Prerequisites](#prerequisites)
* [Getting Started](#getting-started)
* [Database Setup](#database-setup)
* [Running the API](#running-the-api)
* [API Documentation](#api-documentation)
* [Features](#features)
* [Contributing](#contributing)
* [License](#license)
* [Contact](#contact)

---

## Description

VEROSA Beauty Backend is a RESTful API built with ASP.NET Core 8.0 and Entity Framework Core, powering the VEROSA Beauty e-commerce platform. It handles user authentication, product and order management, beauty service appointments, blog content, and customer support.

---

## Technologies

* **Framework:** ASP.NET Core 8.0
* **ORM:** Entity Framework Core
* **Database:** MySQL Server 8.0+
* **Authentication:** JWT Bearer tokens
* **Mapping:** AutoMapper
* **Security:** BCrypt for password hashing
* **Email:** MailKit
* **Documentation:** Swagger UI

---

## Project Structure

```
VEROSA-BE-PROJECT/                # Main API Project
├── Controllers/                  # API controllers
├── Mappers/                      # AutoMapper profiles
└── Program.cs                    # Application entry point

VEROSA.BusinessLogicLayer/       # Business logic
├── PasswordHash/                 # BCrypt helper
└── Services/                     # Domain services

VEROSA.Common/                   # Shared utilities
├── Enums/                        # Common enums
├── Exceptions/                   # Custom exception types
└── Models/                       # DTOs and shared models

VEROSA.DataAccessLayer/          # Data access
├── Context/                      # DbContext definitions
├── Entities/                     # EF Core entity classes
├── Migrations/                   # EF Core migrations
└── Repositories/                 # Repository implementations
```

---

## Prerequisites

* [.NET 8 SDK](https://dotnet.microsoft.com/download)
* [MySQL Server 8.0+](https://dev.mysql.com/downloads/mysql/)

---

## Getting Started

1. **Clone the repository**

   ```bash
   git clone https://github.com/your-username/VEROSA-BE-PROJECT.git
   cd VEROSA-BE-PROJECT
   ```
2. **Configure `appsettings.json`**

   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=VerosaBeauty;User=root;Password=YourPassword;"
     },
     "JwtSettings": {
       "Key": "YOUR_SECRET_KEY",
       "Issuer": "VEROSA",
       "Audience": "VEROSA_Audience",
       "DurationInMinutes": 60
     },
     "EmailSettings": {
       "SmtpServer": "smtp.example.com",
       "Port": 587,
       "Username": "user@example.com",
       "Password": "emailPassword"
     }
   }
   ```

---

## Database Setup

1. **Apply migrations**

   ```bash
   dotnet ef database update --project VEROSA.DataAccessLayer
   ```
2. **Verify**

   * A `VerosaBeauty` database should be created with all tables.

---

## Running the API

```bash
# From solution root
dotnet run --project VEROSA-BE-PROJECT
```

By default, the API will run on `https://localhost:5001`.

---

## API Documentation

Visit the Swagger UI for interactive documentation once the API is running:

```
https://localhost:5001/swagger
```

---

## Features

* **User Management**

  * Registration with email confirmation
  * JWT-based authentication
  * Role-based authorization
  * Secure password hashing (BCrypt)
* **Product Management**

  * Categories, product images, reviews, and favorites
* **Order Management**

  * Shopping cart, discount codes, and payment processing
* **Appointment System**

  * Scheduling beauty services, payment integration
* **Content Management**

  * Blog posts, images, and article types
* **Support System**

  * Contact form, support tickets, and customer service workflows

---

## Contributing

1. Fork the repository
2. Create a feature branch: `git checkout -b feature/YourFeature`
3. Commit your changes: `git commit -m "Add some feature"`
4. Push to branch: `git push origin feature/YourFeature`
5. Open a Pull Request

Please adhere to the existing code style and include tests for new functionality.

---

## License

This project is licensed under the [MIT License](LICENSE).

---

## Contact

For questions or support, open an issue or contact:

* **Thinh Nguyen** - [phongthinh799@gmail.com](mailto:phongthinh799@gmail.com)
