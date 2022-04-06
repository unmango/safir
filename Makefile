SHELL       := /bin/bash

ensure:: prepare_protos

common_dotnet_docker::
	cd src && docker build . -f common/dotnet/Dockerfile

agent:: ensure
	dotnet build src/agent

agent_docker:: ensure
	docker build src -f src/agent/Dockerfile
