# ========================================
# SmartDeliveryApp - Simple Test Data Seeding Script
# ========================================
# This script seeds test data for comprehensive testing
# Usage: .\scripts\seed-test-data-simple.ps1

param(
    [string]$Environment = "Development",
    [switch]$CleanupOnly = $false
)

# Configuration
$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$ProjectRoot = Split-Path -Parent $ScriptDir
$RestaurantApiPath = Join-Path $ProjectRoot "src\services\RestaurantService\Restaurant.API"
$OrderApiPath = Join-Path $ProjectRoot "src\services\OrderService\OrderService.API"

# Colors for output
$Green = "Green"
$Red = "Red"
$Yellow = "Yellow"
$Cyan = "Cyan"

function Write-ColorOutput {
    param([string]$Message, [string]$Color = "White")
    Write-Host $Message -ForegroundColor $Color
}

function Get-ConnectionString {
    param([string]$ServicePath, [string]$Environment)
    
    $appSettingsPath = Join-Path $ServicePath "appsettings.$Environment.json"
    if (Test-Path $appSettingsPath) {
        $appSettings = Get-Content $appSettingsPath | ConvertFrom-Json
        return $appSettings.ConnectionStrings.DefaultConnection
    }
    return $null
}

function Test-ConnectionString {
    param([string]$ConnectionString, [string]$DatabaseName)
    
    try {
        $builder = New-Object Microsoft.Data.SqlClient.SqlConnectionStringBuilder $ConnectionString
        $connection = New-Object Microsoft.Data.SqlClient.SqlConnection $builder.ConnectionString
        $connection.Open()
        $connection.Close()
        Write-ColorOutput "✅ $DatabaseName connection successful" $Green
        return $true
    }
    catch {
        Write-ColorOutput "❌ $DatabaseName connection failed: $($_.Exception.Message)" $Red
        return $false
    }
}

function Seed-RestaurantData {
    Write-ColorOutput "🍕 Seeding Restaurant Service Data..." $Cyan
    
    $connectionString = Get-ConnectionString $RestaurantApiPath $Environment
    if (-not $connectionString) {
        Write-ColorOutput "❌ Could not find Restaurant Service connection string" $Red
        return $false
    }
    
    if (-not (Test-ConnectionString $connectionString "Restaurant")) {
        return $false
    }
    
    try {
        # Create SQL script for restaurant data
        $restaurantSql = @'
/* Clear existing test data */
DELETE FROM MenuItems WHERE RestaurantId = '550e8400-e29b-41d4-a716-446655440001';
DELETE FROM Categories WHERE RestaurantId = '550e8400-e29b-41d4-a716-446655440001';
DELETE FROM Restaurants WHERE Id = '550e8400-e29b-41d4-a716-446655440001';

/* Insert test restaurant */
INSERT INTO Restaurants (Id, Name, Description, Address, PhoneNumber, Email, IsActive, Status, DeliveryFee, MinOrderAmount, AverageRating, OpeningHours, CuisineType, CreatedAt, UpdatedAt)
VALUES (
    '550e8400-e29b-41d4-a716-446655440001',
    'Pizza Palace',
    'Best pizza in town',
    '123 Main St, City',
    '+1-555-0123',
    'info@pizzapalace.com',
    1,
    1,
    2.99,
    10.00,
    4.5,
    '09:00-22:00',
    'Italian',
    GETUTCDATE(),
    GETUTCDATE()
);

/* Insert test categories */
INSERT INTO Categories (Id, RestaurantId, Name, DisplayOrder, CreatedAt)
VALUES 
    ('550e8400-e29b-41d4-a716-446655440010', '550e8400-e29b-41d4-a716-446655440001', 'Pizzas', 1, GETUTCDATE()),
    ('550e8400-e29b-41d4-a716-446655440011', '550e8400-e29b-41d4-a716-446655440001', 'Salads', 2, GETUTCDATE());

/* Insert test menu items */
INSERT INTO MenuItems (Id, RestaurantId, CategoryId, Name, Description, Price, IsAvailable, IsVegetarian, PreparationTime, CreatedAt)
VALUES 
    ('550e8400-e29b-41d4-a716-446655440002', '550e8400-e29b-41d4-a716-446655440001', '550e8400-e29b-41d4-a716-446655440010', 'Margherita Pizza', 'Classic tomato and mozzarella pizza', 15.99, 1, 1, 20, GETUTCDATE()),
    ('550e8400-e29b-41d4-a716-446655440003', '550e8400-e29b-41d4-a716-446655440001', '550e8400-e29b-41d4-a716-446655440010', 'Pepperoni Pizza', 'Spicy pepperoni with cheese', 17.99, 1, 0, 25, GETUTCDATE()),
    ('550e8400-e29b-41d4-a716-446655440004', '550e8400-e29b-41d4-a716-446655440001', '550e8400-e29b-41d4-a716-446655440011', 'Caesar Salad', 'Fresh romaine lettuce with Caesar dressing', 8.99, 1, 0, 10, GETUTCDATE());
'@

        $connection = New-Object Microsoft.Data.SqlClient.SqlConnection $connectionString
        $command = New-Object Microsoft.Data.SqlClient.SqlCommand $restaurantSql, $connection
        
        $connection.Open()
        $command.ExecuteNonQuery() | Out-Null
        $connection.Close()
        
        Write-ColorOutput "✅ Restaurant data seeded successfully" $Green
        return $true
    }
    catch {
        Write-ColorOutput "❌ Failed to seed restaurant data: $($_.Exception.Message)" $Red
        return $false
    }
}

