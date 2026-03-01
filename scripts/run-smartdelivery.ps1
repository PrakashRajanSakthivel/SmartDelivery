# SmartDelivery helper script
#  - Launches the five API backends via `dotnet watch run` (hot reload ready)
#  - Offers a small menu to rerun all tests, focus on the `OrderProcessingFlowTest`,
#    or spin up the same test under a debugger attach wait.
#  - Use Ctrl+C or select Quit to stop and tear down all spawned processes.
#  - Pass -DebugOrderFlow to start with an attachable `OrderProcessingFlowTest` run.

param(
    [switch]$SkipServices,
    [switch]$SkipTests,
    [switch]$DebugOrderFlow
)

$ErrorActionPreference = 'Stop'

$repoRoot = Resolve-Path -LiteralPath (Join-Path $PSScriptRoot '..')
$pwshExe = (Get-Command pwsh -ErrorAction SilentlyContinue)?.Source
if (-not $pwshExe) {
    $pwshExe = "$env:SystemRoot\\System32\\WindowsPowerShell\\v1.0\\powershell.exe"
}

$serviceDefinitions = @(
    @{ Name = 'AuthService'; Path = 'src/services/AuthService/AuthService.API' },
    @{ Name = 'RestaurantService'; Path = 'src/services/RestaurantService/Restaurant.API' },
    @{ Name = 'OrderService'; Path = 'src/services/OrderService/OrderService.API' },
    @{ Name = 'CartService'; Path = 'src/services/CartService/CartService.API' },
    @{ Name = 'PaymentService'; Path = 'src/services/PaymentService/PaymentService.API' }
)

$serviceProcs = [System.Collections.Generic.List[System.Diagnostics.Process]]::new()

function Start-ServiceHost {
    param(
        [string]$name,
        [string]$relativePath
    )

    $fullPath = Join-Path $repoRoot $relativePath
    if (-not (Test-Path -LiteralPath $fullPath)) {
        throw "Service '$name' path not found: $relativePath"
    }

    $command = "Set-Location `"$fullPath`"; dotnet watch run"
    $process = Start-Process -FilePath $pwshExe -ArgumentList '-NoLogo','-NoExit','-Command',$command -PassThru
    $serviceProcs.Add($process)
    Write-Host "[$name] dotnet watch run (PID: $($process.Id))" -ForegroundColor Green
}

function Stop-ServiceHosts {
    foreach ($proc in $serviceProcs) {
        if ($proc -and -not $proc.HasExited) {
            Write-Host "Stopping PID $($proc.Id) ..."
            Stop-Process -Id $proc.Id -Force
        }
    }
}

function Invoke-Tests {
    param(
        [string]$filter,
        [switch]$Debug
    )

    Push-Location $repoRoot
    try {
        $testProject = Join-Path $repoRoot 'OrderService.Infra.Test/OrderService.Infra.Test.csproj'
        $args = @('test',$testProject,'--no-build')
        if ($filter) {
            $args += @('--filter',$filter)
        }
        if ($Debug) {
            $env:VSTEST_HOST_DEBUG = '1'
        }
        & dotnet @args
    }
    finally {
        if ($Debug) {
            Remove-Item Env:\VSTEST_HOST_DEBUG -ErrorAction SilentlyContinue
        }
        Pop-Location
    }
}

try {
    if (-not $SkipServices) {
        foreach ($svc in $serviceDefinitions) {
            Start-ServiceHost -name $svc.Name -relativePath $svc.Path
        }
        Write-Host 'Services launched. Leave the spawned windows open for hot reload after code changes.'
        Start-Sleep -Seconds 5
    }

    if ($DebugOrderFlow) {
        Invoke-Tests -filter 'FullyQualifiedName~OrderProcessingFlowTest' -Debug
    }
    elseif (-not $SkipTests) {
        Invoke-Tests
    }

    while ($true) {
        $choice = Read-Host 'Press [R]erun all tests, [O]rder flow test, [D]ebug order flow, [Q]uit'
        switch ($choice.ToUpperInvariant()) {
            'R' { Invoke-Tests }
            'O' { Invoke-Tests -filter 'FullyQualifiedName~OrderProcessingFlowTest' }
            'D' { Invoke-Tests -filter 'FullyQualifiedName~OrderProcessingFlowTest' -Debug }
            'Q' { break }
            default { Write-Host 'Unknown option.' -ForegroundColor Yellow }
        }
    }
}
finally {
    Stop-ServiceHosts
}
