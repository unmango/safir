#!/bin/bash
set -e

docker build . --file Dockerfile \
  --build-arg GithubUsername="$GITHUB_USERNAME" \
  --build-arg GithubPassword="$GITHUB_PASSWORD" \
  --tag safir-agent:"$(date +%s)"
