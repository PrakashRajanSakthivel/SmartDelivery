#!/usr/bin/env python3
"""
Simple SQL Server connection test
"""

import pyodbc

def test_sql_server():
    """Test basic SQL Server connectivity"""
    try:
        # Try to connect to master database
        conn_str = "Driver={ODBC Driver 17 for SQL Server};Server=localhost;Database=master;Trusted_Connection=true;"
        conn = pyodbc.connect(conn_str)
        print("✅ SQL Server connection successful")
        
        # Check if databases exist
        cursor = conn.cursor()
        cursor.execute("SELECT name FROM sys.databases WHERE name IN ('RestaurentServiceDb', 'OrderServiceDb')")
        databases = [row[0] for row in cursor.fetchall()]
        
        print(f"Found databases: {databases}")
        
        if 'RestaurentServiceDb' not in databases:
            print("❌ RestaurentServiceDb not found")
        else:
            print("✅ RestaurentServiceDb exists")
            
        if 'OrderServiceDb' not in databases:
            print("❌ OrderServiceDb not found")
        else:
            print("✅ OrderServiceDb exists")
        
        conn.close()
        
    except Exception as e:
        print(f"❌ SQL Server connection failed: {str(e)}")

if __name__ == "__main__":
    test_sql_server()

