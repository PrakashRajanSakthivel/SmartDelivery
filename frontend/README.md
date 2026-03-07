# SmartDelivery Frontend

A modern Angular application for the SmartDelivery food delivery platform.

## 🚀 Features

- **Restaurant Browsing**: Grid view of available restaurants
- **Menu Navigation**: Categorized menu items with filtering
- **Shopping Cart**: Add/remove items with quantity controls
- **Checkout Process**: Payment form with mock payment processing
- **Responsive Design**: Works on desktop and mobile devices
- **Material Design**: Modern UI with Angular Material components

## 🛠️ Technology Stack

- **Framework**: Angular 17
- **UI Library**: Angular Material
- **State Management**: RxJS BehaviorSubject
- **Styling**: CSS with Material Design theme
- **HTTP Client**: Angular HttpClient for API calls

## 📁 Project Structure

```
frontend/
├── src/
│   ├── app/
│   │   ├── components/
│   │   │   ├── restaurant-list/
│   │   │   ├── restaurant-menu/
│   │   │   ├── cart/
│   │   │   └── checkout/
│   │   ├── services/
│   │   │   ├── restaurant.service.ts
│   │   │   ├── cart.service.ts
│   │   │   └── payment.service.ts
│   │   ├── models/
│   │   │   └── restaurant.model.ts
│   │   ├── app.component.ts
│   │   ├── app.module.ts
│   │   └── app-routing.module.ts
│   ├── assets/
│   ├── index.html
│   ├── main.ts
│   └── styles.css
├── package.json
├── angular.json
└── README.md
```

## 🚀 Getting Started

### Prerequisites

- Node.js (v18 or higher)
- npm or yarn
- Angular CLI

### Installation

1. **Install dependencies**
   ```bash
   npm install
   ```

2. **Configure API endpoints**
   Update the API URLs in the services:
   - `restaurant.service.ts`: Restaurant API endpoint
   - `payment.service.ts`: Payment API endpoint

3. **Start development server**
   ```bash
   npm start
   ```

4. **Open browser**
   Navigate to `http://localhost:4200`

## 🔧 Configuration

### API Endpoints

Update the following service files with your backend API URLs:

```typescript
// restaurant.service.ts
private apiUrl = 'http://localhost:5001/api/restaurants';

// payment.service.ts
private apiUrl = 'http://localhost:5003/api/payments';
```

### CORS Configuration

Ensure your backend services have CORS enabled for the frontend domain.

## 📱 User Flow

1. **Home Page**: User sees a grid of restaurants
2. **Restaurant Selection**: Click on a restaurant to view its menu
3. **Menu Browsing**: Browse categorized menu items
4. **Add to Cart**: Click "Add to Cart" or use quantity controls
5. **Cart Review**: View cart items and proceed to checkout
6. **Payment**: Fill payment form and process payment
7. **Order Confirmation**: Success/failure message with next steps

## 🎨 UI Components

### Restaurant List
- Grid layout with restaurant cards
- Restaurant images, ratings, and delivery info
- Hover effects and smooth transitions

### Restaurant Menu
- Tabbed interface for menu categories
- Menu item cards with images and descriptions
- Quantity controls and add to cart buttons
- Fixed cart summary at bottom

### Shopping Cart
- List of cart items with quantity controls
- Order summary with subtotal and delivery fee
- Proceed to checkout button

### Checkout
- Payment form with validation
- Order summary sidebar
- Payment processing with loading states
- Success/failure dialogs

## 🔄 State Management

The application uses RxJS BehaviorSubject for state management:

- **Cart State**: Managed by `CartService`
- **API State**: Loading, error, and success states
- **Form State**: Reactive forms with validation

## 📱 Responsive Design

The application is fully responsive with:
- Mobile-first approach
- Flexible grid layouts
- Touch-friendly controls
- Optimized for different screen sizes

## 🧪 Testing

```bash
# Run unit tests
npm test

# Run e2e tests
npm run e2e
```

## 🚀 Build

```bash
# Build for production
npm run build

# Build for development
npm run build --configuration development
```

## 📦 Deployment

The application can be deployed to any static hosting service:

- **Netlify**: Drag and drop the `dist` folder
- **Vercel**: Connect your GitHub repository
- **AWS S3**: Upload the `dist` folder
- **Azure Static Web Apps**: Deploy from GitHub

## 🔧 Development

### Adding New Components

```bash
ng generate component components/new-component
```

### Adding New Services

```bash
ng generate service services/new-service
```

### Code Style

The project follows Angular style guide:
- Use Angular Material components
- Follow TypeScript best practices
- Use reactive forms for user input
- Implement proper error handling

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request

## 📄 License

This project is licensed under the MIT License. 