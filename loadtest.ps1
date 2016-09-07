param(
    [int] $iterations = 3000,
    [int] $rps = 250)

$url = "http://localhost:5000/batch/exec/basetest/method1"

Write-Host -ForegroundColor Green Beginning workload
Write-Host "`& loadtest -k -n $iterations -c 32 --rps $rps $url"
Write-Host

& loadtest -k -n $iterations -c 32 --rps $rps $url