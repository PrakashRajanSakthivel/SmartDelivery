export interface ApiConfig {
  useMockData: boolean;
  apiUrls: {
    restaurant: string;
    order: string;
    payment: string;
  };
}

export const API_CONFIG: ApiConfig = {
  useMockData: true, // Set to false to use real APIs
  apiUrls: {
    restaurant: 'http://localhost:5001/api/restaurants',
    order: 'http://localhost:5002/api/orders',
    payment: 'http://localhost:5003/api/payments'
  }
};

// Helper function to get API URL
export function getApiUrl(service: keyof ApiConfig['apiUrls']): string {
  return API_CONFIG.apiUrls[service];
}

// Helper function to check if mock data should be used
export function shouldUseMockData(): boolean {
  return API_CONFIG.useMockData;
} 