gitroot=$(git rev-parse --show-toplevel)
projectdir="$gitroot/src/common/dotnet/src/Safir.Protos"
docker build -t dotnet-grpc-refresh "$projectdir"
docker run --rm -v "$projectdir:/work" dotnet-grpc-refresh
