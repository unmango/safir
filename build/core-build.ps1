[CmdletBinding(PositionalBinding=$false)]
Param(
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
