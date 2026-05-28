[Back to README](../README.md)

## Sales API

The Sales API implements a complete CRUD for sales records, including item-level discount rules, sale cancellation, item cancellation, pagination, filtering, and ordering.

### Create Sale

`POST /api/sales`

```json
{
  "saleNumber": "SALE-0001",
  "saleDate": "2026-05-28T22:00:00Z",
  "customerId": "11111111-1111-1111-1111-111111111111",
  "customerName": "Patrick Otto",
  "branchId": "22222222-2222-2222-2222-222222222222",
  "branchName": "DeveloperStore Sao Paulo",
  "items": [
    {
      "productId": "33333333-3333-3333-3333-333333333333",
      "productName": "Notebook",
      "quantity": 4,
      "unitPrice": 2500
    },
    {
      "productId": "44444444-4444-4444-4444-444444444444",
      "productName": "Mouse",
      "quantity": 2,
      "unitPrice": 100
    }
  ]
}
```

Expected result:

- The notebook item receives a 10% discount.
- The mouse item receives no discount.
- The sale total is calculated from active item totals.
- A `SaleCreatedEvent` is published through the Rebus in-memory bus and handled in the application layer for logging.

### List Sales

`GET /api/sales?page=1&size=10`

Optional filters:

- `saleNumber`
- `customerId`
- `branchId`
- `isCancelled`
- `orderBy`

Supported `orderBy` values:

- `saleDate`
- `-saleDate`
- `saleNumber`
- `-saleNumber`
- `totalAmount`
- `-totalAmount`

Example:

`GET /api/sales?page=1&size=5&isCancelled=false&orderBy=-totalAmount`

### Get Sale

`GET /api/sales/{id}`

### Update Sale

`PUT /api/sales/{id}`

Use the same payload shape as `POST /api/sales`.

Expected result:

- The sale data and items are replaced.
- Discounts and totals are recalculated.
- A `SaleModifiedEvent` is published through the Rebus in-memory bus and handled in the application layer for logging.

### Cancel Sale

`PATCH /api/sales/{id}/cancel`

Expected result:

- The sale is marked as cancelled.
- All items are marked as cancelled.
- The sale total becomes zero.
- A `SaleCancelledEvent` is published through the Rebus in-memory bus and handled in the application layer for logging.

### Cancel Sale Item

`PATCH /api/sales/{id}/items/{itemId}/cancel`

Expected result:

- Only the selected item is marked as cancelled.
- The sale total is recalculated with the remaining active items.
- An `ItemCancelledEvent` is published through the Rebus in-memory bus and handled in the application layer for logging.

### Delete Sale

`DELETE /api/sales/{id}`

### Business Rule Test Matrix

| Quantity | Expected Discount |
| --- | --- |
| 1 to 3 | 0% |
| 4 to 9 | 10% |
| 10 to 20 | 20% |
| 21 or more | Request rejected |
