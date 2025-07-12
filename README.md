# AgriEnergy Connect

**AgriEnergy Connect** is a modern web application built with **C# in Visual Studio Code** using **ASP.NET Core MVC**. It empowers farmers to manage agricultural products, collaborate, and grow their agribusiness. Designed with simplicity and scalability in mind, this platform supports secure login, product entry via dropdowns, and structured farmer engagement.

---

##  Purpose & Design Decisions

The purpose of **AgriEnergy Connect** is to streamline digital farming operations, giving users the ability to:

- Add and track agricultural products
- Choose from preset categories (no manual typing errors)
- Assign products to registered farmers
- Use secure login and role-based access
- Navigate easily with a clean and responsive interface

###  Design Highlights

- ASP.NET Core MVC for modular structure
- Replaced manual category entry with dropdowns
- Data storage using Entity Framework Core and SQL Server
- `TempData` success feedback and conditional routing

---

##  Features

###  Authentication & Role Management
- Secure registration and login
- Role-based access (Employee vs. Farmer)
- Built using ASP.NET Core Identity

###  Product Management
- Add product with:
  - Product Name
  - Category (dropdown list)
  - Production Date
  - Linked Farmer
- Validation and success message handling
- Data stored in SQL Server via EF Core

###  Farmer Integration
- View and select registered farmers via dropdown
- Farmer info fetched from database in real time

###  Additional Pages
- Support  
- Learn & Grow  
- Collaborate & Get Funded  

---


##  Notes

- Products are saved to the database via EF Core
- Category is now selected via a predefined dropdown list
- Project is fully built in Visual Studio Code with C#
- All required dropdown data is pre-populated in controllers
- Includes navigation to additional helpful pages

---

## Author

- Full Name: Dianca Jade Naidu
- Email: diancanaidu@gmail.com

---

## References
- Microsoft. (2024). ASP.NET Core MVC Overview. Microsoft Learn. Available at: https://learn.microsoft.com/en-us/aspnet/core/mvc/overview (Accessed: 20 June 2025).
- Microsoft. (2024). Identity in ASP.NET Core. Microsoft Learn. Available at: https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity (Accessed: 20 June 2025).
- Microsoft. (2024). Entity Framework Core Documentation. Microsoft Learn. Available at: https://learn.microsoft.com/en-us/ef/core/ (Accessed: 20 June 2025).
- Bootstrap. (2025). Bootstrap 5 Documentation. Available at: https://getbootstrap.com/docs/5.3/getting-started/introduction/ (Accessed: 20 June 2025).
- Stack Overflow. (2025). Binding Dropdown in ASP.NET Core MVC. Available at: https://stackoverflow.com/questions/31914358/asp-net-core-mvc-dropdown-list (Accessed: 20 June 2025).
- W3Schools. (2025). HTML Select Dropdown Tutorial. Available at: https://www.w3schools.com/tags/tag_select.asp (Accessed: 20 June 2025).
