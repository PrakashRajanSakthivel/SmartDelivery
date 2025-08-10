# ========================================
# SmartDeliveryApp - Full Test Suite Orchestrator
# ========================================
# This script orchestrates the complete testing process:
# 1. Seed test data
# 2. Run comprehensive tests
# 3. Cleanup test data
# 4. Generate final report
# Usage: .\scripts\run-full-test-suite.ps1

param(
    [string]$Environment = "Development",
    [switch]$SkipCleanup = $false,
    [switch]$SkipServices = $false,
    [switch]$GenerateReport = $true
)

# Configuration
$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$ProjectRoot = Split-Path -Parent $ScriptDir

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

function Write-SectionHeader {
    param([string]$Title)
    Write-ColorOutput "" $White
    Write-ColorOutput "=" * 60 $Cyan
    Write-ColorOutput "  $Title" $Cyan
    Write-ColorOutput "=" * 60 $Cyan
    Write-ColorOutput "" $White
}

function Test-Prerequisites {
    Write-SectionHeader "Checking Prerequisites"
    
    # Check if scripts exist
    $seedScript = Join-Path $ScriptDir "seed-test-data.ps1"
    $testScript = Join-Path $ScriptDir "run-comprehensive-tests.ps1"
    
    if (-not (Test-Path $seedScript)) {
        Write-ColorOutput "❌ Seed script not found: $seedScript" $Red
        return $false
    }
    
    if (-not (Test-Path $testScript)) {
        Write-ColorOutput "❌ Test script not found: $testScript" $Red
        return $false
    }
    
    Write-ColorOutput "✅ All prerequisite scripts found" $Green
    
    # Check if services can be built
    Write-ColorOutput "🔨 Building services..." $Yellow
    try {
        Push-Location $ProjectRoot
        $buildResult = dotnet build --no-restore
        if ($LASTEXITCODE -ne 0) {
            Write-ColorOutput "❌ Build failed" $Red
            return $false
        }
        Write-ColorOutput "✅ Build successful" $Green
    }
    finally {
        Pop-Location
    }
    
    return $true
}

function Invoke-SeedData {
    Write-SectionHeader "Seeding Test Data"
    
    $seedScript = Join-Path $ScriptDir "seed-test-data.ps1"
    $arguments = @("-Environment", $Environment)
    
    Write-ColorOutput "🌱 Running seed script..." $Yellow
    $seedResult = & $seedScript @arguments
    
    if ($LASTEXITCODE -ne 0) {
        Write-ColorOutput "❌ Seed data failed" $Red
        return $false
    }
    
    Write-ColorOutput "✅ Seed data completed successfully" $Green
    return $true
}

function Invoke-ComprehensiveTests {
    Write-SectionHeader "Running Comprehensive Tests"
    
    $testScript = Join-Path $ScriptDir "run-comprehensive-tests.ps1"
    $arguments = @("-Environment", $Environment, "-GenerateReport", $GenerateReport)
    
    if ($SkipServices) {
        $arguments += "-SkipServices"
    }
    
    Write-ColorOutput "🧪 Running comprehensive tests..." $Yellow
    $testResult = & $testScript @arguments
    
    if ($LASTEXITCODE -ne 0) {
        Write-ColorOutput "❌ Comprehensive tests failed" $Red
        return $false
    }
    
    Write-ColorOutput "✅ Comprehensive tests completed successfully" $Green
    return $true
}

function Invoke-Cleanup {
    Write-SectionHeader "Cleaning Up Test Data"
    
    $seedScript = Join-Path $ScriptDir "seed-test-data.ps1"
    $arguments = @("-Environment", $Environment, "-CleanupOnly")
    
    Write-ColorOutput "🧹 Running cleanup..." $Yellow
    $cleanupResult = & $seedScript @arguments
    
    if ($LASTEXITCODE -ne 0) {
        Write-ColorOutput "❌ Cleanup failed" $Red
        return $false
    }
    
    Write-ColorOutput "✅ Cleanup completed successfully" $Green
    return $true
}

