import { Injectable } from '@angular/core';
import { Observable, of, delay } from 'rxjs';
import { Restaurant, Category, MenuItem } from '../models/restaurant.model';
import { PaymentIntent, PaymentResult } from './payment.service';
import { CreateOrderRequest, Order, OrderItem } from './order.service';

@Injectable({
  providedIn: 'root'
})
export class MockDataService {
  private mockRestaurants: Restaurant[] = [
    {
      id: '1',
      name: 'Pizza Palace',
      description: 'Authentic Italian pizza and pasta',
      address: '123 Main St, Downtown',
      phoneNumber: '+1-555-0123',
      deliveryFee: 2.99,
      minOrderAmount: 15.00,
      isActive: true,
      averageRating: 4.5,
      coverImageUrl: 'assets/restaurants/pizza-palace.jpg',
      logoUrl: 'assets/restaurants/pizza-palace-logo.jpg',
      estimatedDeliveryTime: 25,
      categories: [
        {
          id: '1',
          name: 'Pizzas',
          displayOrder: 1,
          menuItems: [
            {
              id: '1',
              name: 'Margherita Pizza',
              description: 'Fresh mozzarella, tomato sauce, basil',
              price: 12.99,
              imageUrl: 'assets/menu/margherita-pizza.jpg',
              isVegetarian: true,
              isVegan: false,
              isAvailable: true,
              categoryId: '1'
            },
            {
              id: '2',
              name: 'Pepperoni Pizza',
              description: 'Spicy pepperoni, mozzarella, tomato sauce',
              price: 14.99,
              imageUrl: 'assets/menu/pepperoni-pizza.jpg',
              isVegetarian: false,
              isVegan: false,
              isAvailable: true,
              categoryId: '1'
            }
          ]
        },
        {
          id: '2',
          name: 'Pasta',
          displayOrder: 2,
          menuItems: [
            {
              id: '3',
              name: 'Spaghetti Carbonara',
              description: 'Eggs, cheese, pancetta, black pepper',
              price: 11.99,
              imageUrl: 'assets/menu/spaghetti-carbonara.jpg',
              isVegetarian: false,
              isVegan: false,
              isAvailable: true,
              categoryId: '2'
            }
          ]
        }
      ]
    },
    {
      id: '2',
      name: 'Burger House',
      description: 'Juicy burgers and crispy fries',
      address: '456 Oak Ave, Midtown',
      phoneNumber: '+1-555-0456',
      deliveryFee: 1.99,
      minOrderAmount: 12.00,
      isActive: true,
      averageRating: 4.3,
      coverImageUrl: 'assets/restaurants/burger-house.jpg',
      logoUrl: 'assets/restaurants/burger-house-logo.jpg',
      estimatedDeliveryTime: 20,
      categories: [
        {
          id: '3',
          name: 'Burgers',
          displayOrder: 1,
          menuItems: [
            {
              id: '4',
              name: 'Classic Cheeseburger',
              description: 'Beef patty, cheese, lettuce, tomato, onion',
              price: 9.99,
              imageUrl: 'assets/menu/classic-cheeseburger.jpg',
              isVegetarian: false,
              isVegan: false,
              isAvailable: true,
              categoryId: '3'
            },
            {
              id: '5',
              name: 'Veggie Burger',
              description: 'Plant-based patty, cheese, lettuce, tomato',
              price: 10.99,
              imageUrl: 'assets/menu/veggie-burger.jpg',
              isVegetarian: true,
              isVegan: false,
              isAvailable: true,
              categoryId: '3'
            }
          ]
        },
        {
          id: '4',
          name: 'Sides',
          displayOrder: 2,
          menuItems: [
            {
              id: '6',
              name: 'French Fries',
              description: 'Crispy golden fries with sea salt',
              price: 3.99,
              imageUrl: 'assets/menu/french-fries.jpg',
              isVegetarian: true,
              isVegan: true,
              isAvailable: true,
              categoryId: '4'
            },
            {
              id: '7',
              name: 'Onion Rings',
              description: 'Beer-battered onion rings',
              price: 4.99,
              imageUrl: 'assets/menu/onion-rings.jpg',
              isVegetarian: true,
              isVegan: false,
              isAvailable: true,
              categoryId: '4'
            }
          ]
        }
      ]
    },
    {
      id: '3',
      name: 'Sushi Express',
      description: 'Fresh sushi and Japanese cuisine',
      address: '789 Pine St, Uptown',
      phoneNumber: '+1-555-0789',
      deliveryFee: 3.99,
      minOrderAmount: 18.00,
      isActive: true,
      averageRating: 4.7,
      coverImageUrl: 'assets/restaurants/sushi-express.jpg',
      logoUrl: 'assets/restaurants/sushi-express-logo.jpg',
      estimatedDeliveryTime: 30,
      categories: [
        {
          id: '7',
          name: 'Sushi Rolls',
          displayOrder: 1,
          menuItems: [
            {
              id: '12',
              name: 'California Roll',
              description: 'Crab, avocado, cucumber',
              price: 8.99,
              imageUrl: 'assets/menu/california-roll.jpg',
              isVegetarian: false,
              isVegan: false,
              isAvailable: true,
              categoryId: '7'
            },
            {
              id: '13',
              name: 'Spicy Tuna Roll',
              description: 'Spicy tuna with cucumber',
              price: 10.99,
              imageUrl: 'assets/menu/spicy-tuna-roll.jpg',
              isVegetarian: false,
              isVegan: false,
              isAvailable: true,
              categoryId: '7'
            }
          ]
        },
        {
          id: '8',
          name: 'Sashimi',
          displayOrder: 2,
          menuItems: [
            {
              id: '14',
              name: 'Salmon Sashimi',
              description: 'Fresh salmon sashimi (6 pieces)',
              price: 12.99,
              imageUrl: 'assets/menu/salmon-sashimi.jpg',
              isVegetarian: false,
              isVegan: false,
              isAvailable: true,
              categoryId: '8'
            }
          ]
        },
        {
          id: '9',
          name: 'Appetizers',
          displayOrder: 3,
          menuItems: [
            {
              id: '15',
              name: 'Edamame',
              description: 'Steamed soybeans with sea salt',
              price: 4.99,
              imageUrl: 'assets/menu/edamame.jpg',
              isVegetarian: true,
              isVegan: true,
              isAvailable: true,
              categoryId: '9'
            }
          ]
        }
      ]
    }
  ];

