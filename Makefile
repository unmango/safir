SHELL       := /bin/bash

AGENT_TAG			:= safir-agent
CLI_TAG				:= safir-cli
MANAGER_TAG			:= safir-manager
COMMON_DOTNET_TAG	:= safir-common-dotnet
COMMON_NODE_TAG		:= safir-common-node
UI_TAG				:= safir-ui

GIT_ROOT	?= $(shell pwd)
WORK_DIR	:= ${GIT_ROOT}/work

DOCKER_ARGS =

ifeq ($(DOCKER_DEBUG), true)
DOCKER_ARGS += --progress=plain
endif

all:: build

restore::
	dotnet restore
	yarn install

build:: restore
	dotnet build
	yarn build

clean:: clean_dotnet clean_node clean_work compose_down

docker:: agent_docker manager_docker ui_docker cli_docker

compose:: docker_mounts
	docker-compose pull && docker-compose build

compose_up:: compose
	-docker-compose up

compose_down::
	docker-compose down

clean_dotnet::
	dotnet clean

# TODO
clean_node::
	-rm -rf src/ui/build

clean_work::
	-rm -rf ${WORK_DIR}

common_dotnet::
	dotnet build src/common/dotnet

common_dotnet_docker::
	cd src && docker build . \
		-f common/dotnet/Dockerfile \
		-t ${COMMON_DOTNET_TAG} \
		${DOCKER_ARGS}

restore_common_node::
	cd src/common/node && yarn install

common_node:: restore_common_node
	cd src/common/node && yarn build

common_node_docker::
	docker build . \
		-f src/common/node/Dockerfile \
		-t ${COMMON_NODE_TAG} \
		${DOCKER_ARGS}

agent::
	dotnet build src/agent

agent_docker:: common_dotnet_docker
	cd src && docker build . \
		-f agent/Dockerfile \
		--build-arg CommonImage=${COMMON_DOTNET_TAG} \
		-t ${AGENT_TAG} \
		${DOCKER_ARGS}

start_agent_docker:: agent_docker docker_mounts
	docker run -it --rm \
		--name safir-agent \
		-v ${WORK_DIR}/agent/data:/data \
		${AGENT_TAG}

cli::
	dotnet build src/cli --nologo

cli_docker::
	cd src && docker build . \
		-f cli/Dockerfile \
		--build-arg CommonImage=${COMMON_DOTNET_TAG} \
		-t ${CLI_TAG} \
		${DOCKER_ARGS}

manager::
	dotnet build src/manager

manager_docker:: common_dotnet_docker
	cd src && docker build . \
		-f manager/Dockerfile \
		--build-arg CommonImage=${COMMON_DOTNET_TAG} \
		-t ${MANAGER_TAG} \
		${DOCKER_ARGS}

start_manager_docker:: manager_docker docker_mounts
	docker run -it --rm ${MANAGER_TAG}

restore_ui::
	cd src/ui && yarn install

ui:: restore_ui
	cd src/ui && yarn build

ui_docker::
	docker build . \
		-f src/ui/Dockerfile \
		-t ${UI_TAG} \
		${DOCKER_ARGS}

start_ui_docker:: ui_docker
	docker run -it --rm -p 8080:80 ${UI_TAG}

docker_mounts::
	mkdir -p ${WORK_DIR}/agent/{data,}
