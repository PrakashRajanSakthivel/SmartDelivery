import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { CartItem, Cart } from '../models/restaurant.model';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private cartSubject = new BehaviorSubject<CartItem[]>([]);
  public cart$ = this.cartSubject.asObservable();

  private userId = 'user-123'; // In real app, this would come from auth service
  private restaurantId: string | null = null;

  constructor() {
    // Load cart from localStorage on service initialization
    this.loadCartFromStorage();
  }

  getCart(): Observable<CartItem[]> {
    return this.cart$;
  }

  getCartValue(): CartItem[] {
    return this.cartSubject.value;
  }

  // Add method that components expect
  getItems(): CartItem[] {
    return this.cartSubject.value;
  }

  addItem(item: CartItem): void {
    const currentCart = this.cartSubject.value;
    const existingItem = currentCart.find(cartItem => cartItem.menuItemId === item.menuItemId);

    if (existingItem) {
      existingItem.quantity += item.quantity;
      existingItem.totalPrice = existingItem.quantity * existingItem.unitPrice;
    } else {
      // Generate a unique ID for the cart item
      const cartItem: CartItem = {
        ...item,
        id: `cart-${Date.now()}-${Math.random().toString(36).substr(2, 9)}`
      };
      currentCart.push(cartItem);
    }

    this.cartSubject.next([...currentCart]);
    this.saveCartToStorage();
  }

  removeItem(menuItemId: string): void {
    const currentCart = this.cartSubject.value;
    const updatedCart = currentCart.filter(item => item.menuItemId !== menuItemId);
    
    this.cartSubject.next(updatedCart);
    this.saveCartToStorage();
  }

  updateItemQuantity(menuItemId: string, quantity: number): void {
    const currentCart = this.cartSubject.value;
    const item = currentCart.find(cartItem => cartItem.menuItemId === menuItemId);

    if (item) {
      if (quantity <= 0) {
        this.removeItem(menuItemId);
      } else {
        item.quantity = quantity;
        item.totalPrice = item.quantity * item.unitPrice;
        this.cartSubject.next([...currentCart]);
        this.saveCartToStorage();
      }
    }
  }

  // Add method that components expect (alias for updateItemQuantity)
  updateQuantity(menuItemId: string, quantity: number): void {
    this.updateItemQuantity(menuItemId, quantity);
  }

  clearCart(): void {
    this.cartSubject.next([]);
    this.restaurantId = null;
    this.saveCartToStorage();
  }

  getCartTotal(): number {
    return this.cartSubject.value.reduce((total, item) => total + item.totalPrice, 0);
  }

  getCartItemCount(): number {
    return this.cartSubject.value.reduce((count, item) => count + item.quantity, 0);
  }

  setRestaurantId(restaurantId: string): void {
    this.restaurantId = restaurantId;
  }

  getRestaurantId(): string | null {
    return this.restaurantId;
  }

  private saveCartToStorage(): void {
    const cartData = {
      items: this.cartSubject.value,
      restaurantId: this.restaurantId,
      userId: this.userId
    };
    localStorage.setItem('smartDeliveryCart', JSON.stringify(cartData));
  }

  private loadCartFromStorage(): void {
    const cartData = localStorage.getItem('smartDeliveryCart');
    if (cartData) {
      try {
        const parsed = JSON.parse(cartData);
        this.cartSubject.next(parsed.items || []);
        this.restaurantId = parsed.restaurantId || null;
      } catch (error) {
        console.error('Error loading cart from storage:', error);
        this.cartSubject.next([]);
      }
    }
  }
} 