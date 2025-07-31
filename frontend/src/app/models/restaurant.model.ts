export interface Restaurant {
  id: string;
  name: string;
  description: string;
  address?: string;
  phoneNumber?: string;
  deliveryFee: number;
  minOrderAmount: number;
  isActive: boolean;
  averageRating: number;
  coverImageUrl?: string;
  logoUrl?: string;
  estimatedDeliveryTime: number;
  categories: Category[];
}

export interface Category {
  id: string;
  name: string;
  displayOrder: number;
  menuItems: MenuItem[];
}

export interface MenuItem {
  id: string;
  name: string;
  description: string;
  price: number;
  imageUrl?: string;
  isVegetarian: boolean;
  isVegan: boolean;
  isAvailable: boolean;
  categoryId: string;
}

export interface CartItem {
  id: string;
  menuItemId: string;
  menuItemName: string;
  quantity: number;
  unitPrice: number;
  totalPrice: number;
  imageUrl?: string;
}

export interface Cart {
  id: string;
  userId: string;
  restaurantId: string;
  items: CartItem[];
  totalAmount: number;
  totalItems: number;
} 