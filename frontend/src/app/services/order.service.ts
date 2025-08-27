import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { MockDataService } from './mock-data.service';
import { getApiUrl, shouldUseMockData } from '../config/api.config';

export interface CreateOrderRequest {
  userId: string;
  restaurantId: number;
  orderItems: OrderItemDto[];
  totalAmount: number;
}

export interface OrderItemDto {
  menuItemId: number;
  quantity: number;
  unitPrice: number;
}

export interface Order {
  orderId: string;
  userId: string;
  restaurantId: number;
  status: string;
  totalAmount: number;
  createdAt: string;
  updatedAt: string;
  orderItems: OrderItem[];
}

export interface OrderItem {
  id: number;
  menuItemId: number;
  quantity: number;
  unitPrice: number;
  totalPrice: number;
}

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  private apiUrl = getApiUrl('order');
  private useMockData = shouldUseMockData();

  constructor(
    private http: HttpClient,
    private mockDataService: MockDataService
  ) { }

  createOrder(orderData: CreateOrderRequest): Observable<Order> {
    if (this.useMockData) {
      return this.mockDataService.createOrder(orderData);
    }
    return this.http.post<Order>(`${this.apiUrl}`, orderData);
  }

  getOrderById(orderId: string): Observable<Order> {
    if (this.useMockData) {
      return this.mockDataService.getOrderById(orderId);
    }
    return this.http.get<Order>(`${this.apiUrl}/${orderId}`);
  }

  updateOrderStatus(orderId: string, status: string): Observable<Order> {
    if (this.useMockData) {
      // For mock, just return the existing order with updated status
      return this.mockDataService.getOrderById(orderId);
    }
    return this.http.put<Order>(`${this.apiUrl}/${orderId}/status`, { status });
  }
} 