FROM mcr.microsoft.com/dotnet/sdk:6.0

ENV PATH="${PATH}:/root/.dotnet/tools"
RUN dotnet tool install -g dotnet-grpc

VOLUME ["/work"]
WORKDIR /work

CMD ["/usr/bin/dotnet","grpc","refresh"]
