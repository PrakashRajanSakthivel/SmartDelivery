import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CartService } from '../../services/cart.service';
import { PaymentService } from '../../services/payment.service';
import { OrderService, CreateOrderRequest, OrderItemDto } from '../../services/order.service';
import { CartItem } from '../../models/restaurant.model';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.css']
})
export class CheckoutComponent implements OnInit {
  cartItems: CartItem[] = [];
  checkoutForm: FormGroup;
  loading = false;
  processingPayment = false;
  paymentSuccess = false;
  paymentError: string | null = null;
  
  subtotal = 0;
  deliveryFee = 2.99;
  total = 0;

  constructor(
    private router: Router,
    private cartService: CartService,
    private paymentService: PaymentService,
    private orderService: OrderService,
    private fb: FormBuilder
  ) {
    this.checkoutForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', Validators.required],
      address: ['', Validators.required],
      city: ['', Validators.required],
      zipCode: ['', Validators.required],
      cardNumber: ['', [Validators.required, Validators.pattern(/^\d{16}$/)]],
      expiryMonth: ['', [Validators.required, Validators.pattern(/^(0[1-9]|1[0-2])$/)]],
      expiryYear: ['', [Validators.required, Validators.pattern(/^\d{4}$/)]],
      cvv: ['', [Validators.required, Validators.pattern(/^\d{3,4}$/)]]
    });
  }

  ngOnInit(): void {
    this.loadCart();
  }

  loadCart(): void {
    this.cartItems = this.cartService.getItems();
    this.calculateTotals();
    
    if (this.cartItems.length === 0) {
      this.router.navigate(['/']);
    }
  }

  calculateTotals(): void {
    this.subtotal = this.cartItems.reduce((sum, item) => sum + item.totalPrice, 0);
    this.total = this.subtotal + this.deliveryFee;
  }

  async processPayment(): Promise<void> {
    if (this.checkoutForm.invalid) {
      this.markFormGroupTouched();
      return;
    }

    this.processingPayment = true;
    this.paymentError = null;

    try {
      const paymentResult = await this.paymentService.processPayment({
        amount: this.total,
        currency: 'USD',
        cardNumber: this.checkoutForm.get('cardNumber')?.value,
        expiryMonth: this.checkoutForm.get('expiryMonth')?.value,
        expiryYear: this.checkoutForm.get('expiryYear')?.value,
        cvv: this.checkoutForm.get('cvv')?.value
      }).toPromise();

      if (paymentResult && paymentResult.success) {
        await this.createOrder();
      } else {
        this.paymentError = paymentResult?.message || 'Payment failed. Please try again.';
      }
    } catch (error) {
      this.paymentError = 'Payment processing failed. Please try again.';
      console.error('Payment error:', error);
    } finally {
      this.processingPayment = false;
    }
  }

  async createOrder(): Promise<void> {
    try {
      const orderItems: OrderItemDto[] = this.cartItems.map(item => ({
        menuItemId: parseInt(item.menuItemId),
        quantity: item.quantity,
        unitPrice: item.unitPrice
      }));

      const orderData: CreateOrderRequest = {
        userId: 'user-123', // In real app, this would come from auth service
        restaurantId: parseInt(this.cartService.getRestaurantId() || '1'),
        orderItems: orderItems,
        totalAmount: this.total
      };

      const order = await this.orderService.createOrder(orderData).toPromise();
      this.paymentSuccess = true;
      this.cartService.clearCart();
    } catch (error) {
      this.paymentError = 'Order creation failed. Please try again.';
      console.error('Order creation error:', error);
    }
  }

  markFormGroupTouched(): void {
    Object.keys(this.checkoutForm.controls).forEach(key => {
      const control = this.checkoutForm.get(key);
      control?.markAsTouched();
    });
  }

  goBack(): void {
    this.router.navigate(['/cart']);
  }

  goToHome(): void {
    this.router.navigate(['/']);
  }

  retryPayment(): void {
    this.paymentError = null;
    this.processPayment();
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