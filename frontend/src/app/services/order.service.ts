import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { MockDataService } from './mock-data.service';
import { getApiUrl, shouldUseMockData } from '../config/api.config';

export interface CreateOrderRequest {
  userId: string;
  restaurantId: string;
  items: OrderItemRequest[];
  notes?: string;
}

export interface OrderItemRequest {
  menuItemId: string;
  itemName: string;
  quantity: number;
  unitPrice: number;
}

export interface Order {
  id: string;
  userId: string;
  restaurantId: string;
  status: string;
  totalAmount: number;
  createdAt: string;
  updatedAt: string;
  items: OrderItem[];
}

export interface OrderItem {
  id: string;
  menuItemId: string;
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