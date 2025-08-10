#!/usr/bin/env python3
"""
SmartDeliveryApp - Test Data Seeding Script (Python Version)
============================================================
This script seeds test data for comprehensive testing
Usage: python scripts/seed_test_data.py
"""

import json
import os
import sys
import pyodbc
from pathlib import Path
from typing import Optional, Tuple

# Configuration
SCRIPT_DIR = Path(__file__).parent
PROJECT_ROOT = SCRIPT_DIR.parent
RESTAURANT_API_PATH = PROJECT_ROOT / "src" / "services" / "RestaurantService" / "Restaurant.API"
ORDER_API_PATH = PROJECT_ROOT / "src" / "services" / "OrderService" / "OrderService.API"

# Colors for output
class Colors:
    GREEN = '\033[92m'
    RED = '\033[91m'
    YELLOW = '\033[93m'
    CYAN = '\033[96m'
    WHITE = '\033[97m'
    RESET = '\033[0m'

def write_color_output(message: str, color: str = Colors.WHITE):
    """Write colored output to console"""
    print(f"{color}{message}{Colors.RESET}")

def convert_connection_string(dotnet_connection_string):
    """Convert .NET connection string to ODBC format"""
    # Parse the .NET connection string
    parts = {}
    for part in dotnet_connection_string.split(';'):
        if '=' in part:
            key, value = part.split('=', 1)
            parts[key.strip()] = value.strip()
    
    # Convert to ODBC format
    odbc_parts = []
    
    # Add driver
    odbc_parts.append("Driver={ODBC Driver 17 for SQL Server}")
    
    # Convert server - handle both Data Source and Server
    server = None
    if 'Data Source' in parts:
        server = parts['Data Source']
    elif 'Server' in parts:
        server = parts['Server']
    
    if server:
        if server == '.':
            odbc_parts.append("Server=.")
        else:
            odbc_parts.append(f"Server={server}")
    
    # Convert database
    if 'Initial Catalog' in parts:
        odbc_parts.append(f"Database={parts['Initial Catalog']}")
    elif 'Database' in parts:
        odbc_parts.append(f"Database={parts['Database']}")
    
    # Convert authentication
    if 'Integrated Security' in parts and parts['Integrated Security'].lower() == 'true':
        odbc_parts.append("Trusted_Connection=yes")
    elif 'Trusted_Connection' in parts and parts['Trusted_Connection'].lower() == 'true':
        odbc_parts.append("Trusted_Connection=yes")
    
    # Add trust server certificate for development
    odbc_parts.append("TrustServerCertificate=yes")
    
    return ';'.join(odbc_parts)

def test_connection_string(connection_string: str, database_name: str) -> bool:
    """Test database connection"""
    try:
        conn = pyodbc.connect(connection_string)
        conn.close()
        write_color_output(f"{database_name} connection successful", Colors.GREEN)
        return True
    except Exception as e:
        write_color_output(f"{database_name} connection failed: {str(e)}", Colors.RED)
        return False

def get_connection_string(service_path: Path, environment: str, service_type: str) -> Optional[str]:
    """Get connection string from appsettings file"""
    app_settings_path = service_path / f"appsettings.{environment}.json"
    if app_settings_path.exists():
        with open(app_settings_path, 'r') as f:
            app_settings = json.load(f)
            if service_type == "restaurant":
                return app_settings.get("ConnectionStrings", {}).get("RestaurantDatabase")
            elif service_type == "order":
                return app_settings.get("ConnectionStrings", {}).get("OrderDatabase")
    return None

