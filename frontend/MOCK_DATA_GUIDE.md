# Mock Data Guide for SmartDeliveryApp Frontend

## Overview

This guide explains how to use mock data for testing the Angular frontend before integrating with the real .NET backend services.

## Current Mock Data Setup

### 1. Mock Data Service (`src/app/services/mock-data.service.ts`)

The `MockDataService` provides realistic test data that matches your backend API structure:

- **Restaurants**: 3 sample restaurants (Pizza Palace, Burger House, Sushi Express)
- **Menu Items**: Categorized menu items with prices and images
- **Payment Responses**: Simulated payment intents and confirmations
- **Order Data**: Mock order creation and retrieval

### 2. Configuration (`src/app/config/api.config.ts`)

Easy toggle between mock and real APIs:

```typescript
export const API_CONFIG: ApiConfig = {
  useMockData: true, // Set to false to use real APIs
  apiUrls: {
    restaurant: 'http://localhost:5001/api/restaurants',
    order: 'http://localhost:5002/api/orders',
    payment: 'http://localhost:5003/api/payments'
  }
};
```

### 3. Service Integration

All services (`RestaurantService`, `PaymentService`, `OrderService`) automatically use mock data when `useMockData` is `true`.

## How to Use Mock Data

### 1. Enable Mock Data
```typescript
// In src/app/config/api.config.ts
export const API_CONFIG: ApiConfig = {
  useMockData: true, // This enables mock data
  // ... rest of config
};
```

### 2. Test the UI
1. Start the Angular app: `ng serve`
2. Navigate through the app:
   - Browse restaurants
   - Add items to cart
   - Proceed to checkout
   - Complete payment flow

### 3. Switch to Real APIs
```typescript
// In src/app/config/api.config.ts
export const API_CONFIG: ApiConfig = {
  useMockData: false, // This enables real API calls
  // ... rest of config
};
```

## Mock Data Features

### Realistic Data Structure
- Matches your backend DTOs exactly
- Includes proper relationships (restaurants → categories → menu items)
- Realistic pricing and delivery fees

### Simulated Network Delays
- Restaurant data: 800ms delay
- Payment intent: 1000ms delay
- Payment confirmation: 1500ms delay
- Order creation: 1200ms delay

### Error Simulation
- Payment failures (10% chance in mock)
- Network timeouts
- Invalid data responses

## Alternatives to Mocky

### 1. **JSON Server** (Recommended)
```bash
npm install -g json-server
```

Create `db.json`:
```json
{
  "restaurants": [
    {
      "id": 1,
      "name": "Pizza Palace",
      "description": "Authentic Italian pizza",
      "isActive": true,
      "deliveryFee": 2.99,
      "minOrderAmount": 15.00,
      "averageRating": 4.5
    }
  ],
  "menuItems": [
    {
      "id": 1,
      "restaurantId": 1,
      "name": "Margherita Pizza",
      "description": "Classic tomato sauce with mozzarella",
      "price": 18.99,
      "categoryId": 1,
      "isAvailable": true
    }
  ]
}
```

Run:
```bash
json-server --watch db.json --port 3001
```

Update API config:
```typescript
apiUrls: {
  restaurant: 'http://localhost:3001/restaurants',
  // ...
}
```

### 2. **MSW (Mock Service Worker)**
```bash
npm install msw --save-dev
```

Create `src/mocks/handlers.ts`:
```typescript
import { rest } from 'msw'

export const handlers = [
  rest.get('/api/restaurants', (req, res, ctx) => {
    return res(
      ctx.status(200),
      ctx.json([
        {
          id: 1,
          name: 'Pizza Palace',
          // ... more data
        }
      ])
    )
  }),
]
```

### 3. **Angular In-Memory Web API**
```bash
npm install angular-in-memory-web-api --save-dev
```

Create `src/app/services/in-memory-data.service.ts`:
```typescript
import { InMemoryDbService } from 'angular-in-memory-web-api';

export class InMemoryDataService implements InMemoryDbService {
  createDb() {
    const restaurants = [
      { id: 1, name: 'Pizza Palace', /* ... */ }
    ];
    return { restaurants };
  }
}
```

### 4. **Local JSON Files**
Create `src/assets/mock-data/restaurants.json`:
```json
[
  {
    "id": 1,
    "name": "Pizza Palace",
    "description": "Authentic Italian pizza"
  }
]
```

Use in service:
```typescript
getRestaurants(): Observable<Restaurant[]> {
  return this.http.get<Restaurant[]>('/assets/mock-data/restaurants.json');
}
```

### 5. **Faker.js for Dynamic Data**
```bash
npm install @faker-js/faker
```

```typescript
import { faker } from '@faker-js/faker';

const mockRestaurants = Array.from({ length: 10 }, () => ({
  id: faker.number.int(),
  name: faker.company.name(),
  description: faker.lorem.sentence(),
  deliveryFee: faker.number.float({ min: 1, max: 5 }),
  // ...
}));
```

## Comparison of Alternatives

| Method | Pros | Cons | Best For |
|--------|------|------|----------|
| **Current Mock Service** | ✅ Full control<br>✅ TypeScript support<br>✅ Easy switching | ❌ Static data<br>❌ Manual maintenance | Development & testing |
| **JSON Server** | ✅ RESTful API<br>✅ CRUD operations<br>✅ Real HTTP calls | ❌ Setup required<br>❌ Limited relationships | Full API simulation |
| **MSW** | ✅ Intercepts real requests<br>✅ Service worker<br>✅ Production ready | ❌ Complex setup<br>❌ Learning curve | Advanced mocking |
| **In-Memory Web API** | ✅ Angular native<br>✅ Easy setup<br>✅ CRUD operations | ❌ Limited features<br>❌ Angular specific | Angular projects |
| **Local JSON** | ✅ Simple<br>✅ No dependencies<br>✅ Easy to edit | ❌ No dynamic data<br>❌ No relationships | Simple static data |
| **Faker.js** | ✅ Dynamic data<br>✅ Realistic data<br>✅ Many data types | ❌ Bundle size<br>❌ Random data | Dynamic test data |

## Recommended Approach

### For Your Current Needs:
1. **Keep the current MockDataService** - it's working well and matches your backend structure
2. **Use JSON Server** for more complex scenarios or when you need CRUD operations
3. **Consider MSW** when you want to intercept real API calls in production

### Migration Path:
1. **Phase 1**: Use current mock service for UI development
2. **Phase 2**: Implement your C# backend services
3. **Phase 3**: Switch `useMockData` to `false` in config
4. **Phase 4**: Use MSW for testing edge cases and error scenarios

## Testing Scenarios

### Happy Path Testing
- Browse restaurants ✅
- Add items to cart ✅
- Proceed to checkout ✅
- Complete payment ✅
- Order confirmation ✅

### Error Scenarios
- Network failures
- Payment declines
- Invalid form data
- Empty cart checkout
- Restaurant unavailable

### Edge Cases
- Large orders
- Special characters in data
- Very long text fields
- Zero quantity items
- Duplicate items in cart

## Next Steps

1. **Test the current mock setup** with the Angular app
2. **Choose an alternative** if you need more features
3. **Build your C# backend services**
4. **Switch to real APIs** when ready
5. **Add comprehensive error handling**

The current mock data setup should be sufficient for testing your UI and user flows before building the backend services. 