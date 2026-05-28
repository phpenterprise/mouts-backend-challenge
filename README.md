# Developer Evaluation Project

`READ CAREFULLY`

## Use Case
**You are a developer on the DeveloperStore team. Now we need to implement the API prototypes.**

As we work with `DDD`, to reference entities from other domains, we use the `External Identities` pattern with denormalization of entity descriptions.

Therefore, you will write an API (complete CRUD) that handles sales records. The API needs to be able to inform:

* Sale number
* Date when the sale was made
* Customer
* Total sale amount
* Branch where the sale was made
* Products
* Quantities
* Unit prices
* Discounts
* Total amount for each item
* Cancelled/Not Cancelled

It's not mandatory, but it would be a differential to build code for publishing events of:
* SaleCreated
* SaleModified
* SaleCancelled
* ItemCancelled

If you write the code, **it's not required** to actually publish to any Message Broker. You can log a message in the application log or however you find most convenient.

### Business Rules

* Purchases above 4 identical items have a 10% discount
* Purchases between 10 and 20 identical items have a 20% discount
* It's not possible to sell above 20 identical items
* Purchases below 4 items cannot have a discount

These business rules define quantity-based discounting tiers and limitations:

1. Discount Tiers:
   - 4+ items: 10% discount
   - 10-20 items: 20% discount

2. Restrictions:
   - Maximum limit: 20 items per product
   - No discounts allowed for quantities below 4 items

## Implementation

The backend implementation is available in [`template/backend`](./template/backend) and follows the template's layered architecture:

- Domain: `Sale` aggregate and `SaleItem` business rules.
- Application: MediatR commands and handlers for sale workflows, plus Rebus in-memory event publishing.
- Infrastructure: EF Core mappings, migrations, and repository implementation.
- WebApi: REST endpoints for CRUD and cancellation operations.

### Running locally

From `template/backend`, run:

```bash
docker compose up -d ambev.developerevaluation.database ambev.developerevaluation.nosql ambev.developerevaluation.cache
dotnet restore
dotnet ef database update --project src/Ambev.DeveloperEvaluation.ORM --startup-project src/Ambev.DeveloperEvaluation.WebApi
dotnet run --project src/Ambev.DeveloperEvaluation.WebApi
```

Swagger is enabled in development at `/swagger`.

### Tests

From `template/backend`, run:

```bash
dotnet test Ambev.DeveloperEvaluation.sln
```

### Sales API

- `POST /api/sales`: create a sale.
- `GET /api/sales`: list sales with pagination, filters, and ordering.
- `GET /api/sales/{id}`: retrieve one sale.
- `PUT /api/sales/{id}`: update a sale.
- `PATCH /api/sales/{id}/cancel`: cancel a sale and all items.
- `PATCH /api/sales/{id}/items/{itemId}/cancel`: cancel a single sale item.
- `DELETE /api/sales/{id}`: delete a sale.

Manual test examples are available in [Sales API](./.doc/sales-api.md) and [Tests](./TESTS.md).

Discounts are calculated automatically from item quantity:

- 1-3 identical items: no discount.
- 4-9 identical items: 10% discount.
- 10-20 identical items: 20% discount.
- Above 20 identical items: rejected.

## Overview
This section provides a high-level overview of the project and the various skills and competencies it aims to assess for developer candidates. 

See [Overview](/.doc/overview.md)

## Tech Stack
This section lists the key technologies used in the project, including the backend, testing, frontend, and database components. 

See [Tech Stack](/.doc/tech-stack.md)

## Frameworks
This section outlines the frameworks and libraries that are leveraged in the project to enhance development productivity and maintainability. 

See [Frameworks](/.doc/frameworks.md)

<!-- 
## API Structure
This section includes links to the detailed documentation for the different API resources:
- [API General](./docs/general-api.md)
- [Products API](/.doc/products-api.md)
- [Carts API](/.doc/carts-api.md)
- [Users API](/.doc/users-api.md)
- [Auth API](/.doc/auth-api.md)
-->

## Project Structure
This section describes the overall structure and organization of the project files and directories. 

See [Project Structure](/.doc/project-structure.md)
