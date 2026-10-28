# Gruberoo Food Delivery Management System

Team academic project Â· Ngee Ann Polytechnic Â· Programming II Â· Semester 2 folder, October 2025 coursework

## Operational problem

Model a food-delivery service's customer orders, restaurant menus, order lifecycle and refunds in a C# console application. The project provides practice in operational data handling and reconciliation rules.

## Methodology, ownership and tools

C# / .NET 8, object-oriented classes, dictionaries, queues, stacks and CSV persistence separate customers, restaurants, menus, food items, orders, offers and favourites.

Brian implemented assignment questions 2, 3, 5 and 7, bulk processing and favourite orders. The teammate implemented questions 1, 4, 6 and 8, totals and analytics. Original authorship comments are retained. The application is a joint project; the analytics feature is not claimed as Brian's individual work.

## Implemented features and findings

The application loads case-study fixtures, creates and modifies orders, processes status changes, handles cancellations/refunds and produces operational totals. The assignment's reporting rules distinguish delivered orders from cancelled or rejected orders and apply a 30% platform commission to eligible amounts excluding delivery charges. These are coded case rules, not achieved commercial metrics.

## Repository layout

```text
Gruberoo.sln
src/Gruberoo/
  Gruberoo.csproj
  Program.cs
  Customer.cs, Restaurant.cs, Menu.cs, FoodItem.cs
  Order.cs, OrderedFoodItem.cs, SpecialOffer.cs, Favourite.cs
  *.csv
docs/  # privacy, provenance and validation notes
```

## Run locally

Install the .NET 8 SDK, or a newer SDK capable of targeting .NET 8 with the .NET 8 runtime available.

```powershell
dotnet build Gruberoo.sln
Set-Location src/Gruberoo
dotnet run --project Gruberoo.csproj
```

Run from `src/Gruberoo`: input files are resolved relative to the working directory. Menu option `0` exits. The application can write order/favourite/refund files; use a disposable clone when exploring it.

## Data and privacy

CSV identities and delivery addresses were replaced with clearly labelled demo values. Transaction amounts, status, dates and food line items were preserved. Student identifiers were removed from source comments, namespace and paths. See [privacy and provenance](docs/PRIVACY_AND_PROVENANCE.md) and [validation](docs/VALIDATION.md). Original D: files and Git history are preserved. This PR keeps the repository private; historical commits still need separate consideration before making it public.

## Limitations

This is an educational console program with local CSV storage, not a deployed service. There is no production authentication, transactional database or comprehensive automated test suite. Build success and a startup smoke check do not establish correctness of every order/refund edge case.
