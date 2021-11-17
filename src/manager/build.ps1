#!/usr/bin/env pwsh
$ErrorActionPreference = 'Stop'

docker build . --file Dockerfile `
  --build-arg GithubUsername=$env:GITHUB_USERNAME `
  --build-arg GithubPassword=$env:GITHUB_PASSWORD `
  --tag safir-agent:$(date +%s)