function Seed-OrderData {
    Write-ColorOutput "📦 Seeding Order Service Data..." $Cyan
    
    $connectionString = Get-ConnectionString $OrderApiPath $Environment
    if (-not $connectionString) {
        Write-ColorOutput "❌ Could not find Order Service connection string" $Red
        return $false
    }
    
    if (-not (Test-ConnectionString $connectionString "Order")) {
        return $false
    }
    
    try {
        # Create SQL script for order data
        $orderSql = @'
/* Clear existing test data */
DELETE FROM OrderItems WHERE OrderId IN (SELECT OrderId FROM Orders WHERE UserId = '550e8400-e29b-41d4-a716-446655440000');
DELETE FROM Orders WHERE UserId = '550e8400-e29b-41d4-a716-446655440000';

/* Insert test order */
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

/* Insert test order items */
INSERT INTO OrderItems (OrderItemId, OrderId, MenuItemId, ItemName, Quantity, UnitPrice, TotalPrice)
VALUES 
    ('550e8400-e29b-41d4-a716-446655440101', '550e8400-e29b-41d4-a716-446655440100', '550e8400-e29b-41d4-a716-446655440002', 'Margherita Pizza', 2, 15.99, 31.98),
    ('550e8400-e29b-41d4-a716-446655440102', '550e8400-e29b-41d4-a716-446655440100', '550e8400-e29b-41d4-a716-446655440004', 'Caesar Salad', 1, 8.99, 8.99);
'@

        $connection = New-Object Microsoft.Data.SqlClient.SqlConnection $connectionString
        $command = New-Object Microsoft.Data.SqlClient.SqlCommand $orderSql, $connection
        
        $connection.Open()
        $command.ExecuteNonQuery() | Out-Null
        $connection.Close()
        
        Write-ColorOutput "✅ Order data seeded successfully" $Green
        return $true
    }
    catch {
        Write-ColorOutput "❌ Failed to seed order data: $($_.Exception.Message)" $Red
        return $false
    }
}

function Cleanup-TestData {
    Write-ColorOutput "🧹 Cleaning up test data..." $Yellow
    
    # Cleanup Restaurant data
    $restaurantConnectionString = Get-ConnectionString $RestaurantApiPath $Environment
    if ($restaurantConnectionString) {
        try {
            $cleanupRestaurantSql = @'
/* Cleanup restaurant test data */
DELETE FROM MenuItems WHERE RestaurantId = '550e8400-e29b-41d4-a716-446655440001';
DELETE FROM Categories WHERE RestaurantId = '550e8400-e29b-41d4-a716-446655440001';
DELETE FROM Restaurants WHERE Id = '550e8400-e29b-41d4-a716-446655440001';
'@
            $connection = New-Object Microsoft.Data.SqlClient.SqlConnection $restaurantConnectionString
            $command = New-Object Microsoft.Data.SqlClient.SqlCommand $cleanupRestaurantSql, $connection
            $connection.Open()
            $command.ExecuteNonQuery() | Out-Null
            $connection.Close()
            Write-ColorOutput "✅ Restaurant test data cleaned up" $Green
        }
        catch {
            Write-ColorOutput "❌ Failed to cleanup restaurant data: $($_.Exception.Message)" $Red
        }
    }
    
    # Cleanup Order data
    $orderConnectionString = Get-ConnectionString $OrderApiPath $Environment
    if ($orderConnectionString) {
        try {
            $cleanupOrderSql = @'
/* Cleanup order test data */
DELETE FROM OrderItems WHERE OrderId IN (SELECT OrderId FROM Orders WHERE UserId = '550e8400-e29b-41d4-a716-446655440000');
DELETE FROM Orders WHERE UserId = '550e8400-e29b-41d4-a716-446655440000';
'@
            $connection = New-Object Microsoft.Data.SqlClient.SqlConnection $orderConnectionString
            $command = New-Object Microsoft.Data.SqlClient.SqlCommand $cleanupOrderSql, $connection
            $connection.Open()
            $command.ExecuteNonQuery() | Out-Null
            $connection.Close()
            Write-ColorOutput "✅ Order test data cleaned up" $Green
        }
        catch {
            Write-ColorOutput "❌ Failed to cleanup order data: $($_.Exception.Message)" $Red
        }
    }
}

# Main execution
Write-ColorOutput "🚀 SmartDeliveryApp Test Data Seeding Script" $Cyan
Write-ColorOutput "Environment: $Environment" $Yellow

if ($CleanupOnly) {
    Cleanup-TestData
    Write-ColorOutput "✅ Cleanup completed" $Green
    exit 0
}

# Seed data
$restaurantSuccess = Seed-RestaurantData
$orderSuccess = Seed-OrderData

if ($restaurantSuccess -and $orderSuccess) {
    Write-ColorOutput "✅ All test data seeded successfully!" $Green
    Write-ColorOutput "📋 Test Data Summary:" $Cyan
    Write-ColorOutput "   - Restaurant: Pizza Palace (ID: 550e8400-e29b-41d4-a716-446655440001)" $White
    Write-ColorOutput "   - Categories: Pizzas, Salads" $White
    Write-ColorOutput "   - Menu Items: Margherita Pizza, Pepperoni Pizza, Caesar Salad" $White
    Write-ColorOutput "   - Test Order: Order for user 550e8400-e29b-41d4-a716-446655440000" $White
    Write-ColorOutput "" $White
    Write-ColorOutput "🎯 Ready for testing! Run the test script next." $Green
} else {
    Write-ColorOutput "❌ Failed to seed some test data" $Red
    exit 1
}
