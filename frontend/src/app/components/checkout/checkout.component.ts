import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CartService } from '../../services/cart.service';
import { PaymentService } from '../../services/payment.service';
import { OrderService, CreateOrderRequest, OrderItemRequest } from '../../services/order.service';
import { AuthService } from '../../services/auth.service';
import { CartItem } from '../../models/cart-item.model';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.css']
})
export class CheckoutComponent implements OnInit, OnDestroy {
  cartItems: CartItem[] = [];
  checkoutForm: FormGroup;
  loading = false;
  processingPayment = false;
  paymentSuccess = false;
  paymentError: string | null = null;
  confirmedOrderId = '';
  orderStatusIndex = 0;
  readonly statusSteps = [
    { label: 'Order Received',    icon: 'receipt' },
    { label: 'Confirmed',         icon: 'check_circle' },
    { label: 'Preparing',         icon: 'restaurant' },
    { label: 'Out for Delivery',  icon: 'delivery_dining' },
    { label: 'Delivered',         icon: 'home' }
  ];
  private stepTimers: ReturnType<typeof setTimeout>[] = [];
  
  subtotal = 0;
  deliveryFee = 2.99;
  total = 0;

  constructor(
    private router: Router,
    private cartService: CartService,
    private paymentService: PaymentService,
    private orderService: OrderService,
    private authService: AuthService,
    private fb: FormBuilder
  ) {
    this.checkoutForm = this.fb.group({
      firstName: ['John', Validators.required],
      lastName: ['Doe', Validators.required],
      email: ['john.doe@example.com', [Validators.required, Validators.email]],
      phone: ['5551234567', Validators.required],
      address: ['123 Main Street', Validators.required],
      city: ['San Francisco', Validators.required],
      zipCode: ['94102', Validators.required],
      cardNumber: ['4242424242424242', [Validators.required, Validators.pattern(/^\d{16}$/)]],
      expiryMonth: ['12', [Validators.required, Validators.pattern(/^(0[1-9]|1[0-2])$/)]],
      expiryYear: ['2028', [Validators.required, Validators.pattern(/^\d{4}$/)]],
      cvv: ['123', [Validators.required, Validators.pattern(/^\d{3,4}$/)]]
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
      const userId = this.authService.getUser()?.userId;
      if (!userId) {
        this.paymentError = 'User session expired. Please log in again.';
        return;
      }

      // Step 1: Create order
      const orderItems: OrderItemRequest[] = this.cartItems.map(item => ({
        menuItemId: item.menuItemId,
        itemName: item.menuItemName,
        quantity: item.quantity,
        unitPrice: item.unitPrice
      }));

      const orderData: CreateOrderRequest = {
        userId,
        restaurantId: this.cartService.getRestaurantId() || '',
        items: orderItems
      };

      await this.orderService.createOrder(orderData).toPromise().then(order => {
        this.confirmedOrderId = order?.id || '';
      });

      // Step 2: Create payment intent
      const intent = await this.paymentService.createPaymentIntent(this.total, 'usd').toPromise();
      if (!intent) {
        this.paymentError = 'Failed to initiate payment.';
        return;
      }

      // Step 3: Confirm payment
      const result = await this.paymentService.confirmPayment(intent.id).toPromise();
      if (result?.succeeded) {
        this.paymentSuccess = true;
        this.cartService.clearCart();
        this.startOrderStatusProgress();
      } else {
        this.paymentError = result?.error || 'Payment failed. Please try again.';
      }
    } catch (error) {
      this.paymentError = 'Checkout failed. Please try again.';
      console.error('Checkout error:', error);
    } finally {
      this.processingPayment = false;
    }
  }

  markFormGroupTouched(): void {
    Object.keys(this.checkoutForm.controls).forEach(key => {
      const control = this.checkoutForm.get(key);
      control?.markAsTouched();
    });
  }

  startOrderStatusProgress(): void {
    [2000, 5000, 9000, 14000].forEach((delay, i) => {
      this.stepTimers.push(setTimeout(() => { this.orderStatusIndex = i + 1; }, delay));
    });
  }

  ngOnDestroy(): void {
    this.stepTimers.forEach(t => clearTimeout(t));
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