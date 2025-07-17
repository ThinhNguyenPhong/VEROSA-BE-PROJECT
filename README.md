# SkinCare Product Sale

## Description

SkinCare Product Sale is a web application for managing and selling skincare products, built with ASP.NET Core and Entity Framework Core. It streamlines product catalog management, order processing, customer relationships, and promotional campaigns.

## Key Features

* **Product Management**: Create, edit, delete, and categorize products.
* **Shopping Cart**: Add products to the cart, adjust quantities, and view order totals.
* **Order Processing**: Create orders, track status updates, and manage order history.
* **Customer Management**: Maintain customer profiles, shipping addresses, and purchase history.
* **Promotion System**: Create discount codes and apply promotional offers to orders.
* **Payment Handling**: Integrate with payment gateways and manage payment statuses.
* **Authentication & Authorization**: Role-based access control with Admin, Customer, and Employee roles.
* **Admin Dashboard**: A comprehensive interface for administrators to manage all aspects of the system.

## Project Structure

```
SkinCare-Product-Sale/
├── Business_Layer/         # Business logic layer
│   ├── Services/           # Business service implementations
│   └── Business_Layer.csproj
├── Data_Access_Layer/      # Data access layer
│   ├── DBContext/          # Entity Framework database context
│   ├── Entities/           # Entity models
│   ├── Migrations/         # EF Core migrations
│   ├── Repositories/       # Repository classes
│   ├── Requests/           # Request DTOs
│   └── Responses/          # Response DTOs
└── SkinCare-Product-Sale/  # Web application
    ├── Controllers/        # MVC controllers
    ├── Models/             # View models
    ├── Views/              # Razor views
    └── wwwroot/            # Static files (CSS, JS, images)
```

## Technologies Used

* ASP.NET Core 6.0+
* Entity Framework Core
* SQL Server / SQL Server Express
* Bootstrap 5
* jQuery
* HTML, CSS, JavaScript

## Prerequisites

* [.NET SDK 6.0 or later](https://dotnet.microsoft.com/download)
* SQL Server or SQL Server Express

## Getting Started

1. **Clone the repository:**

   ```bash
   git clone https://github.com/username/SkinCare-Product-Sale.git
   cd SkinCare-Product-Sale
   ```
2. **Configure the database connection:**

   * Open `appsettings.json` in the `SkinCare-Product-Sale` folder.
   * Update the `DefaultConnection` string with your SQL Server details:

     ```json
     "ConnectionStrings": {
       "DefaultConnection": "Server=YOUR_SERVER;Database=SkinCareSaleDb;Trusted_Connection=True;"
     }
     ```
3. **Apply EF Core migrations:**

   ```bash
   cd Data_Access_Layer
   dotnet ef database update
   ```
4. **Run the application:**

   ```bash
   cd ../SkinCare-Product-Sale
   dotnet run
   ```
5. **Access the app:**

   * Open your browser and navigate to `https://localhost:5001` (or the specified port).

## Authentication & Authorization

The application uses ASP.NET Core Identity with role-based access control:

* **Admin**: Full permissions, including managing products, orders, promotions, and users.
* **Customer**: Can browse products, place orders, and view their own order history.
* **Employee**: Can manage orders and customer information.

## Contributing

1. Fork this repository.
2. Create a new branch for your feature or bug fix:

   ```bash
   git checkout -b feature/FeatureName
   ```
3. Commit your changes:

   ```bash
   git commit -m "Add FeatureName"
   ```
4. Push to the branch:

   ```bash
   git push origin feature/FeatureName
   ```
5. Open a Pull Request.


