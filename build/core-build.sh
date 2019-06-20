#!/usr/bin/env bash

# Stop script if unbound variable found (use ${var:-} if intentional)
set -u

# Stop script if command returns non-zero exit code.
# Prevents hidden errors caused by missing error code propagation.
set -e

usage()
{
  echo "Common settings:"
  echo "  --configuration <value>    Build configuration: 'Debug' or 'Release' (short: -c)"
  echo "  --verbosity <value>        Msbuild verbosity: q[uiet], m[inimal], n[ormal], d[etailed], and diag[nostic] (short: -v)"
  echo "  --help                     Print help and exit (short: -h)"
  echo ""

  echo "Actions:"
  echo "  --restore                  Restore dependencies (short: -r)"
  echo "  --build                    Build solution (short: -b)"
  echo "  --rebuild                  Rebuild solution"
  echo "  --test                     Run all unit tests in the solution (short: -t)"
  echo "  --integrationTest          Run all integration tests in the solution"
  echo "  --performanceTest          Run all performance tests in the solution"
  echo "  --pack                     Package build outputs into NuGet packages and Willow components"
  echo "  --publish                  Publish artifacts (e.g. symbols)"
  echo ""

  echo "Advanced settings:"
  echo "  --projects <value>       Project or solution file(s) to build"
  echo "  --ci                     Set when running on CI server"
  echo "  --warnAsError <value>    Sets warnaserror msbuild parameter ('true' or 'false')"
  echo ""
  echo "Command line arguments not listed above are passed thru to msbuild."
  echo "Arguments can also be passed in with a single hyphen."
}

source="${BASH_SOURCE[0]}"

# resolve $source until the file is no longer a symlink
while [[ -h "$source" ]]; do
  scriptroot="$( cd -P "$( dirname "$source" )" && pwd )"
  source="$(readlink "$source")"
  # if $source was a relative symlink, we need to resolve it relative to the path where the
  # symlink file was located
  [[ $source != /* ]] && source="$scriptroot/$source"
done
scriptroot="$( cd -P "$( dirname "$source" )" && pwd )"

restore=false
build=false
rebuild=false
test=false
integration_test=false
performance_test=false
pack=false
publish=false
ci=false

warn_as_error=true

projects=''
configuration='Debug'
verbosity='minimal'

properties=''

while [[ $# > 0 ]]; do
  opt="$(echo "${1/#--/-}" | awk '{print tolower($0)}')"
  case "$opt" in
    -help|-h)
      usage
      exit 0
      ;;
    -configuration|-c)
      configuration=$2
      shift
      ;;
    -verbosity|-v)
      verbosity=$2
      shift
      ;;
    -restore|-r)
      restore=true
      ;;
    -build|-b)
      build=true
      ;;
    -rebuild)
      rebuild=true
      ;;
    -pack)
      pack=true
      ;;
    -test|-t)
      test=true
      ;;
    -integrationtest)
      integration_test=true
      ;;
    -performancetest)
      performance_test=true
      ;;
    -publish)
      publish=true
      ;;
    -projects)
      projects=$2
      shift
      ;;
    -ci)
      ci=true
      ;;
    -warnaserror)
      warn_as_error=$2
      shift
      ;;
    *)
      properties="$properties $1"
      ;;
  esac

  shift
done

# TODO: Actually build
if [ "${CI-}" = true ]; then
  ci=true
fi

artifacts="$scriptroot/artifacts"
packages="$artifacts/packages"

dotnet msbuild /t:UpdateCiSettings
dotnet build --configuration $configuration
dotnet pack --no-restore --no-build --configuration $configuration -o $packages

if $integration_test; then
  dotnet test --no-restore --no-build --configuration $configuration '-clp:Summary' \
    "$scriptroot/test/Safir.Importer.Service.IntegrationTests/Safir.Importer.IntegrationTests.csproj" \
    --filter "Category=Integration"
fi

echo -e "${PURPLE}Done${NC}"