def seed_restaurant_data(environment: str) -> bool:
    """Seed restaurant service data"""
    write_color_output("Seeding Restaurant Service Data...", Colors.CYAN)
    
    connection_string = get_connection_string(RESTAURANT_API_PATH, environment, "restaurant")
    if not connection_string:
        write_color_output("Could not find Restaurant Service connection string", Colors.RED)
        return False
    
    # Convert connection string first
    converted_connection_string = convert_connection_string(connection_string)
    
    if not test_connection_string(converted_connection_string, "Restaurant"):
        return False
    
    try:
        # SQL script for restaurant data
        restaurant_sql = """
DELETE FROM MenuItems WHERE RestaurantId = '550e8400-e29b-41d4-a716-446655440001';
DELETE FROM Categories WHERE RestaurantId = '550e8400-e29b-41d4-a716-446655440001';
DELETE FROM Restaurants WHERE Id = '550e8400-e29b-41d4-a716-446655440001';

INSERT INTO Restaurants (Id, Name, Description, Address, PhoneNumber, Email, IsActive, DeliveryFee, MinOrderAmount, AverageRating, OpeningHours, CuisineType, CreatedAt, UpdatedAt, CoverImageUrl, LogoUrl, EstimatedDeliveryTime)
VALUES (
    '550e8400-e29b-41d4-a716-446655440001',
    'Pizza Palace',
    'Best pizza in town',
    '123 Main St, City',
    '+1-555-0123',
    'info@pizzapalace.com',
    1,
    2.99,
    10.00,
    4.5,
    '09:00-22:00',
    'Italian',
    GETUTCDATE(),
    GETUTCDATE(),
    NULL,
    NULL,
    30
);

INSERT INTO Categories (Id, RestaurantId, Name, DisplayOrder)
VALUES 
    ('550e8400-e29b-41d4-a716-446655440010', '550e8400-e29b-41d4-a716-446655440001', 'Pizzas', 1),
    ('550e8400-e29b-41d4-a716-446655440011', '550e8400-e29b-41d4-a716-446655440001', 'Salads', 2);

INSERT INTO MenuItems (Id, RestaurantId, CategoryId, Name, Description, Price, IsAvailable, IsVegetarian, PreparationTime, CreatedAt)
VALUES 
    ('550e8400-e29b-41d4-a716-446655440002', '550e8400-e29b-41d4-a716-446655440001', '550e8400-e29b-41d4-a716-446655440010', 'Margherita Pizza', 'Classic tomato and mozzarella pizza', 15.99, 1, 1, 20, GETUTCDATE()),
    ('550e8400-e29b-41d4-a716-446655440003', '550e8400-e29b-41d4-a716-446655440001', '550e8400-e29b-41d4-a716-446655440010', 'Pepperoni Pizza', 'Spicy pepperoni with cheese', 17.99, 1, 0, 25, GETUTCDATE()),
    ('550e8400-e29b-41d4-a716-446655440004', '550e8400-e29b-41d4-a716-446655440001', '550e8400-e29b-41d4-a716-446655440011', 'Caesar Salad', 'Fresh romaine lettuce with Caesar dressing', 8.99, 1, 0, 10, GETUTCDATE());
"""
        
        conn = pyodbc.connect(converted_connection_string)
        cursor = conn.cursor()
        cursor.execute(restaurant_sql)
        conn.commit()
        conn.close()
        
        write_color_output("Restaurant data seeded successfully", Colors.GREEN)
        return True
    except Exception as e:
        write_color_output(f"Failed to seed restaurant data: {str(e)}", Colors.RED)
        return False

def seed_order_data(environment: str) -> bool:
    """Seed order service data"""
    write_color_output("Seeding Order Service Data...", Colors.CYAN)
    
    connection_string = get_connection_string(ORDER_API_PATH, environment, "order")
    if not connection_string:
        write_color_output("Could not find Order Service connection string", Colors.RED)
        return False
    
    # Convert connection string first
    converted_connection_string = convert_connection_string(connection_string)
    
    if not test_connection_string(converted_connection_string, "Order"):
        return False
    
    try:
        # SQL script for order data
        order_sql = """
DELETE FROM OrderItems WHERE OrderId IN (SELECT OrderId FROM Orders WHERE UserId = '550e8400-e29b-41d4-a716-446655440000');
DELETE FROM Orders WHERE UserId = '550e8400-e29b-41d4-a716-446655440000';

INSERT INTO Orders (OrderId, UserId, RestaurantId, Status, TotalAmount, CreatedAt, UpdatedAt, DeliveredAt, Notes)
VALUES (
    '550e8400-e29b-41d4-a716-446655440100',
    '550e8400-e29b-41d4-a716-446655440000',
    '550e8400-e29b-41d4-a716-446655440001',
    1,
    40.97,
    GETUTCDATE(),
    GETUTCDATE(),
    NULL,
    'Test order for validation'
);

INSERT INTO OrderItems (OrderItemId, OrderId, MenuItemId, ItemName, Quantity, UnitPrice, TotalPrice)
VALUES 
    ('550e8400-e29b-41d4-a716-446655440101', '550e8400-e29b-41d4-a716-446655440100', '550e8400-e29b-41d4-a716-446655440002', 'Margherita Pizza', 2, 15.99, 31.98),
    ('550e8400-e29b-41d4-a716-446655440102', '550e8400-e29b-41d4-a716-446655440100', '550e8400-e29b-41d4-a716-446655440004', 'Caesar Salad', 1, 8.99, 8.99);
"""
        
        conn = pyodbc.connect(converted_connection_string)
        cursor = conn.cursor()
        cursor.execute(order_sql)
        conn.commit()
        conn.close()
        
        write_color_output("Order data seeded successfully", Colors.GREEN)
        return True
    except Exception as e:
        write_color_output(f"Failed to seed order data: {str(e)}", Colors.RED)
        return False

