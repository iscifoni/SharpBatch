param(
    [int] $iterations = 20000,
    [int] $rps = 1000,
    [int] $timeout = 36000000)

$url = "http://localhost:5000/batch/exec/basetest/method1"

Write-Host -ForegroundColor Green Beginning workload
Write-Host "`& loadtest -k -n $iterations -c 256 --timeout $timeout --rps $rps $url"
Write-Host

& loadtest -k -n $iterations -c 256 --timeout $timeout  --rps $rps $url