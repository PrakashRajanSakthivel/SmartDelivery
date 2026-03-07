import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { MockDataService } from './mock-data.service';
import { getApiUrl, shouldUseMockData } from '../config/api.config';

export interface PaymentIntent {
  id: string;
  amount: number;
  currency: string;
  status: string;
}

export interface PaymentResult {
  succeeded: boolean;
  error: string | null;
}

@Injectable({
  providedIn: 'root'
})
export class PaymentService {
  private apiUrl = getApiUrl('payment');
  private useMockData = shouldUseMockData();

  constructor(
    private http: HttpClient,
    private mockDataService: MockDataService
  ) { }

  createPaymentIntent(amount: number, currency: string = 'usd'): Observable<PaymentIntent> {
    if (this.useMockData) {
      return this.mockDataService.createPaymentIntent(amount, currency);
    }
    return this.http.post<PaymentIntent>(`${this.apiUrl}/intents`, {
      amount,
      currency
    });
  }

  confirmPayment(paymentIntentId: string): Observable<PaymentResult> {
    if (this.useMockData) {
      return this.mockDataService.confirmPayment(paymentIntentId);
    }
    return this.http.post<PaymentResult>(`${this.apiUrl}/confirm`, {
      paymentIntentId
    });
  }
}