  private mockOrders: Order[] = [];

  constructor() { }

  getRestaurants(): Observable<Restaurant[]> {
    return of(this.mockRestaurants);
  }

  getRestaurantById(id: string): Observable<Restaurant | undefined> {
    const restaurant = this.mockRestaurants.find(r => r.id === id);
    return of(restaurant);
  }

  getRestaurantDetails(id: string): Observable<Restaurant | undefined> {
    const restaurant = this.mockRestaurants.find(r => r.id === id);
    return of(restaurant);
  }

  // Payment methods
  createPaymentIntent(amount: number, currency: string = 'usd'): Observable<PaymentIntent> {
    const paymentIntent: PaymentIntent = {
      id: `pi_${Date.now()}_${Math.random().toString(36).substr(2, 9)}`,
      amount: amount,
      currency: currency,
      status: 'requires_payment_method'
    };
    return of(paymentIntent).pipe(delay(1000)); // Simulate network delay
  }

  confirmPayment(paymentIntentId: string): Observable<PaymentResult> {
    // Simulate random success/failure
    const success = Math.random() > 0.3; // 70% success rate
    
    const result: PaymentResult = {
      success: success,
      message: success ? 'Payment successful!' : 'Payment failed. Please try again.',
      transactionId: success ? `txn_${Date.now()}_${Math.random().toString(36).substr(2, 9)}` : undefined
    };
    
    return of(result).pipe(delay(1500)); // Simulate processing time
  }

  // Order methods
  createOrder(orderData: CreateOrderRequest): Observable<Order> {
    const order: Order = {
      orderId: `order_${Date.now()}_${Math.random().toString(36).substr(2, 9)}`,
      userId: orderData.userId,
      restaurantId: orderData.restaurantId,
      status: 'pending',
      totalAmount: orderData.totalAmount,
      createdAt: new Date().toISOString(),
      updatedAt: new Date().toISOString(),
      orderItems: orderData.orderItems.map((item, index) => ({
        id: index + 1,
        menuItemId: item.menuItemId,
        quantity: item.quantity,
        unitPrice: item.unitPrice,
        totalPrice: item.quantity * item.unitPrice
      }))
    };
    
    this.mockOrders.push(order);
    return of(order).pipe(delay(1000));
  }

  getOrderById(orderId: string): Observable<Order> {
    const order = this.mockOrders.find(o => o.orderId === orderId);
    if (order) {
      return of(order);
    }
    throw new Error('Order not found');
  }
} 