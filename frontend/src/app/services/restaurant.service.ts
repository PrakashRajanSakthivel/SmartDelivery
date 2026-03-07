import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { Restaurant } from '../models/restaurant.model';
import { MockDataService } from './mock-data.service';
import { getApiUrl, shouldUseMockData } from '../config/api.config';

@Injectable({
  providedIn: 'root'
})
export class RestaurantService {
  private apiUrl = getApiUrl('restaurant');
  private useMockData = shouldUseMockData();

  constructor(
    private http: HttpClient,
    private mockDataService: MockDataService
  ) { }

  getRestaurants(): Observable<Restaurant[]> {
    if (this.useMockData) {
      return this.mockDataService.getRestaurants();
    }
    return this.http.get<Restaurant[]>(this.apiUrl);
  }

  getRestaurantById(id: string): Observable<Restaurant | null> {
    if (this.useMockData) {
      return this.mockDataService.getRestaurantById(id).pipe(
        map(restaurant => restaurant || null)
      );
    }
    return this.http.get<Restaurant>(`${this.apiUrl}/${id}`);
  }

  getRestaurantDetails(id: string): Observable<Restaurant | null> {
    if (this.useMockData) {
      return this.mockDataService.getRestaurantDetails(id).pipe(
        map(restaurant => restaurant || null)
      );
    }
    return this.http.get<Restaurant>(`${this.apiUrl}/${id}/details`);
  }
} 