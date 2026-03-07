import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { AuthService } from './services/auth.service';
import { CartService } from './services/cart.service';

@Component({
  selector: 'app-root',
  template: `
    <mat-toolbar color="primary" >
      <span [routerLink]="['/']" style="cursor: pointer;">🍕 SmartDelivery</span>
      <span class="spacer"></span>
      <span *ngIf="isLoggedIn">Welcome, {{ username }}</span>
      <button mat-icon-button [routerLink]="['/cart']" *ngIf="isLoggedIn"
        [matBadge]="cartCount > 0 ? cartCount : null" matBadgeColor="accent" matBadgeSize="small">
        <mat-icon>shopping_cart</mat-icon>
      </button>
      <button mat-icon-button [routerLink]="['/']" *ngIf="isLoggedIn">
        <mat-icon>home</mat-icon>
      </button>
      <button mat-icon-button (click)="logout()" *ngIf="isLoggedIn">
        <mat-icon>logout</mat-icon>
      </button>
    </mat-toolbar>

    <div class="content">
      <router-outlet></router-outlet>
    </div>
  `,
  styles: [`
    .spacer {
      flex: 1 1 auto;
    }
    
    .content {
      padding: 20px;
      max-width: 1200px;
      margin: 0 auto;
    }
    
    .cart-badge {
      background: #ff4444;
      color: white;
      border-radius: 50%;
      padding: 2px 6px;
      font-size: 12px;
      position: absolute;
      top: 8px;
      right: 8px;
    }
  `]
})
export class AppComponent implements OnInit, OnDestroy {
  cartItemCount = 0;
  cartCount = 0;
  isLoggedIn = false;
  username = '';
  private authStatusSubscription?: Subscription;
  private cartSubscription?: Subscription;

  constructor(private authService: AuthService, private router: Router, private cartService: CartService) {}

  ngOnInit() {
    this.authStatusSubscription = this.authService.authStatus$.subscribe(isLoggedIn => {
      this.isLoggedIn = isLoggedIn;
      if (isLoggedIn) {
        const user = this.authService.getUser();
        this.username = user ? user.username : '';
      } else {
        this.username = '';
      }
    });
    this.cartSubscription = this.cartService.cart$.subscribe(items => {
      this.cartCount = items.reduce((sum, item) => sum + item.quantity, 0);
    });
  }

  ngOnDestroy() {
    this.authStatusSubscription?.unsubscribe();
    this.cartSubscription?.unsubscribe();
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}