# ========================================
# SmartDeliveryApp - Comprehensive Test Runner
# ========================================
# This script runs all HTTP tests and validates results
# Usage: .\scripts\run-comprehensive-tests.ps1

param(
    [string]$Environment = "Development",
    [switch]$SkipServices = $false,
    [switch]$GenerateReport = $true
)

# Configuration
$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$ProjectRoot = Split-Path -Parent $ScriptDir
$TestResultsDir = Join-Path $ProjectRoot "test-results"
$ReportsDir = Join-Path $ProjectRoot "reports"

# Create directories if they don't exist
if (-not (Test-Path $TestResultsDir)) { New-Item -ItemType Directory -Path $TestResultsDir | Out-Null }
if (-not (Test-Path $ReportsDir)) { New-Item -ItemType Directory -Path $ReportsDir | Out-Null }

# Colors for output
$Green = "Green"
$Red = "Red"
$Yellow = "Yellow"
$Cyan = "Cyan"
$White = "White"

function Write-ColorOutput {
    param([string]$Message, [string]$Color = "White")
    Write-Host $Message -ForegroundColor $Color
}

function Start-Services {
    Write-ColorOutput "🚀 Starting Services..." $Cyan
    
    # Start Restaurant Service
    Write-ColorOutput "🍕 Starting Restaurant Service..." $Yellow
    $restaurantProcess = Start-Process -FilePath "dotnet" -ArgumentList "run", "--project", "src\services\RestaurantService\Restaurant.API" -PassThru -WindowStyle Hidden
    
    # Start Order Service
    Write-ColorOutput "📦 Starting Order Service..." $Yellow
    $orderProcess = Start-Process -FilePath "dotnet" -ArgumentList "run", "--project", "src\services\OrderService\OrderService.API" -PassThru -WindowStyle Hidden
    
    # Wait for services to start
    Write-ColorOutput "⏳ Waiting for services to start..." $Yellow
    Start-Sleep -Seconds 10
    
    return @{
        Restaurant = $restaurantProcess
        Order = $orderProcess
    }
}

function Stop-Services {
    param($Processes)
    
    Write-ColorOutput "🛑 Stopping Services..." $Yellow
    
    if ($Processes.Restaurant -and -not $Processes.Restaurant.HasExited) {
        Stop-Process -Id $Processes.Restaurant.Id -Force
        Write-ColorOutput "✅ Restaurant Service stopped" $Green
    }
    
    if ($Processes.Order -and -not $Processes.Order.HasExited) {
        Stop-Process -Id $Processes.Order.Id -Force
        Write-ColorOutput "✅ Order Service stopped" $Green
    }
}

function Test-ServiceHealth {
    param([string]$ServiceName, [string]$BaseUrl)
    
    try {
        $response = Invoke-RestMethod -Uri "$BaseUrl/health" -Method GET -TimeoutSec 10
        Write-ColorOutput "✅ $ServiceName is healthy" $Green
        return $true
    }
    catch {
        Write-ColorOutput "❌ $ServiceName health check failed: $($_.Exception.Message)" $Red
        return $false
    }
}

function Invoke-HTTPTest {
    param([string]$TestName, [string]$Method, [string]$Url, [string]$Body = $null, [hashtable]$Headers = @{})
    
    $testResult = @{
        Name = $TestName
        Method = $Method
        Url = $Url
        StatusCode = $null
        Response = $null
        Success = $false
        Error = $null
        Duration = $null
    }
    
    try {
        $stopwatch = [System.Diagnostics.Stopwatch]::StartNew()
        
        $params = @{
            Uri = $Url
            Method = $Method
            TimeoutSec = 30
        }
        
        if ($Headers.Count -gt 0) {
            $params.Headers = $Headers
        }
        
        if ($Body) {
            $params.Body = $Body
            $params.ContentType = "application/json"
        }
        
        $response = Invoke-RestMethod @params
        
        $stopwatch.Stop()
        $testResult.Duration = $stopwatch.ElapsedMilliseconds
        $testResult.Response = $response
        $testResult.Success = $true
        
        Write-ColorOutput "✅ $TestName - ${Duration}ms" $Green
    }
    catch {
        $stopwatch.Stop()
        $testResult.Duration = $stopwatch.ElapsedMilliseconds
        $testResult.Error = $_.Exception.Message
        $testResult.StatusCode = $_.Exception.Response.StatusCode.value__
        
        Write-ColorOutput "❌ $TestName - ${Duration}ms - $($_.Exception.Message)" $Red
    }
    
    return $testResult
}

