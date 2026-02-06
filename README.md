# Library Management System

ASP.NET Core web application for managing library operations including books, authors, members, and borrowings.
###[Документација](https://github.com/user-attachments/files/25140412/Library.Management.System.-.pdf)

## University Project For:

### 1. Integrated Systems (IS)
- **System:** Full-stack Library Management System
- **Architecture:** Onion Architecture
- **Features:** Book/Author/Member CRUD, Borrowing system with fine calculation, Open Library API integration
- **Tech Stack:** ASP.NET Core MVC, Entity Framework Core, SQL Server

### 2. Software Quality and Testing Assurance (SKIT)
- **Testing Framework:** Playwright for .NET + xUnit
- **Total Tests:** 91 (31 Unit + 60 UI)
- **Coverage:** All modules, business logic, and user workflows

## System Features

- Books & Authors Management
- Member Registration & Profiles
- Book Borrowing & Returns
- Automated Fine Calculation ($1/day overdue)
- External Book Search (Open Library API)
- REST API Endpoints

## Technologies

**Backend:**
- ASP.NET Core 8.0 MVC
- Entity Framework Core
- SQL Server / LocalDB

**Testing:**
- Playwright for .NET (UI Testing)
- xUnit (Unit Testing)
- Moq (Mocking Framework)
- NUnit (Playwright integration)
