#!/usr/bin/env python3
"""
SQL Server Basic Connectivity Test
==================================
This script tests if SQL Server is running and accessible
"""

import pyodbc

def test_sql_server():
    """Test basic SQL Server connectivity"""
    print("🔍 Testing SQL Server Connectivity...")
    
    # Try different connection strings with correct instance name
    connection_strings = [
        "Driver={ODBC Driver 17 for SQL Server};Server=localhost\\MSSQLSERVER;Database=master;Trusted_Connection=yes;TrustServerCertificate=yes;",
        "Driver={ODBC Driver 17 for SQL Server};Server=.;Database=master;Trusted_Connection=yes;TrustServerCertificate=yes;",
        "Driver={ODBC Driver 17 for SQL Server};Server=(local);Database=master;Trusted_Connection=yes;TrustServerCertificate=yes;",
        "Driver={ODBC Driver 17 for SQL Server};Server=localhost;Database=master;Trusted_Connection=yes;TrustServerCertificate=yes;",
        "Server=localhost;Database=master;Trusted_Connection=yes;TrustServerCertificate=yes;"
    ]
    
    for i, conn_str in enumerate(connection_strings, 1):
        print(f"\n📡 Attempt {i}: Testing connection...")
        print(f"   Connection string: {conn_str}")
        
        try:
            conn = pyodbc.connect(conn_str)
            print("✅ Connection successful!")
            
            # Test a simple query
            cursor = conn.cursor()
            cursor.execute("SELECT @@VERSION")
            version = cursor.fetchone()[0]
            print(f"📋 SQL Server Version: {version[:100]}...")
            
            # Check if our databases exist
            cursor.execute("SELECT name FROM sys.databases WHERE name IN ('OrderServiceDb', 'RestaurentServiceDb')")
            databases = [row[0] for row in cursor.fetchall()]
            print(f"📊 Found databases: {databases}")
            
            conn.close()
            return True
            
        except Exception as e:
            print(f"❌ Connection failed: {str(e)}")
            continue
    
    print("\n❌ All connection attempts failed!")
    return False

def main():
    """Main execution function"""
    print("🚀 SQL Server Basic Connectivity Test")
    print("=" * 40)
    
    success = test_sql_server()
    
    if success:
        print("\n🎯 SQL Server is accessible! We can proceed.")
        return 0
    else:
        print("\n⚠️ SQL Server is not accessible. Please check:")
        print("   1. SQL Server is installed and running")
        print("   2. SQL Server Browser service is running")
        print("   3. Windows Authentication is enabled")
        print("   4. ODBC drivers are installed")
        return 1

if __name__ == "__main__":
    import sys
    sys.exit(main())

