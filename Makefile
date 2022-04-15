SHELL       := /bin/bash

AGENT_TAG			:= safir-agent
MANAGER_TAG			:= safir-manager
COMMON_DOTNET_TAG	:= safir-common-dotnet

common_dotnet_docker::
	cd src && docker build . \
		-f common/dotnet/Dockerfile \
		-t ${COMMON_DOTNET_TAG}

agent::
	dotnet build src/agent

agent_docker:: common_dotnet_docker
	cd src && docker build . \
		-f agent/Dockerfile \
		--build-arg CommonImage=${COMMON_DOTNET_TAG} \
		-t ${AGENT_TAG}

start_agent_docker:: agent_docker
	docker run -it --rm ${AGENT_TAG}

manager::
	dotnet build src/manager

manager_docker:: common_dotnet_docker
	cd src && docker build . \
		-f manager/Dockerfile \
		--build-arg CommonImage=${COMMON_DOTNET_TAG} \
		-t ${MANAGER_TAG}

start_manager_docker:: manager_docker
	docker run -it --rm ${MANAGER_TAG}
