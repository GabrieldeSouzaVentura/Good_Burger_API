# Good_Burger_API
My First Web API Restful | Service system of a hamburger shop |

---

Good Burger is a RESTful Web API for a burger ordering system. It was developed in C# using ASP.NET Core with Entity Framework Core, featuring user authentication and an in-memory SQLite database.

## 🚀Good Burger Features:

- **User registration and login**
- **Create, retrieve, update, and delete orders**
- **Order organization and management**
- **jwt authentication with `user` and `admin` roles**

---
## Users

### 👨‍💻Exclusive
- **Create Role**
- **Add User Role**

### 👨‍🔧Admin
- **Revoke**

### 🍔User
- **Register**
- **Login**
- **Refresh Token**
- **Burger list**
- **Extra list**
- **Menu list**
- **Order: `Get`, `Post`, `Put` and `Delete`**

---

## 🛠️Main Technologies, Frameworks, and Libraries:

- **Backend: .NET C#**
- **Databases: SQLite (in-memory), MongoDB**

---

## ⚙️how to configure

### 1. Prerequisites
- **.Net 8.0**
- **MongoDb**
- **Visual Studio 2022**

### 2. Clone repository
````
gh repo clone GabrieldeSouzaVentura/Good_Burger_API
````
- **open with visual studio 2022**

---

## 🔧Endpoints

### 🎮AuthController 
- `Post/CreateRole` - Create Role(**Exclusive**)
- `Post/AddUserToRole` - Add users to roles(**Exclusive**)
- `Post/Login` - Login user
- `Post/Register` - Register user
- `Post/RefreshToken` - Refresh Token
- `Post/Revoke` - Revoke user (**Admin**)

### 🍔Burger
- `Get/GetBurger` - Get burger list (**User**)

### ➕Extra
- `Get/GetExtras` - Get extras list (**User**)

### 🙋‍♂️Order
- `Get/Order/MyOrder` - Get order list (**User**)
- `Post/Order/CreateOrder` - Place order(**User**)
- `Put/Order/UpdateOrder/{Id}` - Update order (**User**)
- `Delete/DeleteOrder/{Id}` - Delete the order(**User**)

---

The idea behind Good Burger is to provide a simple system that allows customers to place their orders quickly and easily.

## 🗂️Packages used in this project:

- **BCrypt.Net-Next – Provides secure password hashing using the BCrypt algorithm.**

- **Microsoft.AspNetCore.Authentication.JwtBearer – Supports JWT-based authentication in ASP.NET Core applications.**

- **Microsoft.AspNetCore.Identity.EntityFrameworkCore – Integrates ASP.NET Core Identity with Entity Framework Core.**

- **Microsoft.EntityFrameworkCore – Main ORM for working with databases in .NET.**

- **Microsoft.EntityFrameworkCore.Design – Tools for scaffolding and managing EF Core migrations.**

- **Microsoft.EntityFrameworkCore.Sqlite – EF Core provider for SQLite databases.**

- **Pomelo.EntityFrameworkCore.MySql – EF Core provider for MySQL databases.**

- **Serilog.AspNetCore – Structured logging for ASP.NET Core applications using Serilog.**

- **Serilog.Sinks.MongoDB – Enables logging with Serilog directly to MongoDB.**

- **Swashbuckle.AspNetCore – Automatically generates Swagger documentation for ASP.NET Core APIs.**

- **Swashbuckle.AspNetCore.SwaggerUI – Provides an interactive Swagger UI for testing API endpoints.**

- **System.Linq – Provides LINQ functionality for querying and manipulating collections.**