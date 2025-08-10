#!/usr/bin/env python3
"""
Comprehensive Test Runner
=========================
This script runs a complete test suite:
1. Seeds test data
2. Runs HTTP tests against services
3. Validates results
4. Cleans up test data
"""

import subprocess
import sys
import time
import requests
import json
import urllib3
from pathlib import Path

# Suppress SSL warnings for local development
urllib3.disable_warnings(urllib3.exceptions.InsecureRequestWarning)

class Colors:
    GREEN = '\033[92m'
    RED = '\033[91m'
    YELLOW = '\033[93m'
    CYAN = '\033[96m'
    WHITE = '\033[97m'
    RESET = '\033[0m'

def write_color_output(message: str, color: str = Colors.WHITE):
    """Write colored output"""
    print(f"{color}{message}{Colors.RESET}")

def run_command(command: str, description: str) -> bool:
    """Run a command and return success status"""
    write_color_output(f"Running {description}...", Colors.YELLOW)
    write_color_output(f"   Command: {command}", Colors.WHITE)
    
    try:
        result = subprocess.run(command, shell=True, capture_output=True, text=True)
        if result.returncode == 0:
            write_color_output(f"SUCCESS: {description} completed successfully", Colors.GREEN)
            if result.stdout:
                write_color_output(f"   Output: {result.stdout.strip()}", Colors.WHITE)
            return True
        else:
            write_color_output(f"FAILED: {description} failed", Colors.RED)
            if result.stderr:
                write_color_output(f"   Error: {result.stderr.strip()}", Colors.RED)
            return False
    except Exception as e:
        write_color_output(f"FAILED: {description} failed with exception: {str(e)}", Colors.RED)
        return False

def test_restaurant_api():
    """Test Restaurant API endpoints"""
    write_color_output("Testing Restaurant API...", Colors.CYAN)
    
    base_url = "https://localhost:7101"
    test_restaurant_id = "550e8400-e29b-41d4-a716-446655440001"
    
    tests = [
        {
            "name": "Get Restaurant by ID",
            "url": f"{base_url}/api/restaurants/{test_restaurant_id}",
            "method": "GET",
            "expected_status": 200
        },
        {
            "name": "Get Restaurant for Validation",
            "url": f"{base_url}/api/restaurants/{test_restaurant_id}/validation",
            "method": "GET",
            "expected_status": 200
        },
        {
            "name": "Get Restaurant Operating Hours",
            "url": f"{base_url}/api/restaurants/{test_restaurant_id}/operating-hours",
            "method": "GET",
            "expected_status": 200
        },
        {
            "name": "Get Restaurant Active Status",
            "url": f"{base_url}/api/restaurants/{test_restaurant_id}/is-active",
            "method": "GET",
            "expected_status": 200
        }
    ]
    
    results = []
    for test in tests:
        try:
            response = requests.request(
                method=test["method"],
                url=test["url"],
                timeout=10,
                verify=False  # Disable SSL verification for local development
            )
            
            success = response.status_code == test["expected_status"]
            status = "PASS" if success else "FAIL"
            
            write_color_output(f"   {status} {test['name']}: {response.status_code}", 
                             Colors.GREEN if success else Colors.RED)
            
            results.append({
                "test": test["name"],
                "success": success,
                "status_code": response.status_code,
                "response": response.text[:200] if response.text else ""
            })
            
        except requests.exceptions.ConnectionError:
            write_color_output(f"   FAIL {test['name']}: Connection refused (service not running)", Colors.RED)
            results.append({
                "test": test["name"],
                "success": False,
                "status_code": None,
                "response": "Connection refused"
            })
        except Exception as e:
            write_color_output(f"   FAIL {test['name']}: {str(e)}", Colors.RED)
            results.append({
                "test": test["name"],
                "success": False,
                "status_code": None,
                "response": str(e)
            })
    
    return results

