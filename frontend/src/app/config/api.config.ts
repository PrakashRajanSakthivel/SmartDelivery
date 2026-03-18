import { environment } from '../../environments/environment';

export interface ApiConfig {
  useMockData: boolean;
  apiUrls: {
    restaurant: string;
    order: string;
    payment: string;
    auth: string;
  };
}

export const API_CONFIG: ApiConfig = {
  useMockData: environment.useMockData,
  apiUrls: environment.apiUrls
};

// Helper function to get API URL
export function getApiUrl(service: keyof ApiConfig['apiUrls']): string {
  return API_CONFIG.apiUrls[service];
}

// Helper function to check if mock data should be used
export function shouldUseMockData(): boolean {
  return API_CONFIG.useMockData;
}