function Run-RestaurantTests {
    Write-ColorOutput "🍕 Running Restaurant Service Tests..." $Cyan
    
    $results = @()
    
    # Test 1: Get Restaurant Basic Info
    $results += Invoke-HTTPTest -TestName "Get Restaurant Basic Info" -Method "GET" -Url "http://localhost:5001/api/restaurants/550e8400-e29b-41d4-a716-446655440001"
    
    # Test 2: Get Restaurant Validation Data
    $results += Invoke-HTTPTest -TestName "Get Restaurant Validation Data" -Method "GET" -Url "http://localhost:5001/api/restaurants/550e8400-e29b-41d4-a716-446655440001/validation"
    
    # Test 3: Get Restaurant Menu
    $results += Invoke-HTTPTest -TestName "Get Restaurant Menu" -Method "GET" -Url "http://localhost:5001/api/restaurants/550e8400-e29b-41d4-a716-446655440001/menu"
    
    # Test 4: Get Restaurant Status
    $results += Invoke-HTTPTest -TestName "Get Restaurant Status" -Method "GET" -Url "http://localhost:5001/api/restaurants/550e8400-e29b-41d4-a716-446655440001/status"
    
    # Test 5: Get Restaurant Operating Hours
    $results += Invoke-HTTPTest -TestName "Get Restaurant Operating Hours" -Method "GET" -Url "http://localhost:5001/api/restaurants/550e8400-e29b-41d4-a716-446655440001/operating-hours"
    
    # Test 6: Check Restaurant Active Status
    $results += Invoke-HTTPTest -TestName "Check Restaurant Active Status" -Method "GET" -Url "http://localhost:5001/api/restaurants/550e8400-e29b-41d4-a716-446655440001/is-active"
    
    # Test 7: Create Menu Item (Valid)
    $createMenuItemBody = @{
        restaurantId = "550e8400-e29b-41d4-a716-446655440001"
        categoryId = "550e8400-e29b-41d4-a716-446655440010"
        name = "Hawaiian Pizza"
        description = "Pineapple and ham pizza"
        price = 18.99
        isAvailable = $true
        isVegetarian = $false
        preparationTime = 25
    } | ConvertTo-Json
    
    $results += Invoke-HTTPTest -TestName "Create Menu Item (Valid)" -Method "POST" -Url "http://localhost:5001/api/menu-items" -Body $createMenuItemBody
    
    # Test 8: Create Menu Item (Duplicate Name - Should Fail)
    $results += Invoke-HTTPTest -TestName "Create Menu Item (Duplicate Name)" -Method "POST" -Url "http://localhost:5001/api/menu-items" -Body $createMenuItemBody
    
    # Test 9: Create Menu Item (Invalid Price - Should Fail)
    $invalidPriceBody = @{
        restaurantId = "550e8400-e29b-41d4-a716-446655440001"
        categoryId = "550e8400-e29b-41d4-a716-446655440010"
        name = "Test Pizza"
        description = "Test pizza with invalid price"
        price = -5.00
        isAvailable = $true
        isVegetarian = $false
        preparationTime = 20
    } | ConvertTo-Json
    
    $results += Invoke-HTTPTest -TestName "Create Menu Item (Invalid Price)" -Method "POST" -Url "http://localhost:5001/api/menu-items" -Body $invalidPriceBody
    
    return $results
}

