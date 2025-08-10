# SmartDeliveryApp Testing Scripts

This directory contains comprehensive testing scripts for the SmartDeliveryApp microservices.

## 📋 Scripts Overview

### 1. `seed-test-data.ps1`
**Purpose**: Seeds test data in Restaurant and Order databases using connection strings from appsettings.

**Usage**:
```powershell
# Seed test data
.\scripts\seed-test-data.ps1

# Seed test data for specific environment
.\scripts\seed-test-data.ps1 -Environment Production

# Cleanup test data only
.\scripts\seed-test-data.ps1 -CleanupOnly
```

**Features**:
- ✅ Reads connection strings from `appsettings.{Environment}.json`
- ✅ Tests database connectivity before seeding
- ✅ Seeds comprehensive test data (restaurant, categories, menu items, orders)
- ✅ Cleans up existing test data before seeding
- ✅ Color-coded output for better visibility

### 2. `run-comprehensive-tests.ps1`
**Purpose**: Runs comprehensive HTTP tests against both Restaurant and Order services.

**Usage**:
```powershell
# Run tests with automatic service startup
.\scripts\run-comprehensive-tests.ps1

# Run tests against already running services
.\scripts\run-comprehensive-tests.ps1 -SkipServices

# Run tests without generating HTML report
.\scripts\run-comprehensive-tests.ps1 -GenerateReport:$false
```

**Features**:
- ✅ Automatically starts/stops services
- ✅ Health checks before testing
- ✅ Comprehensive test scenarios for both services
- ✅ Performance timing for each test
- ✅ HTML report generation
- ✅ Color-coded test results

### 3. `run-full-test-suite.ps1`
**Purpose**: Master orchestrator that runs the complete testing process.

**Usage**:
```powershell
# Run complete test suite
.\scripts\run-full-test-suite.ps1

# Run without cleanup (keep test data)
.\scripts\run-full-test-suite.ps1 -SkipCleanup

# Run against already running services
.\scripts\run-full-test-suite.ps1 -SkipServices
```

**Features**:
- ✅ Orchestrates the complete testing workflow
- ✅ Prerequisites checking
- ✅ Build verification
- ✅ Comprehensive reporting
- ✅ Error handling and recovery

## 🧪 Test Scenarios

### Restaurant Service Tests
1. **Get Restaurant Basic Info** - Validates basic restaurant retrieval
2. **Get Restaurant Validation Data** - Tests optimized validation endpoint
3. **Get Restaurant Menu** - Validates menu retrieval
4. **Get Restaurant Status** - Tests status endpoint
5. **Get Restaurant Operating Hours** - Validates operating hours
6. **Check Restaurant Active Status** - Tests active status endpoint
7. **Create Menu Item (Valid)** - Tests successful menu item creation
8. **Create Menu Item (Duplicate Name)** - Tests business rule validation
9. **Create Menu Item (Invalid Price)** - Tests price validation

### Order Service Tests
1. **Create Order (Valid)** - Tests successful order creation
2. **Create Order (Invalid Restaurant)** - Tests restaurant validation
3. **Create Order (Below Minimum)** - Tests minimum order amount validation
4. **Get Orders** - Validates order retrieval

## 📊 Test Data

### Restaurant Test Data
- **Restaurant ID**: `550e8400-e29b-41d4-a716-446655440001`
- **Name**: Pizza Palace
- **Status**: Active
- **Categories**: Pizzas, Salads
- **Menu Items**: Margherita Pizza, Pepperoni Pizza, Caesar Salad

### Order Test Data
- **User ID**: `550e8400-e29b-41d4-a716-446655440000`
- **Order ID**: `550e8400-e29b-41d4-a716-446655440100`
- **Items**: Margherita Pizza (2x), Caesar Salad (1x)

## 📁 Output Files

### Reports Directory
- **Location**: `reports/`
- **Format**: HTML reports with timestamps
- **Content**: Detailed test results with request/response data

### Test Results Directory
- **Location**: `test-results/`
- **Format**: JSON test results (if needed)

## 🚀 Quick Start

1. **Ensure services are built**:
   ```powershell
   dotnet build
   ```

2. **Run complete test suite**:
   ```powershell
   .\scripts\run-full-test-suite.ps1
   ```

3. **Review results**:
   - Check console output for summary
   - Open HTML report in `reports/` directory
   - Verify all tests passed

## 🔧 Configuration

### Environment Variables
- **Environment**: Defaults to "Development"
- **Service Ports**: 
  - Restaurant Service: `http://localhost:5001`
  - Order Service: `http://localhost:5000`

### Connection Strings
Scripts automatically read connection strings from:
- `src/services/RestaurantService/Restaurant.API/appsettings.{Environment}.json`
- `src/services/OrderService/OrderService.API/appsettings.{Environment}.json`

## 🐛 Troubleshooting

### Common Issues

1. **Services won't start**:
   - Check if ports 5000 and 5001 are available
   - Verify connection strings in appsettings
   - Ensure databases are accessible

2. **Tests fail**:
   - Check service logs for errors
   - Verify test data was seeded correctly
   - Ensure services are healthy before testing

3. **Database connection issues**:
   - Verify connection strings
   - Check database server is running
   - Ensure proper permissions

### Debug Mode
Run with verbose output:
```powershell
$VerbosePreference = "Continue"
.\scripts\run-full-test-suite.ps1
```

## 📈 Performance

### Expected Results
- **Total Duration**: 2-5 minutes (including service startup)
- **Test Count**: 13 comprehensive tests
- **Success Rate**: 100% when all services are healthy

### Optimization Tips
- Use `-SkipServices` if services are already running
- Use `-SkipCleanup` to preserve test data for debugging
- Run individual scripts for specific testing needs

## 🎯 Business Validation Testing

These scripts specifically test:

### Restaurant Service Business Rules
- ✅ Restaurant status transitions
- ✅ Menu item uniqueness within restaurant
- ✅ Menu item price validation
- ✅ Category deletion constraints
- ✅ Restaurant activation requirements

### Order Service Business Rules
- ✅ Restaurant availability validation
- ✅ Menu item availability validation
- ✅ Minimum order amount validation
- ✅ Price matching validation
- ✅ Order status transitions

### Inter-Service Communication
- ✅ Optimized single API call for validation
- ✅ Error handling and timeout management
- ✅ Data consistency across services
