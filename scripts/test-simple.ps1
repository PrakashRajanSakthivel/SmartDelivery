param(
    [string]$Environment = "Development"
)

Write-Host "Testing PowerShell script" -ForegroundColor Green
Write-Host "Environment: $Environment" -ForegroundColor Yellow

$test = $true

if ($test) {
    Write-Host "Test passed" -ForegroundColor Green
} else {
    Write-Host "Test failed" -ForegroundColor Red
}