function Run-OrderTests {
    Write-ColorOutput "📦 Running Order Service Tests..." $Cyan
    
    $results = @()
    
    # Test 1: Create Order (Valid)
    $createOrderBody = @{
        userId = "550e8400-e29b-41d4-a716-446655440000"
        restaurantId = "550e8400-e29b-41d4-a716-446655440001"
        items = @(
            @{
                menuItemId = "550e8400-e29b-41d4-a716-446655440002"
                itemName = "Margherita Pizza"
                quantity = 2
                unitPrice = 15.99
            },
            @{
                menuItemId = "550e8400-e29b-41d4-a716-446655440004"
                itemName = "Caesar Salad"
                quantity = 1
                unitPrice = 8.99
            }
        )
        notes = "Extra cheese please"
    } | ConvertTo-Json -Depth 3
    
    $results += Invoke-HTTPTest -TestName "Create Order (Valid)" -Method "POST" -Url "http://localhost:5000/api/order" -Body $createOrderBody
    
    # Test 2: Create Order (Invalid Restaurant - Should Fail)
    $invalidRestaurantBody = @{
        userId = "550e8400-e29b-41d4-a716-446655440000"
        restaurantId = "00000000-0000-0000-0000-000000000000"
        items = @(
            @{
                menuItemId = "550e8400-e29b-41d4-a716-446655440002"
                itemName = "Margherita Pizza"
                quantity = 1
                unitPrice = 15.99
            }
        )
    } | ConvertTo-Json -Depth 3
    
    $results += Invoke-HTTPTest -TestName "Create Order (Invalid Restaurant)" -Method "POST" -Url "http://localhost:5000/api/order" -Body $invalidRestaurantBody
    
    # Test 3: Create Order (Below Minimum Amount - Should Fail)
    $belowMinimumBody = @{
        userId = "550e8400-e29b-41d4-a716-446655440000"
        restaurantId = "550e8400-e29b-41d4-a716-446655440001"
        items = @(
            @{
                menuItemId = "550e8400-e29b-41d4-a716-446655440004"
                itemName = "Caesar Salad"
                quantity = 1
                unitPrice = 5.00
            }
        )
    } | ConvertTo-Json -Depth 3
    
    $results += Invoke-HTTPTest -TestName "Create Order (Below Minimum)" -Method "POST" -Url "http://localhost:5000/api/order" -Body $belowMinimumBody
    
    # Test 4: Get Orders
    $results += Invoke-HTTPTest -TestName "Get Orders" -Method "GET" -Url "http://localhost:5000/api/order"
    
    return $results
}

