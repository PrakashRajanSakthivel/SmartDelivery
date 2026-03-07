import { Component, OnInit } from '@angular/core';
import { CartService } from '../../services/cart.service';
import { CartItem } from '../../models/cart-item.model';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css']
})
export class CartComponent implements OnInit {
  cartItems: CartItem[] = [];
  subtotal = 0;
  deliveryFee = 2.99;
  total = 0;

  constructor(private cartService: CartService) {}

  ngOnInit() {
    this.loadCart();
  }

  loadCart() {
    this.cartItems = this.cartService.getItems();
    this.calculateTotals();
  }

  calculateTotals() {
    this.subtotal = this.cartItems.reduce((sum, item) => sum + item.totalPrice, 0);
    this.total = this.subtotal + this.deliveryFee;
  }

  increaseQuantity(item: CartItem) {
    this.cartService.updateQuantity(item.menuItemId, item.quantity + 1);
    this.loadCart();
  }

  decreaseQuantity(item: CartItem) {
    if (item.quantity > 1) {
      this.cartService.updateQuantity(item.menuItemId, item.quantity - 1);
      this.loadCart();
    }
  }

  removeItem(item: CartItem) {
    this.cartService.removeItem(item.menuItemId);
    this.loadCart();
  }

  clearCart() {
    this.cartService.clearCart();
    this.loadCart();
  }

  proceedToCheckout() {
    // Navigate to checkout page
    console.log('Proceeding to checkout...');
  }

  getFoodIcon(itemName: string): string {
    const name = itemName.toLowerCase();
    if (name.includes('pizza')) return 'local_pizza';
    if (name.includes('burger')) return 'fastfood';
    if (name.includes('sushi') || name.includes('roll')) return 'set_meal';
    if (name.includes('pasta') || name.includes('spaghetti')) return 'ramen_dining';
    if (name.includes('salad')) return 'eco';
    if (name.includes('fries')) return 'fastfood';
    if (name.includes('chicken')) return 'restaurant';
    if (name.includes('fish') || name.includes('salmon')) return 'set_meal';
    if (name.includes('steak') || name.includes('beef')) return 'restaurant';
    if (name.includes('soup')) return 'soup_kitchen';
    if (name.includes('coffee') || name.includes('latte')) return 'local_cafe';
    if (name.includes('cake') || name.includes('dessert')) return 'cake';
    if (name.includes('ice cream')) return 'icecream';
    if (name.includes('drink') || name.includes('soda')) return 'local_bar';
    if (name.includes('bread') || name.includes('toast')) return 'bakery_dining';
    if (name.includes('egg')) return 'egg';
    if (name.includes('milk') || name.includes('cream')) return 'local_drink';
    return 'restaurant'; // default icon
  }
} 