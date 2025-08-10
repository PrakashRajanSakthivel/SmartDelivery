#!/usr/bin/env python3
"""
Simple Order Database Test Script
================================
This script tests basic connectivity to the Order database
"""

import pyodbc
import requests
import json

def test_order_database():
    """Test Order database connection and make a simple query"""
    print("🔍 Testing Order Database Connection...")
    
    # Hardcoded connection string for Order database - using the working format
    connection_string = "Driver={ODBC Driver 17 for SQL Server};Server=.;Database=OrderServiceDb;Trusted_Connection=yes;TrustServerCertificate=yes;"
    
    try:
        # Connect to database
        print("📡 Connecting to OrderServiceDb...")
        conn = pyodbc.connect(connection_string)
        print("✅ Database connection successful!")
        
        # Make a simple query
        cursor = conn.cursor()
        cursor.execute("SELECT COUNT(*) as OrderCount FROM Orders")
        result = cursor.fetchone()
        order_count = result[0] if result else 0
        print(f"📊 Found {order_count} orders in the database")
        
        # Get a sample order if any exist
        if order_count > 0:
            cursor.execute("SELECT TOP 1 OrderId, UserId, Status, TotalAmount FROM Orders")
            sample_order = cursor.fetchone()
            if sample_order:
                print(f"📋 Sample Order: ID={sample_order[0]}, User={sample_order[1]}, Status={sample_order[2]}, Amount=${sample_order[3]}")
        
        conn.close()
        print("✅ Database test completed successfully!")
        return True
        
    except Exception as e:
        print(f"❌ Database connection failed: {str(e)}")
        return False

def test_order_api():
    """Test Order API connectivity"""
    print("\n🌐 Testing Order API...")
    
    try:
        # Test the Order API
        response = requests.get("http://localhost:5002/api/orders", timeout=5)
        print(f"✅ Order API response: {response.status_code}")
        
        if response.status_code == 200:
            orders = response.json()
            print(f"📊 API returned {len(orders)} orders")
        else:
            print(f"⚠️ API returned status code: {response.status_code}")
            
        return True
        
    except requests.exceptions.ConnectionError:
        print("❌ Order API is not running or not accessible")
        return False
    except Exception as e:
        print(f"❌ Order API test failed: {str(e)}")
        return False

def main():
    """Main execution function"""
    print("🚀 Simple Order Database & API Test")
    print("=" * 40)
    
    # Test database
    db_success = test_order_database()
    
    # Test API
    api_success = test_order_api()
    
    # Summary
    print("\n📋 Test Summary:")
    print(f"   Database: {'✅ PASSED' if db_success else '❌ FAILED'}")
    print(f"   API: {'✅ PASSED' if api_success else '❌ FAILED'}")
    
    if db_success and api_success:
        print("\n🎯 All tests passed! Ready to proceed with seeding.")
        return 0
    else:
        print("\n⚠️ Some tests failed. Please check the issues above.")
        return 1

if __name__ == "__main__":
    import sys
    sys.exit(main())