function Generate-TestReport {
    param([array]$RestaurantResults, [array]$OrderResults)
    
    $timestamp = Get-Date -Format "yyyy-MM-dd_HH-mm-ss"
    $reportPath = Join-Path $ReportsDir "test-report-$timestamp.html"
    
    $totalTests = $RestaurantResults.Count + $OrderResults.Count
    $passedTests = ($RestaurantResults | Where-Object { $_.Success }).Count + ($OrderResults | Where-Object { $_.Success }).Count
    $failedTests = $totalTests - $passedTests
    
    $html = @"
<!DOCTYPE html>
<html>
<head>
    <title>SmartDeliveryApp Test Report - $timestamp</title>
    <style>
        body { font-family: Arial, sans-serif; margin: 20px; }
        .header { background-color: #f0f0f0; padding: 20px; border-radius: 5px; }
        .summary { background-color: #e8f5e8; padding: 15px; border-radius: 5px; margin: 20px 0; }
        .test-section { margin: 20px 0; }
        .test-result { margin: 10px 0; padding: 10px; border-radius: 3px; }
        .success { background-color: #d4edda; border-left: 4px solid #28a745; }
        .failure { background-color: #f8d7da; border-left: 4px solid #dc3545; }
        .details { font-family: monospace; font-size: 12px; margin-top: 10px; }
    </style>
</head>
<body>
    <div class="header">
        <h1>SmartDeliveryApp Test Report</h1>
        <p>Generated: $timestamp</p>
    </div>
    
    <div class="summary">
        <h2>Test Summary</h2>
        <p><strong>Total Tests:</strong> $totalTests</p>
        <p><strong>Passed:</strong> $passedTests</p>
        <p><strong>Failed:</strong> $failedTests</p>
        <p><strong>Success Rate:</strong> $([math]::Round(($passedTests / $totalTests) * 100, 2))%</p>
    </div>
    
    <div class="test-section">
        <h2>Restaurant Service Tests</h2>
"@

    foreach ($result in $RestaurantResults) {
        $statusClass = if ($result.Success) { "success" } else { "failure" }
        $statusIcon = if ($result.Success) { "✅" } else { "❌" }
        
        $html += @"
        <div class="test-result $statusClass">
            <h3>$statusIcon $($result.Name)</h3>
            <p><strong>Method:</strong> $($result.Method) | <strong>URL:</strong> $($result.Url) | <strong>Duration:</strong> $($result.Duration)ms</p>
            <div class="details">
                <p><strong>Response:</strong></p>
                <pre>$($result.Response | ConvertTo-Json -Depth 3)</pre>
                $(if ($result.Error) { "<p><strong>Error:</strong> $($result.Error)</p>" })
            </div>
        </div>
"@
    }
    
    $html += @"
    </div>
    
    <div class="test-section">
        <h2>Order Service Tests</h2>
"@

    foreach ($result in $OrderResults) {
        $statusClass = if ($result.Success) { "success" } else { "failure" }
        $statusIcon = if ($result.Success) { "✅" } else { "❌" }
        
        $html += @"
        <div class="test-result $statusClass">
            <h3>$statusIcon $($result.Name)</h3>
            <p><strong>Method:</strong> $($result.Method) | <strong>URL:</strong> $($result.Url) | <strong>Duration:</strong> $($result.Duration)ms</p>
            <div class="details">
                <p><strong>Response:</strong></p>
                <pre>$($result.Response | ConvertTo-Json -Depth 3)</pre>
                $(if ($result.Error) { "<p><strong>Error:</strong> $($result.Error)</p>" })
            </div>
        </div>
"@
    }
    
    $html += @"
    </div>
</body>
</html>
"@
    
    $html | Out-File -FilePath $reportPath -Encoding UTF8
    Write-ColorOutput "📊 Test report generated: $reportPath" $Cyan
    
    return $reportPath
}

# Main execution
Write-ColorOutput "🚀 SmartDeliveryApp Comprehensive Test Runner" $Cyan
Write-ColorOutput "Environment: $Environment" $Yellow

$processes = $null

try {
    # Start services if not skipped
    if (-not $SkipServices) {
        $processes = Start-Services
        
        # Test service health
        $restaurantHealthy = Test-ServiceHealth "Restaurant Service" "http://localhost:5001"
        $orderHealthy = Test-ServiceHealth "Order Service" "http://localhost:5000"
        
        if (-not $restaurantHealthy -or -not $orderHealthy) {
            Write-ColorOutput "❌ Services are not healthy. Please check if they're running correctly." $Red
            exit 1
        }
    }
    
    # Run tests
    Write-ColorOutput "🧪 Running Comprehensive Tests..." $Cyan
    
    $restaurantResults = Run-RestaurantTests
    $orderResults = Run-OrderTests
    
    # Generate report
    if ($GenerateReport) {
        $reportPath = Generate-TestReport -RestaurantResults $restaurantResults -OrderResults $orderResults
    }
    
    # Summary
    $totalTests = $restaurantResults.Count + $orderResults.Count
    $passedTests = ($restaurantResults | Where-Object { $_.Success }).Count + ($orderResults | Where-Object { $_.Success }).Count
    $failedTests = $totalTests - $passedTests
    
    Write-ColorOutput "" $White
    Write-ColorOutput "📊 Test Summary:" $Cyan
    Write-ColorOutput "   Total Tests: $totalTests" $White
    Write-ColorOutput "   Passed: $passedTests" $Green
    Write-ColorOutput "   Failed: $failedTests" $(if ($failedTests -gt 0) { $Red } else { $Green })
    Write-ColorOutput "   Success Rate: $([math]::Round(($passedTests / $totalTests) * 100, 2))%" $White
    
    if ($failedTests -gt 0) {
        Write-ColorOutput "❌ Some tests failed. Check the report for details." $Red
        exit 1
    } else {
        Write-ColorOutput "✅ All tests passed!" $Green
    }
}
finally {
    # Stop services
    if ($processes) {
        Stop-Services -Processes $processes
    }
}
