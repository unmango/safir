[CmdletBinding(PositionalBinding=$false)]
Param(
  [ValidateSet('Debug', 'Release')]
  [string][Alias('c')]$configuration = "Debug",
  [string]$platform = $null,
  [string] $projects,
  [string][Alias('v')]$verbosity = "minimal",
  [bool] $warnAsError = $true,
  [switch][Alias('r')]$restore,
  [switch][Alias('b')]$build,
  [switch][Alias('t')]$test,
  [switch] $integrationTest,
  [switch] $performanceTest,
  [switch] $pack,
  [switch] $publish,
  [switch] $ci,
  [switch] $help,
  [Parameter(ValueFromRemainingArguments=$true)][String[]]$properties
)

Set-StrictMode -Version 1
$ErrorActionPreference = 'Stop'

Import-Module -Force -Scope Local "$PSScriptRoot/../src/common.psm1"

#
# Main
#

if ($env:CI -eq 'true') {
    $ci = $true
}

if (!$Configuration) {
    $Configuration = if ($ci) { 'Release' } else { 'Debug' }
}

if ($ci) {
    $properties += '-p:CI=true'
    $env:DOTNET_CLI_TELEMETRY_OPTOUT = 1;
}

$isPr = ($env:BUILD_REASON -eq 'PullRequest')
if (-not (Test-Path variable:\IsCoreCLR)) {
    $IsWindows = $true
}

$artifacts = "$PSScriptRoot/artifacts/"
$packages = $artifacts + "packages/"
$web = $artifacts + "web/"

Remove-Item -Recurse $artifacts -ErrorAction Ignore
exec dotnet msbuild /t:UpdateCiSettings @properties
exec dotnet build --configuration $Configuration @properties
exec dotnet pack --no-restore --no-build --configuration $Configuration -o $packages @properties

#$importer = $web + "importer/"
#exec dotnet publish --no-restore --no-build --configuration $Configuration -o $importer `
#    "$PSScriptRoot/src/Safir.Importer.Service/Safir.Importer.Service.csproj" `
#    @properties

[string[]] $testArgs=@()
if ($PSVersionTable.PSEdition -eq 'Core' -and -not $IsWindows) {
    $testArgs += '--framework','netcoreapp2.2'
}
if ($env:TF_BUILD) {
    $testArgs += '--logger', 'trx'
}

if ($integrationTest) {
    exec dotnet test --no-restore --no-build --configuration $Configuration '-clp:Summary' `
        "$PSScriptRoot/test/Safir.Importer.Service.IntegrationTests/Safir.Importer.Service.IntegrationTests.csproj" `
        --filter "Category=Integration" `
        @testArgs `
        @properties
}

write-host -f magenta 'Done'

exit 0