def test_order_api():
    """Test Order API endpoints"""
    write_color_output("Testing Order API...", Colors.CYAN)
    
    base_url = "https://localhost:7074"
    test_user_id = "550e8400-e29b-41d4-a716-446655440000"
    
    tests = [
        {
            "name": "Get Orders by User ID",
            "url": f"{base_url}/api/Order?userId={test_user_id}",
            "method": "GET",
            "expected_status": 200
        },
        {
            "name": "Get All Orders",
            "url": f"{base_url}/api/Order",
            "method": "GET",
            "expected_status": 200
        }
    ]
    
    results = []
    for test in tests:
        try:
            response = requests.request(
                method=test["method"],
                url=test["url"],
                timeout=10,
                verify=False  # Disable SSL verification for local development
            )
            
            success = response.status_code == test["expected_status"]
            status = "PASS" if success else "FAIL"
            
            write_color_output(f"   {status} {test['name']}: {response.status_code}", 
                             Colors.GREEN if success else Colors.RED)
            
            results.append({
                "test": test["name"],
                "success": success,
                "status_code": response.status_code,
                "response": response.text[:200] if response.text else ""
            })
            
        except requests.exceptions.ConnectionError:
            write_color_output(f"   ❌ FAIL {test['name']}: Connection refused (service not running)", Colors.RED)
            results.append({
                "test": test["name"],
                "success": False,
                "status_code": None,
                "response": "Connection refused"
            })
        except Exception as e:
            write_color_output(f"   ❌ FAIL {test['name']}: {str(e)}", Colors.RED)
            results.append({
                "test": test["name"],
                "success": False,
                "status_code": None,
                "response": str(e)
            })
    
    return results

def validate_results(restaurant_results, order_results):
    """Validate test results and provide summary"""
    write_color_output("\nTest Results Summary", Colors.CYAN)
    write_color_output("=" * 40, Colors.WHITE)
    
    # Restaurant API results
    restaurant_passed = sum(1 for r in restaurant_results if r["success"])
    restaurant_total = len(restaurant_results)
    
    write_color_output(f"Restaurant API: {restaurant_passed}/{restaurant_total} tests passed", 
                      Colors.GREEN if restaurant_passed == restaurant_total else Colors.YELLOW)
    
    # Order API results
    order_passed = sum(1 for r in order_results if r["success"])
    order_total = len(order_results)
    
    write_color_output(f"Order API: {order_passed}/{order_total} tests passed", 
                      Colors.GREEN if order_passed == order_total else Colors.YELLOW)
    
    # Overall results
    total_passed = restaurant_passed + order_passed
    total_tests = restaurant_total + order_total
    
    write_color_output(f"\nOverall: {total_passed}/{total_tests} tests passed", 
                      Colors.GREEN if total_passed == total_tests else Colors.YELLOW)
    
    # Detailed results
    if restaurant_passed < restaurant_total:
        write_color_output("\nFailed Restaurant API Tests:", Colors.RED)
        for result in restaurant_results:
            if not result["success"]:
                write_color_output(f"   - {result['test']}: {result['response']}", Colors.RED)
    
    if order_passed < order_total:
        write_color_output("\nFailed Order API Tests:", Colors.RED)
        for result in order_results:
            if not result["success"]:
                write_color_output(f"   - {result['test']}: {result['response']}", Colors.RED)
    
    return total_passed == total_tests

def main():
    """Main execution function"""
    write_color_output("SmartDeliveryApp Comprehensive Test Suite", Colors.CYAN)
    write_color_output("=" * 50, Colors.WHITE)
    
    # Step 1: Seed test data
    write_color_output("\nStep 1: Seeding Test Data", Colors.YELLOW)
    if not run_command("python scripts/seed_test_data.py", "Seeding test data"):
        write_color_output("Failed to seed test data. Aborting tests.", Colors.RED)
        return 1
    
    # Step 2: Wait a moment for data to be available
    write_color_output("\nWaiting for data to be available...", Colors.YELLOW)
    time.sleep(2)
    
    # Step 3: Test Restaurant API
    write_color_output("\nStep 2: Testing Restaurant API", Colors.YELLOW)
    restaurant_results = test_restaurant_api()
    
    # Step 4: Test Order API
    write_color_output("\nStep 3: Testing Order API", Colors.YELLOW)
    order_results = test_order_api()
    
    # Step 5: Validate results
    write_color_output("\nStep 4: Validating Results", Colors.YELLOW)
    all_tests_passed = validate_results(restaurant_results, order_results)
    
    # Step 6: Cleanup test data
    write_color_output("\nStep 5: Cleaning Up Test Data", Colors.YELLOW)
    if not run_command("python scripts/seed_test_data.py --cleanup-only", "Cleaning up test data"):
        write_color_output("Warning: Failed to cleanup test data", Colors.YELLOW)
    
    # Final summary
    write_color_output("\nTest Suite Complete", Colors.CYAN)
    write_color_output("=" * 30, Colors.WHITE)
    
    if all_tests_passed:
        write_color_output("SUCCESS: All tests passed! The system is working correctly.", Colors.GREEN)
        return 0
    else:
        write_color_output("FAILED: Some tests failed. Please check the issues above.", Colors.RED)
        return 1

if __name__ == "__main__":
    sys.exit(main())
