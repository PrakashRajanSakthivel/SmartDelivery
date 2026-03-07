import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { RestaurantService } from '../../services/restaurant.service';
import { Restaurant } from '../../models/restaurant.model';

@Component({
  selector: 'app-restaurant-list',
  templateUrl: './restaurant-list.component.html',
  styleUrls: ['./restaurant-list.component.css']
})
export class RestaurantListComponent implements OnInit {
  restaurants: Restaurant[] = [];
  loading = false;
  error: string | null = null;

  constructor(
    private restaurantService: RestaurantService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.loadRestaurants();
  }

  loadRestaurants(): void {
    this.loading = true;
    this.error = null;
    
    this.restaurantService.getRestaurants().subscribe({
      next: (restaurants) => {
        this.restaurants = restaurants;
        this.loading = false;
      },
      error: (error) => {
        this.error = 'Failed to load restaurants. Please try again.';
        this.loading = false;
        console.error('Error loading restaurants:', error);
      }
    });
  }

  selectRestaurant(restaurantId: string): void {
    this.router.navigate(['/restaurant', restaurantId]);
  }

  getRestaurantIcon(restaurantName: string): string {
    const name = restaurantName.toLowerCase();
    if (name.includes('pizza')) return 'local_pizza';
    if (name.includes('burger')) return 'fastfood';
    if (name.includes('sushi')) return 'set_meal';
    if (name.includes('coffee') || name.includes('cafe')) return 'local_cafe';
    if (name.includes('ice cream') || name.includes('dessert')) return 'icecream';
    if (name.includes('chinese') || name.includes('asian')) return 'ramen_dining';
    if (name.includes('mexican') || name.includes('taco')) return 'restaurant';
    if (name.includes('italian')) return 'local_pizza';
    if (name.includes('indian')) return 'restaurant_menu';
    return 'restaurant'; // default icon
  }
} 