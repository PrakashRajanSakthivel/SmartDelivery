import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  template: `
    <mat-toolbar color="primary" >
      <span [routerLink]="['/']" style="cursor: pointer;">🍕 SmartDelivery</span>
      <span class="spacer"></span>
      <button mat-icon-button [routerLink]="['/']">
        <mat-icon>home</mat-icon>
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
export class AppComponent {
  cartItemCount = 0; // This would be updated from a cart service
} 