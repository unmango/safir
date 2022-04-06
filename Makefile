SHELL       := /bin/bash

common_dotnet_docker::
	cd src && docker build . \
		-f common/dotnet/Dockerfile \
		-t safir-common-dotnet

agent::
	dotnet build src/agent

agent_docker:: common_dotnet_docker
	cd src && docker build . -f agent/Dockerfile