def cleanup_test_data(environment: str):
    """Clean up test data"""
    write_color_output("Cleaning up test data...", Colors.YELLOW)
    
    # Cleanup Restaurant data
    restaurant_connection_string = get_connection_string(RESTAURANT_API_PATH, environment, "restaurant")
    if restaurant_connection_string:
        try:
            cleanup_restaurant_sql = """
DELETE FROM MenuItems WHERE RestaurantId = '550e8400-e29b-41d4-a716-446655440001';
DELETE FROM Categories WHERE RestaurantId = '550e8400-e29b-41d4-a716-446655440001';
DELETE FROM Restaurants WHERE Id = '550e8400-e29b-41d4-a716-446655440001';
"""
            restaurant_connection_string = convert_connection_string(restaurant_connection_string)
            conn = pyodbc.connect(restaurant_connection_string)
            cursor = conn.cursor()
            cursor.execute(cleanup_restaurant_sql)
            conn.commit()
            conn.close()
            write_color_output("Restaurant test data cleaned up", Colors.GREEN)
        except Exception as e:
            write_color_output(f"Failed to cleanup restaurant data: {str(e)}", Colors.RED)
    
    # Cleanup Order data
    order_connection_string = get_connection_string(ORDER_API_PATH, environment, "order")
    if order_connection_string:
        try:
            cleanup_order_sql = """
DELETE FROM OrderItems WHERE OrderId IN (SELECT OrderId FROM Orders WHERE UserId = '550e8400-e29b-41d4-a716-446655440000');
DELETE FROM Orders WHERE UserId = '550e8400-e29b-41d4-a716-446655440000';
"""
            order_connection_string = convert_connection_string(order_connection_string)
            conn = pyodbc.connect(order_connection_string)
            cursor = conn.cursor()
            cursor.execute(cleanup_order_sql)
            conn.commit()
            conn.close()
            write_color_output("Order test data cleaned up", Colors.GREEN)
        except Exception as e:
            write_color_output(f"Failed to cleanup order data: {str(e)}", Colors.RED)

def test_database_connections(environment: str):
    """Test database connections"""
    write_color_output("Testing database connections...", Colors.CYAN)
    
    # Test Restaurant database
    restaurant_connection_string = get_connection_string(RESTAURANT_API_PATH, environment, "restaurant")
    if restaurant_connection_string:
        write_color_output("Testing Restaurant database connection...", Colors.YELLOW)
        converted_connection = convert_connection_string(restaurant_connection_string)
        write_color_output(f"   Connection string: {converted_connection}", Colors.WHITE)
        
        if test_connection_string(converted_connection, "RestaurentServiceDb"):
            write_color_output("Restaurant database connection successful", Colors.GREEN)
        else:
            write_color_output("Restaurant database connection failed", Colors.RED)
    else:
        write_color_output("Could not get Restaurant connection string", Colors.RED)
    
    # Test Order database
    order_connection_string = get_connection_string(ORDER_API_PATH, environment, "order")
    if order_connection_string:
        write_color_output("Testing Order database connection...", Colors.YELLOW)
        converted_connection = convert_connection_string(order_connection_string)
        write_color_output(f"   Connection string: {converted_connection}", Colors.WHITE)
        
        if test_connection_string(converted_connection, "OrderServiceDb"):
            write_color_output("Order database connection successful", Colors.GREEN)
        else:
            write_color_output("Order database connection failed", Colors.RED)
    else:
        write_color_output("Could not get Order connection string", Colors.RED)

def main():
    """Main execution function"""
    import argparse
    
    parser = argparse.ArgumentParser(description='SmartDeliveryApp Test Data Seeding Script')
    parser.add_argument('--environment', '-e', default='Development', help='Environment (default: Development)')
    parser.add_argument('--cleanup-only', '-c', action='store_true', help='Only cleanup test data')
    parser.add_argument('--test-connections', '-t', action='store_true', help='Test database connections only')
    
    args = parser.parse_args()
    
    write_color_output("SmartDeliveryApp Test Data Seeding Script (Python)", Colors.CYAN)
    write_color_output(f"Environment: {args.environment}", Colors.YELLOW)
    
    if args.test_connections:
        test_database_connections(args.environment)
        return 0
    
    if args.cleanup_only:
        cleanup_test_data(args.environment)
        write_color_output("Cleanup completed", Colors.GREEN)
        return 0
    
    # Seed data
    restaurant_success = seed_restaurant_data(args.environment)
    order_success = seed_order_data(args.environment)
    
    if restaurant_success and order_success:
        write_color_output("All test data seeded successfully!", Colors.GREEN)
        write_color_output("Test Data Summary:", Colors.CYAN)
        write_color_output("   - Restaurant: Pizza Palace (ID: 550e8400-e29b-41d4-a716-446655440001)", Colors.WHITE)
        write_color_output("   - Categories: Pizzas, Salads", Colors.WHITE)
        write_color_output("   - Menu Items: Margherita Pizza, Pepperoni Pizza, Caesar Salad", Colors.WHITE)
        write_color_output("   - Test Order: Order for user 550e8400-e29b-41d4-a716-446655440000", Colors.WHITE)
        write_color_output("", Colors.WHITE)
        write_color_output("Ready for testing! Run the test script next.", Colors.GREEN)
        return 0
    else:
        write_color_output("Failed to seed some test data", Colors.RED)
        return 1

if __name__ == "__main__":
    sys.exit(main())