function Generate-FinalReport {
    param([bool]$SeedSuccess, [bool]$TestSuccess, [bool]$CleanupSuccess)
    
    Write-SectionHeader "Final Test Report"
    
    $timestamp = Get-Date -Format "yyyy-MM-dd HH:mm:ss"
    $overallSuccess = $SeedSuccess -and $TestSuccess -and ($CleanupSuccess -or $SkipCleanup)
    
    Write-ColorOutput "📊 Test Suite Summary" $Cyan
    Write-ColorOutput "   Timestamp: $timestamp" $White
    Write-ColorOutput "   Environment: $Environment" $White
    Write-ColorOutput "" $White
    
    Write-ColorOutput "📋 Phase Results:" $Cyan
    Write-ColorOutput "   🌱 Data Seeding: $(if ($SeedSuccess) { '✅ PASSED' } else { '❌ FAILED' })" $(if ($SeedSuccess) { $Green } else { $Red })
    Write-ColorOutput "   🧪 Comprehensive Tests: $(if ($TestSuccess) { '✅ PASSED' } else { '❌ FAILED' })" $(if ($TestSuccess) { $Green } else { $Red })
    Write-ColorOutput "   🧹 Data Cleanup: $(if ($SkipCleanup) { '⏭️ SKIPPED' } elseif ($CleanupSuccess) { '✅ PASSED' } else { '❌ FAILED' })" $(if ($SkipCleanup) { $Yellow } elseif ($CleanupSuccess) { $Green } else { $Red })
    Write-ColorOutput "" $White
    
    Write-ColorOutput "🎯 Overall Result: $(if ($overallSuccess) { '✅ ALL TESTS PASSED' } else { '❌ SOME TESTS FAILED' })" $(if ($overallSuccess) { $Green } else { $Red })
    
    if ($overallSuccess) {
        Write-ColorOutput "" $White
        Write-ColorOutput "🎉 Congratulations! All tests passed successfully!" $Green
        Write-ColorOutput "   - Business validation rules are working correctly" $White
        Write-ColorOutput "   - Inter-service communication is functioning" $White
        Write-ColorOutput "   - API endpoints are responding as expected" $White
        Write-ColorOutput "   - Database operations are successful" $White
    } else {
        Write-ColorOutput "" $White
        Write-ColorOutput "⚠️  Some tests failed. Please review the detailed reports:" $Yellow
        Write-ColorOutput "   - Check the test report in the reports/ directory" $White
        Write-ColorOutput "   - Review service logs for errors" $White
        Write-ColorOutput "   - Verify database connections and data" $White
    }
    
    return $overallSuccess
}

# Main execution
Write-ColorOutput "🚀 SmartDeliveryApp Full Test Suite Orchestrator" $Cyan
Write-ColorOutput "Environment: $Environment" $Yellow
Write-ColorOutput "Skip Cleanup: $SkipCleanup" $Yellow
Write-ColorOutput "Skip Services: $SkipServices" $Yellow
Write-ColorOutput "Generate Report: $GenerateReport" $Yellow

$startTime = Get-Date
$overallSuccess = $true

try {
    # Step 1: Check prerequisites
    if (-not (Test-Prerequisites)) {
        Write-ColorOutput "❌ Prerequisites check failed" $Red
        exit 1
    }
    
    # Step 2: Seed test data
    $seedSuccess = Invoke-SeedData
    if (-not $seedSuccess) {
        $overallSuccess = $false
        Write-ColorOutput "❌ Cannot proceed without seed data" $Red
        exit 1
    }
    
    # Step 3: Run comprehensive tests
    $testSuccess = Invoke-ComprehensiveTests
    if (-not $testSuccess) {
        $overallSuccess = $false
    }
    
    # Step 4: Cleanup (unless skipped)
    $cleanupSuccess = $true
    if (-not $SkipCleanup) {
        $cleanupSuccess = Invoke-Cleanup
        if (-not $cleanupSuccess) {
            $overallSuccess = $false
        }
    }
    
    # Step 5: Generate final report
    $finalSuccess = Generate-FinalReport -SeedSuccess $seedSuccess -TestSuccess $testSuccess -CleanupSuccess $cleanupSuccess
    
    # Calculate total duration
    $endTime = Get-Date
    $duration = $endTime - $startTime
    
    Write-ColorOutput "" $White
    Write-ColorOutput "⏱️  Total Duration: $($duration.TotalMinutes.ToString('F2')) minutes" $Cyan
    
    if ($finalSuccess) {
        Write-ColorOutput "✅ Test suite completed successfully!" $Green
        exit 0
    } else {
        Write-ColorOutput "❌ Test suite completed with failures" $Red
        exit 1
    }
}
catch {
    Write-ColorOutput "💥 Unexpected error during test execution: $($_.Exception.Message)" $Red
    Write-ColorOutput "Stack trace: $($_.ScriptStackTrace)" $Red
    exit 1
}
