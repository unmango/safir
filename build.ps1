#
# https://github.com/dotnet/core-sdk/blob/master/run-build.ps1
#

[CmdletBinding(PositionalBinding=$false)]
param(
    [string]$Configuration="Debug",
    [string]$Architecture="x64",
    [bool]$WarnAsError=$true,
    [Parameter(ValueFromRemainingArguments=$true)][String[]]$ExtraParameters
)

$RepoRoot = "$PSScriptRoot"

$Parameters = "/p:Architecture=$Architecture"
$Parameters = "$Parameters -configuration $Configuration"

$Parameters = "$Parameters -WarnAsError `$$WarnAsError"

try {
    $ExpressionToInvoke = "$RepoRoot\build\core-build.ps1 -restore -build $Parameters $ExtraParameters"
    Write-Host "Invoking expression: $ExpressionToInvoke"
    Invoke-Expression $ExpressionToInvoke
}
catch {
    Write-Error $_
    Write-Error $_.ScriptStackTrace
    throw "Failed to build"
}

if($LASTEXITCODE -ne 0) { throw "Failed to build" }
