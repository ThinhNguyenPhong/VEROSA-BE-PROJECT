VEROSA Beauty - Backend API
An ASP.NET Core Web API backend for VEROSA Beauty, an e-commerce platform for beauty products and services.

Technologies
ASP.NET Core 8.0
Entity Framework Core
MySQL Database
JWT Authentication
AutoMapper
BCrypt Password Hashing
MailKit Email Service
Swagger UI
Project Structure
Features
User Management

Registration with email confirmation
Authentication with JWT tokens
Role-based authorization
Password hashing with BCrypt
Product Management

Categories
Images
Reviews
Favorites
Order Management

Shopping cart
Discount codes
Payment processing
Appointment System

Beauty services
Scheduling
Payments
Content Management

Blog posts
Images
Article types
Support System

Contact form
Support tickets
Customer service
Database Schema
Key entities include:

Accounts
Products
Orders
Appointments
Services
BlogPosts
Reviews
SupportTickets
Getting Started
Prerequisites:

.NET 8 SDK
MySQL Server 8.0+
Configure connection string in appsettings.json:

Run migrations:
Start the API:
Browse Swagger UI at https://localhost:5001/swagger
Authentication
The API uses JWT bearer tokens for authentication. Include the token in requests:

Contributing
Fork the repository
Create a feature branch
Commit changes
Push to the branch
Submit a pull request
License
This project is proprietary software. All rights reserved.
