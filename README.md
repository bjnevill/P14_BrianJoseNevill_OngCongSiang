# Gruberoo — Food Delivery Management System

**Ngee Ann Polytechnic | Diploma in Data Science | Programming II | Team academic project (2026)**

A C# console application modelling the operational workflows of a food-delivery service, including customers, restaurant menus, orders, refunds and reporting. Built as a **school assignment**, not a production service.

## What this project demonstrates

- **Object-oriented design:** separate classes for customers, restaurants, menus, food items, orders, special offers and saved favourites.
- **Business process modelling:** order creation and lifecycle transitions, including preparation, delivery, cancellation and rejection.
- **Data handling:** CSV-based inputs and persisted records for orders and related operations.
- **Data structures:** lists, dictionaries, queues and stacks for tracking operational data.
- **Business analytics:** reporting on order activity and key operational measures.

## Features

| Area | Functionality |
| --- | --- |
| Restaurant management | Load restaurant information, menus and food items |
| Customer ordering | Create, view, modify and cancel orders |
| Order processing | Process pending orders and update order statuses |
| Refund handling | Record rejected/cancelled orders for refunds |
| Favourites | Save and manage reusable orders |
| Operational reporting | Summarise orders and delivery-related performance |

**Business rules implemented in the assignment:** fulfilled orders contribute to sales reporting; cancelled or rejected orders are excluded from successful-order revenue; the case study specifies a 30% platform commission on eligible orders, excluding delivery charges.

## Repository structure

```text
S10274174G_PRG2Assignment.sln
S10274174G_PRG2Assignment/
  Program.cs
  Customer.cs
  Restaurant.cs
  Menu.cs
  FoodItem.cs
  OrderedFoodItem.cs
  Order.cs
  SpecialOffer.cs
  Favourite.cs
  *.csv
```

## Running locally

1. Clone the repository.
2. Open `S10274174G_PRG2Assignment.sln` in Visual Studio with the required .NET/C# tooling.
3. Run the console project. The supplied CSV files are sample assignment data.

The exact .NET SDK and runtime requirements should be checked against the included `.csproj` file before building from a command line.

## Context and limitations

This is a **collaborative student assignment**, developed with a teammate; it is presented as a team project, not solely individual work. It demonstrates educational implementation of order-handling workflows, rather than production-scale reliability or deployed infrastructure.

## Relevant skills

`C#` · `Object-Oriented Programming` · `CSV Data Handling` · `Business Process Modelling` · `Operational Reporting`

---

*For other analytics and finance-oriented coursework, see the owner's GitHub profile as additional repositories are published.*
